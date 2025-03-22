import { ELEMENT_AWAITER_TIMEOUT, PROJECT_URL } from '../constants.js'
import unitsHelpers from '../helpers/unitsHelpers.js'
import { webDbClientFactory } from '../configs/dbConfig.js'
import imagesHelpers from '../helpers/imagesHelpers.js'
import productPriceGroupsHelpers from '../helpers/productPriceGroupsHelpers.js'
import productsHelpers from '../helpers/productsHelpers.js'
import productPricesHelpers from '../helpers/productPricesHelpers.js'
import { By, until } from 'selenium-webdriver'
import usersHelpers from '../helpers/usersHelpers.js'
import { vaultClient } from '../configs/vaultConfig.js'
import assert from 'assert'
const { TEST_USER_PLAIN_PASSWORD } = await vaultClient.getSecret(
    'web/public/api'
)
const state = {
    user: { password: TEST_USER_PLAIN_PASSWORD },
}
const webDbClient = await webDbClientFactory.create()

export default {
    beforeExecution: async () => {
        const { Username: username, Password: password } =
            await usersHelpers.registerAndConfirmMockUser(webDbClient)
        state.user.username = username

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

        const profiKutakButtonLocator = By.xpath(
            '/html/body/div/div/header/div[2]/a[5]'
        )
        const profiKutakButton = await driver.wait(
            until.elementLocated(profiKutakButtonLocator),
            ELEMENT_AWAITER_TIMEOUT
        )
        await driver.wait(until.elementIsVisible(profiKutakButton), ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsEnabled(profiKutakButton), ELEMENT_AWAITER_TIMEOUT)
        await profiKutakButton.click()
        
        const usernameInputLocator = By.xpath('//*[@id="username"]')
        const usernameInput = await driver.wait(
            until.elementLocated(usernameInputLocator),
            ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsVisible(usernameInput), ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsEnabled(usernameInput), ELEMENT_AWAITER_TIMEOUT)
        await usernameInput.sendKeys(state.user.username)
        
        const passwordInputLocator = By.xpath('//*[@id="password"]')
        const passwordInput = await driver.wait(
            until.elementLocated(passwordInputLocator),
            ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsVisible(passwordInput), ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsEnabled(passwordInput), ELEMENT_AWAITER_TIMEOUT)
        await passwordInput.sendKeys(state.user.password)
        
        const loginButtonLocator = By.xpath('/html/body/div/div/main/div[2]/div/button[1]')
        const loginButton = await driver.wait(
            until.elementLocated(loginButtonLocator),
            ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsVisible(loginButton), ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsEnabled(loginButton), ELEMENT_AWAITER_TIMEOUT)
        await loginButton.click()
        
        await driver.wait(until.elementLocated(By.xpath('/html/body/div/div/main/div[2]/h6')), ELEMENT_AWAITER_TIMEOUT)
        
        await driver.get(PROJECT_URL + '/proizvodi/' + state.product.Src)
        
        const dodajUKorpuButtonLocator = By.xpath('/html/body/div/div/main/div[2]/div/div[2]/div[2]/div/div[1]/button')
        const dodajUKorpuButton = await driver.wait(
            until.elementLocated(dodajUKorpuButtonLocator),
            ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsVisible(dodajUKorpuButton), ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsEnabled(dodajUKorpuButton), ELEMENT_AWAITER_TIMEOUT)
        await dodajUKorpuButton.click()
        
        const zakljuciPorudzbinuButtonLocator = By.xpath('/html/body/div/div/main/div[2]/div[5]/div/button')
        const zakljuciPorudzbinuButton = await driver.wait(
            until.elementLocated(zakljuciPorudzbinuButtonLocator),
            ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsVisible(zakljuciPorudzbinuButton), ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsEnabled(zakljuciPorudzbinuButton), ELEMENT_AWAITER_TIMEOUT)
        await zakljuciPorudzbinuButton.click()
        
        const statusLabelLocator = By.xpath('/html/body/div/div/main/div[2]/div[1]/div/div[1]/p[5]')
        const statusLabel = await driver.wait(
            until.elementLocated(statusLabelLocator),
            ELEMENT_AWAITER_TIMEOUT)
        await driver.wait(until.elementIsVisible(statusLabel), ELEMENT_AWAITER_TIMEOUT)
        assert.equal((await statusLabel.getText()).indexOf('Status:') >= 0, true)
    },
}
