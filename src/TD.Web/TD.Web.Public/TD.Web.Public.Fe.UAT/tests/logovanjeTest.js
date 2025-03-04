import { PROJECT_URL, PUBLIC_API_CLIENT, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import usersHelpers from '../helpers/usersHelpers.js'
import { GenerateAutomationWebDbClient } from '../configs/dbConfig.js'

const webDbClient = await GenerateAutomationWebDbClient()
const state = {}

export default {
    beforeExecution: async () => {
        await usersHelpers.registerAndConfirmUser(
            webDbClient,
            (username, password) => {
                state.username = username
                state.password = password
            }
        )
    },
    afterExecution: async () => {
        await webDbClient.usersRepository.hardDelete(state.username)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        const profiKutakButtonLocator = By.xpath(
            `//*[@id="header-wrapper"]/a[5]`
        )
        await driver.wait(
            until.elementLocated(profiKutakButtonLocator),
            WAIT_TIMEOUT
        )
        const profiKutakButton = await driver.findElement(
            profiKutakButtonLocator
        )
        await profiKutakButton.click()

        const usernameInputLocator = By.xpath(`//*[@id="username"]`)
        await driver.wait(
            until.elementLocated(usernameInputLocator),
            WAIT_TIMEOUT
        )
        const usernameInput = await driver.findElement(usernameInputLocator)
        await usernameInput.sendKeys(state.username)

        const passwordInputLocator = By.xpath(`//*[@id="password"]`)
        await driver.wait(
            until.elementLocated(passwordInputLocator),
            WAIT_TIMEOUT
        )
        const passwordInput = await driver.findElement(passwordInputLocator)
        await passwordInput.sendKeys(state.password)

        const loginButtonLocator = By.xpath(
            `//*[@id="__next"]/div/main/div[2]/div/button[1]`
        )
        await driver.wait(
            until.elementLocated(loginButtonLocator),
            WAIT_TIMEOUT
        )
        const loginButton = await driver.findElement(loginButtonLocator)
        await loginButton.click()

        const welcomeMessageLocator = By.xpath(
            `//*[@id="__next"]/div/main/div[2]/h6`
        )
        await driver.wait(
            until.elementLocated(welcomeMessageLocator),
            WAIT_TIMEOUT
        )
        await driver.sleep(1000)
        const welcomeMessage = await driver.findElement(welcomeMessageLocator)
        const message = await welcomeMessage.getText()

        await assert.equal(message, `Dobrodošao ${state.username}`)
    },
}
