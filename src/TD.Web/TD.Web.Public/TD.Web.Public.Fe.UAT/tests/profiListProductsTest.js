import { webDbClientFactory } from '../configs/dbConfig.js'
import productsHelpers from '../helpers/productsHelpers.js'
import productPricesHelpers from '../helpers/productPricesHelpers.js'
import unitsHelpers from '../helpers/unitsHelpers.js'
import productPriceGroupsHelpers from '../helpers/productPriceGroupsHelpers.js'
import imagesHelpers from '../helpers/imagesHelpers.js'
import { ELEMENT_AWAITER_TIMEOUT, PROJECT_URL, PUBLIC_API_CLIENT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import usersHelpers from '../helpers/usersHelpers.js'
import { vaultClient } from '../configs/vaultConfig.js'

const state = { token: '' }
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

        const { TEST_USER_PLAIN_PASSWORD } = await vaultClient.getSecret(
            'web/public/api'
        )
        const { Username: username } = await usersHelpers.registerAndConfirmMockUser(webDbClient)
        state.username = username
        state.token = await PUBLIC_API_CLIENT.users.login({ username, password: TEST_USER_PLAIN_PASSWORD })
    },
    afterExecution: async () => {
        await productPricesHelpers.hardDeleteMockProductPrice(
            webDbClient,
            state.productPrice.Id
        )
        await productsHelpers.hardDeleteMockProduct(
            webDbClient,
            state.product.Id
        )
        await unitsHelpers.hardDeleteMockUnit(webDbClient, state.unit.Id)
        await productPriceGroupsHelpers.hardDeleteMockProductPriceGroup(
            webDbClient,
            state.productPriceGroup.Id
        )
        if (state.imageFilename)
            await imagesHelpers.removeImageFromMinio(state.imageFilename)

        await usersHelpers.hardDelete(webDbClient, state.username)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        await driver.manage().addCookie({
            name: 'token',
            value: state.token,
            path: '/',
        })
        
        await driver.sleep(500)
        await driver.get(PROJECT_URL)
        
        const firstProductCardButtonLocator = By.xpath('/html/body/div/div/main/div[2]/div/div[3]/div[1]//a')
        await driver.wait(
            until.elementLocated(firstProductCardButtonLocator),
            ELEMENT_AWAITER_TIMEOUT,
            "Haven't found any product cards on main page"
        )
    },
}
