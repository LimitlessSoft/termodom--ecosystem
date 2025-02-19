import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'

export default {
    beforeExecution: () => {},
    afterExecution: () => {},
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        const profiKutakButtonLocator = By.xpath(
            `/html/body/div/div/header/div[2]/a[4]`
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
        await usernameInput.sendKeys('ALEKS13')

        const passwordInputLocator = By.xpath(`//*[@id="password"]`)
        await driver.wait(
            until.elementLocated(passwordInputLocator),
            WAIT_TIMEOUT
        )
        const passwordInput = await driver.findElement(passwordInputLocator)
        await passwordInput.sendKeys('Test123.')

        const loginButtonLocator = By.xpath(
            `/html/body/div/div/main/div[2]/div/button[1]`
        )
        await driver.wait(
            until.elementLocated(loginButtonLocator),
            WAIT_TIMEOUT
        )
        const loginButton = await driver.findElement(loginButtonLocator)
        await loginButton.click()

        const welcomeMessageLocator = By.xpath(
            `/html/body/div/div/main/div[2]/h6`
        )
        await driver.wait(
            until.elementLocated(welcomeMessageLocator),
            WAIT_TIMEOUT
        )
        await driver.sleep(1000)
        const welcomeMessage = await driver.findElement(welcomeMessageLocator)
        const message = await welcomeMessage.getText()

        await assert.equal(message, 'Dobrodošao Aleksa Ristic')
    },
}
