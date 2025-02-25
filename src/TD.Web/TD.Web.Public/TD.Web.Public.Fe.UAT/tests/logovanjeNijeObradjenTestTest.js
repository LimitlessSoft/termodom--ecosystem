import { PROJECT_URL, PUBLIC_API_CLIENT, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import { GenerateWebDbClient } from '../db.js'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'
import usersHelpers from '../helpers/usersHelpers.js'

const webDbClient = await GenerateWebDbClient()
const state = {}

export default {
    beforeExecution: async () => {
        await usersHelpers.registerUser(webDbClient, (username,  password) => {
            state.username = username
            state.password = password
        })
    },
    afterExecution: async () => {
        await webDbClient.userRepository.hardDelete(state.username)
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

        const errorMessageLocator = By.xpath(
            `/html/body/div[1]/div/main/div[1]/div/div/div[1]/div[2]`
        )
        await driver.wait(
            until.elementLocated(errorMessageLocator),
            WAIT_TIMEOUT
        )
        await driver.sleep(1000)
        const errorMessage = await driver.findElement(errorMessageLocator)
        const message = await errorMessage.getText()

        await assert.equal(message, `Nemate pravo pristupa`)
    },
}
