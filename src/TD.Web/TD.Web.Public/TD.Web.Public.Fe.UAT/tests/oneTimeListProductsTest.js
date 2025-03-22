import { webDbClientFactory } from '../configs/dbConfig.js'
import productsHelpers from '../helpers/productsHelpers.js'
import productPricesHelpers from '../helpers/productPricesHelpers.js'
import unitsHelpers from '../helpers/unitsHelpers.js'
import productPriceGroupsHelpers from '../helpers/productPriceGroupsHelpers.js'
import imagesHelpers from '../helpers/imagesHelpers.js'
import { ELEMENT_AWAITER_TIMEOUT, PROJECT_URL } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'

const state = {}
const webDbClient = await webDbClientFactory.create()

export default {
    beforeExecution: async () => {
        const {
            product,
            unit,
            productPriceGroup,
            imageFilename,
            productPrice,
        } = await productsHelpers.createProduct(webDbClient)

        state.product = product
        state.unit = unit
        state.productPriceGroup = productPriceGroup
        state.imageFilename = imageFilename
        state.productPrice = productPrice
    },
    afterExecution: async () => {
        await webDbClient.productPricesRepository.hardDelete(state.productPrice.Id)
        await webDbClient.productsRepository.hardDelete(state.product.Id)
        await webDbClient.unitsRepository.hardDelete(state.unit.id)
        await webDbClient.productPriceGroupsRepository.hardDelete(state.productPriceGroup.Id)
        await imagesHelpers.removeImageFromMinio(state.imageFilename)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)
        
        const firstProductCardButtonLocator = By.xpath('/html/body/div/div/main/div[2]/div/div[4]/div[1]//button')
        await driver.wait(
            until.elementLocated(firstProductCardButtonLocator),
            ELEMENT_AWAITER_TIMEOUT,
            "Haven't found any product cards on main page"
        )
    },
}
