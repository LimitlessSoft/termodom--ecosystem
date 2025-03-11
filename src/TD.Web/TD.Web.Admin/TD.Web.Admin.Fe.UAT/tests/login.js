import { webDbClientFactory } from '../configs/dbConfig.js'
import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import usersHelpers from '../helpers/usersHelpers.js'
import { vaultClient } from '../configs/vaultConfig.js'
import assert from 'assert'

const { TEST_USER_PLAIN_PASSWORD } = await vaultClient.getSecret(
    'web/admin/api'
)

const webDbClient = await webDbClientFactory.create()
const state = {}

export default {
    beforeExecution: async () => {
        const { Username: username } = await usersHelpers.createMockUser(
            webDbClient
        )

        state.username = username
    },
    afterExecution: async () => {
        if (state.username) {
            await usersHelpers.hardDeleteMockUser(webDbClient, state.username)
        }

        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

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
                By.xpath(
                    `//*[@id="__next"]/div/main/div[3]/div/div[2]/div/button`
                )
            )
        )

        await loginButton.click()

        await driver.sleep(1000)

        const releaseNotesLabel = await (
            await driver.wait(
                until.elementLocated(
                    By.xpath(
                        `//*[@id="__next"]/div/main/div[3]/div/div[2]/div[1]/div/p`
                    )
                ),
                WAIT_TIMEOUT
            )
        ).getText()

        console.log(TEST_USER_PLAIN_PASSWORD, state, releaseNotesLabel)

        assert.equal(releaseNotesLabel, 'Release notes')
    },
}
