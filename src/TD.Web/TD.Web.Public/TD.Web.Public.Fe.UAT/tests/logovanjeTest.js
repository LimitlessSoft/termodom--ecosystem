import { PROJECT_URL, PUBLIC_API_CLIENT, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import { GenerateWebDbClient } from '../db.js'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const webDbClient = await GenerateWebDbClient()
const username = faker.string.fromCharacters('abcdefghijklmnopqrstuvwxyz', 10)
const password = 'Test251f11f2123!'

export default {
    beforeExecution: async () => {
        await PUBLIC_API_CLIENT.users.registerUser(
            username,
            password,
            faker.date.birthdate({ min: 18, max: 65, mode: 'age' }),
            `06${(10000000 + Math.floor(Math.random() * 90000000))}`,
            faker.location.street(),
            3,
            121,
            faker.internet.email()
        )
        // await webDbClient.userRepository.setProcessingDate(username, new Date())
    },
    afterExecution: async () => {
        // await webDbClient.userRepository.hardDelete(username)
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
        await usernameInput.sendKeys(username)

        const passwordInputLocator = By.xpath(`//*[@id="password"]`)
        await driver.wait(
            until.elementLocated(passwordInputLocator),
            WAIT_TIMEOUT
        )
        const passwordInput = await driver.findElement(passwordInputLocator)
        await passwordInput.sendKeys(password)

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

        console.log(`Ovo je poruka: ${message}`)
        await assert.equal(message, `Dobrodošao ${username}`)
    },
}
