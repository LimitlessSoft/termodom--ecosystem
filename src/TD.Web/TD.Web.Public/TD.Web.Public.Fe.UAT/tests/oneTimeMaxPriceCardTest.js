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
            Name: productName,
            VAT,
        } = await productsHelpers.createMockProductCore(webDbClient, {
            unitId,
            productPriceGroupId,
            imageFilename,
        })

        state.productId = productId
        expected.productName = productName

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
        await driver.get(PROJECT_URL)

        const productElement = await driver.wait(
            until.elementLocated(
                By.xpath(
                    `//*[@id="__next"]/div/main/div[2]/div/div[4]/div[1]/div[./a/div/button/div/div[1]/p[text()="${expected.productName}"]]`
                )
            ),
            ELEMENT_AWAITER_TIMEOUT
        )

        const maxPrice = await (
            await productElement.findElement(
                By.xpath(`./a/div/button/div/div[2]/div[2]/span[2]`)
            )
        ).getText()

        assert.equal(maxPrice, `${expected.maxPrice} RSD`)
    },
}
