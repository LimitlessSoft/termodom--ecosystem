import { By, until } from 'selenium-webdriver'
import { webDbClientFactory } from '../configs/dbConfig.js'
import productPriceGroupsHelpers from '../helpers/productPriceGroupsHelpers.js'
import productPricesHelpers from '../helpers/productPricesHelpers.js'
import productsHelpers from '../helpers/productsHelpers.js'
import unitsHelpers from '../helpers/unitsHelpers.js'
import { ELEMENT_AWAITER_TIMEOUT, PROJECT_URL } from '../constants.js'
import imagesHelpers from '../helpers/imagesHelpers.js'
import assert from 'assert'
import numbersHelpers from '../helpers/numbersHelpers.js'
import usersHelpers from '../helpers/usersHelpers.js'

const webDbClient = await webDbClientFactory.create()

const state = {}
const expected = {}

export default {
    beforeExecution: async () => {
        const PRODUCT_PRICE_GROUP_LEVEL = 1

        const { Id: unitId } = await unitsHelpers.createMockUnit(webDbClient)

        state.unitId = unitId

        const imageFilename = await imagesHelpers.uploadImageToMinio()

        state.imageFilename = imageFilename

        const { Id: productPriceGroupId } =
            await productPriceGroupsHelpers.createMockProductPriceGroup(
                webDbClient
            )

        state.productPriceGroupId = productPriceGroupId

        const { Id: userId } = await usersHelpers.registerAndConfirmMockUser(
            webDbClient
        )

        await webDbClient.productPriceGroupLevelRepository.create({
            userId,
            level: PRODUCT_PRICE_GROUP_LEVEL,
            productPriceGroupId,
        })

        const {
            Id: productId,
            VAT,
            Src: productSrc,
        } = await productsHelpers.createMockProduct(webDbClient, {
            unitId,
            productPriceGroupId,
            imageFilename,
        })

        state.productId = productId
        expected.productSrc = productSrc

        const { Id: priceId, Max: maxPrice } =
            await productPricesHelpers.createMockProductPrice(webDbClient, {
                productId,
            })

        state.priceId = priceId
        expected.maxPrice = numbersHelpers.formatNumber(
            maxPrice * (1 + VAT / 100)
        )
    },
    afterExecution: async () => {
        if (state.productPriceId) {
            await productPricesHelpers.hardDeleteMockProductPrice(
                webDbClient,
                state.productPriceId
            )
        }

        if (state.productId) {
            await productsHelpers.hardDeleteMockProduct(
                webDbClient,
                state.productId
            )
        }

        if (state.unitId) {
            await unitsHelpers.hardDeleteMockUnit(webDbClient, state.unitId)
        }

        if (state.productPriceGroupId) {
            await productPriceGroupsHelpers.hardDeleteMockProductPriceGroup(
                webDbClient,
                state.productPriceGroupId
            )
        }

        if (state.imageFilename) {
            await imagesHelpers.removeImageFromMinio(state.imageFilename)
        }

        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(`${PROJECT_URL}/proizvodi/${expected.productSrc}`)

        const maxPrice = await (
            await driver.wait(
                until.elementLocated(
                    By.xpath(
                        `//*[@id="__next"]/div/main/div[2]/div/div[2]/div[2]/div/div[1]/div[1]/div/div[1]/h6/span[3]`
                    )
                ),
                ELEMENT_AWAITER_TIMEOUT
            )
        ).getText()

        assert.equal(
            `Max price: ${maxPrice}, Min price: ${minPrice}`,
            `Max price: ${expected.maxPrice}, Min price: ${expected.minPrice}`
        )

        assert.equal(maxPrice, expected.maxPrice)
    },
}
