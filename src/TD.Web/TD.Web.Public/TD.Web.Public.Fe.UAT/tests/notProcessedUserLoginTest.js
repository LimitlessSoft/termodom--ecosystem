import { PROJECT_URL, ELEMENT_AWAITER_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import usersHelpers from '../helpers/usersHelpers.js'
import { webDbClientFactory } from '../configs/dbConfig.js'
import { vaultClient } from '../configs/vaultConfig.js'

const { TEST_USER_PLAIN_PASSWORD } = await vaultClient.getSecret(
    'web/public/api'
)

const webDbClient = await webDbClientFactory.create()
const state = {}

export default {
    beforeExecution: async () => {
        await usersHelpers.registerMockUser((username) => {
            state.username = username
        })
    },
    afterExecution: async () => {
        await webDbClient.usersRepository.hardDeleteByUsername(state.username)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        const profiKutakButton = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="header-wrapper"]/a[5]`)),
            ELEMENT_AWAITER_TIMEOUT
        )
        await profiKutakButton.click()

        const usernameInput = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="username"]`)),
            ELEMENT_AWAITER_TIMEOUT
        )
        await usernameInput.sendKeys(state.username)

        const passwordInput = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="password"]`)),
            ELEMENT_AWAITER_TIMEOUT
        )
        await passwordInput.sendKeys(TEST_USER_PLAIN_PASSWORD)

        const loginButton = await driver.wait(
            until.elementLocated(
                By.xpath(`//*[@id="__next"]/div/main/div[2]/div/button[1]`)
            ),
            ELEMENT_AWAITER_TIMEOUT
        )
        await loginButton.click()

        const toastErrorElement = await driver.wait(
            until.elementLocated(By.xpath(`//*[@role="alert"]/div[2]`)),
            ELEMENT_AWAITER_TIMEOUT
        )

        await driver.wait(
            until.elementIsVisible(toastErrorElement),
            ELEMENT_AWAITER_TIMEOUT
        )

        const errorMessage = await toastErrorElement.getText()

        assert.equal(errorMessage, `Nemate pravo pristupa`)
    },
}
