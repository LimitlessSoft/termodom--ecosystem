import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
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
        if (state.username) {
            await usersHelpers.hardDeleteMockUser(webDbClient, state.username)
        }

        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        const profiKutakButton = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="header-wrapper"]/a[5]`)),
            WAIT_TIMEOUT
        )
        await profiKutakButton.click()

        const usernameInput = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="username"]`)),
            WAIT_TIMEOUT
        )
        await usernameInput.sendKeys(state.username)

        const passwordInput = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="password"]`)),
            WAIT_TIMEOUT
        )
        await passwordInput.sendKeys(TEST_USER_PLAIN_PASSWORD)

        const loginButton = await driver.wait(
            until.elementLocated(
                By.xpath(`//*[@id="__next"]/div/main/div[2]/div/button[1]`)
            ),
            WAIT_TIMEOUT
        )
        await loginButton.click()

        await driver.sleep(1000)

        const errorMessage = await (
            await driver.wait(
                until.elementLocated(
                    By.xpath(
                        `/html/body/div[1]/div/main/div[1]/div/div/div[1]/div[2]`
                    ),
                    WAIT_TIMEOUT
                )
            )
        ).getText()

        assert.equal(errorMessage, `Nemate pravo pristupa`)
    },
}
