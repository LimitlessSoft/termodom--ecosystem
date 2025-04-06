import { By, until } from 'selenium-webdriver'
import { webDbClientFactory } from '../configs/dbConfig.js'
import productPriceGroupsHelpers from '../helpers/productPriceGroupsHelpers.js'
import productPricesHelpers from '../helpers/productPricesHelpers.js'
import productsHelpers from '../helpers/productsHelpers.js'
import unitsHelpers from '../helpers/unitsHelpers.js'
import {
    ELEMENT_AWAITER_TIMEOUT,
    PERCENT_OF_DIFFERENCE_TO_PLAY_WITH,
    PROJECT_URL,
} from '../constants.js'
import imagesHelpers from '../helpers/imagesHelpers.js'
import assert from 'assert'
import numbersHelpers from '../helpers/numbersHelpers.js'

const webDbClient = await webDbClientFactory.create()

const state = {}
const expected = {}

export default {
    beforeExecution: async () => {
        const { Id: unitId } = await unitsHelpers.createMockUnit(webDbClient)

        state.unitId = unitId

        const imageFilename = await imagesHelpers.uploadImageToMinio()

        state.imageFilename = imageFilename

        const { Id: productPriceGroupId } =
            await productPriceGroupsHelpers.createMockProductPriceGroup(
                webDbClient
            )

        state.productPriceGroupId = productPriceGroupId

        const {
            Id: productId,
            VAT,
            Src: productSrc,
        } = await productsHelpers.createMockProductCore(webDbClient, {
            unitId,
            productPriceGroupId,
            imageFilename,
        })

        state.productId = productId
        expected.productSrc = productSrc

        const {
            Id: priceId,
            Max: maxPrice,
            Min: minPrice,
        } = await productPricesHelpers.createMockProductPrice(webDbClient, {
            productId,
        })

        state.priceId = priceId
        expected.minPrice = numbersHelpers.formatNumber(
            (maxPrice -
                (maxPrice - minPrice) * PERCENT_OF_DIFFERENCE_TO_PLAY_WITH) *
                (1 + VAT / 100)
        )
    },
    afterExecution: async () => {
        await webDbClient.productPricesRepository.hardDelete(
            state.productPriceId
        )
        await webDbClient.productsRepository.hardDelete(state.productId)
        await webDbClient.unitsRepository.hardDelete(state.unitId)
        await webDbClient.productPriceGroupsRepository.hardDelete(
            state.productPriceGroupId
        )
        await imagesHelpers.removeImageFromMinio(state.imageFilename)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(`${PROJECT_URL}/proizvodi/${expected.productSrc}`)

        const minPrice = await (
            await driver.wait(
                until.elementLocated(
                    By.xpath(
                        `//*[@id="__next"]/div/main/div[2]/div/div[2]/div[2]/div/div[1]/div[1]/div/div[1]/h6/span[2]`
                    )
                ),
                ELEMENT_AWAITER_TIMEOUT
            )
        ).getText()

        assert.equal(minPrice, expected.minPrice)
    },
}
