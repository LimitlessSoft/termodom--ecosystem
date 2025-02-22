import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import { webDb } from '../vaultConfig.js'
import deleteTestUserQuery from '../queries/deleteTestUserQuery.js'
import registerUser from '../utils/registerUser.js'

const TEST_USER_USERNAME = 'logovanje'

const client = await webDb.connect()

export default {
    beforeExecution: async () => {
        await registerUser(TEST_USER_USERNAME, client)
    },
    afterExecution: async () => {
        await client.query(deleteTestUserQuery(TEST_USER_USERNAME))
        client.end()
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
        await usernameInput.sendKeys(TEST_USER_USERNAME)

        const passwordInputLocator = By.xpath(`//*[@id="password"]`)
        await driver.wait(
            until.elementLocated(passwordInputLocator),
            WAIT_TIMEOUT
        )
        const passwordInput = await driver.findElement(passwordInputLocator)
        await passwordInput.sendKeys('Test123!')

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

        await assert.equal(message, `Dobrodošao ${TEST_USER_USERNAME}`)
    },
}
