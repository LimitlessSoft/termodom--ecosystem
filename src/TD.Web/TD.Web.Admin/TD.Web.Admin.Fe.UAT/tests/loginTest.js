import { webDbClientFactory } from '../configs/dbConfig.js'
import { PROJECT_URL, ELEMENT_AWAITER_TIMEOUT } from '../constants.js'
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
        await webDbClient.usersRepository.hardDeleteByUsername(state.username)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

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
                By.xpath(`//*[@id="__next"]/div/main/div/div/div[2]/div/button`)
            ),
            ELEMENT_AWAITER_TIMEOUT
        )

        await loginButton.click()

        const releaseNotesLabel = await (
            await driver.wait(
                until.elementLocated(
                    By.xpath(
                        `//*[@id="__next"]/div/main/div[3]/div/div[2]/div[1]/div/p`
                    )
                ),
                ELEMENT_AWAITER_TIMEOUT
            )
        ).getText()

        assert.equal(releaseNotesLabel, 'Release notes')
    },
}
