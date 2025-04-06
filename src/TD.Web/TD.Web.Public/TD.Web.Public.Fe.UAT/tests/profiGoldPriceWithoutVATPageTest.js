import { By, until } from 'selenium-webdriver'
import { webDbClientFactory } from '../configs/dbConfig.js'
import { vaultClient } from '../configs/vaultConfig.js'
import {
    ELEMENT_AWAITER_TIMEOUT,
    PRICE_GROUP_LEVELS,
    PROJECT_URL,
    PUBLIC_API_CLIENT,
} from '../constants.js'
import imagesHelpers from '../helpers/imagesHelpers.js'
import productPricesHelpers from '../helpers/productPricesHelpers.js'
import productsHelpers from '../helpers/productsHelpers.js'
import usersHelpers from '../helpers/usersHelpers.js'
import assert from 'assert'

const webDbClient = await webDbClientFactory.create()

const state = {}
const expected = {}

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
        const { Id: userId, Username: username } =
            await usersHelpers.registerAndConfirmMockUser(webDbClient)

        state.username = username

        const { Id: productPriceGroupLevelId } =
            await webDbClient.productPriceGroupLevelRepository.create({
                userId,
                level: PRICE_GROUP_LEVELS.GOLD,
                productPriceGroupId: productPriceGroup.Id,
            })

        state.productPriceGroupLevelId = productPriceGroupLevelId
        state.token = await PUBLIC_API_CLIENT.users.login({
            username,
            password: TEST_USER_PLAIN_PASSWORD,
        })

        expected.productSrc = product.Src
        expected.productPrice =
            await productPricesHelpers.calculateProfiProductPrice({
                maxPrice: productPrice.Max,
                minPrice: productPrice.Min,
                level: PRICE_GROUP_LEVELS.GOLD,
            })
    },
    afterExecution: async () => {
        await webDbClient.productPricesRepository.hardDelete(
            state.productPrice.Id
        )
        await webDbClient.productsRepository.hardDelete(state.product.Id)
        await webDbClient.unitsRepository.hardDelete(state.unit.Id)
        await webDbClient.productPriceGroupLevelRepository.hardDelete(
            state.productPriceGroupLevelId
        )
        await webDbClient.productPriceGroupsRepository.hardDelete(
            state.productPriceGroup.Id
        )
        await imagesHelpers.removeImageFromMinio(state.imageFilename)
        await webDbClient.usersRepository.hardDeleteByUsername(state.username)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await usersHelpers.uatLogin(driver, state.token)

        await driver.sleep(500)

        await driver.get(`${PROJECT_URL}/proizvodi/${expected.productSrc}`)

        const price = await (
            await driver.wait(
                until.elementLocated(
                    By.xpath(
                        `//*[@id="__next"]/div/main/div[2]/div/div[2]/div[2]/div/div[1]/div[1]/div[1]/h2/strong`
                    )
                ),
                ELEMENT_AWAITER_TIMEOUT
            )
        ).getText()

        assert.equal(price, expected.productPrice)
    },
}
