import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'

export default {
    beforeExecution: () => {},
    afterExecution: () => {},
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        const usernameInputLocator = By.xpath(`//*[@id="username"]`)
        await driver.wait(
            until.elementLocated(usernameInputLocator),
            WAIT_TIMEOUT
        )
        const usernameInput = await driver.findElement(usernameInputLocator)
        await usernameInput.sendKeys('filip')

        const passwordInputLocator = By.xpath(`//*[@id="password"]`)
        await driver.wait(
            until.elementLocated(passwordInputLocator),
            WAIT_TIMEOUT
        )
        const passwordInput = await driver.findElement(passwordInputLocator)
        await passwordInput.sendKeys('123123')

        const loginButtonLocator = By.xpath(
            `//*[@id="__next"]/div/main/div[3]/div/div[2]/div/button`
        )
        await driver.wait(
            until.elementLocated(loginButtonLocator),
            WAIT_TIMEOUT
        )
        const loginButton = await driver.findElement(loginButtonLocator)
        await loginButton.click()
    },
}
