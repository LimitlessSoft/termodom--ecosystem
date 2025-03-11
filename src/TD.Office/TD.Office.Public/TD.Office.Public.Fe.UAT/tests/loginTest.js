import { officeDbClientFactory } from '../configs/dbConfig.js'
import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import usersHelpers from '../helpers/usersHelpers.js'
import { vaultClient } from '../configs/vaultConfig.js'
import assert from 'assert'

const { TEST_USER_PLAIN_PASSWORD } = await vaultClient.getSecret(
    'office/public/api'
)

const officeDbClient = await officeDbClientFactory.create()
const state = {}

export default {
    beforeExecution: async () => {
        const { Username: username } = await usersHelpers.createMockUser(
            officeDbClient
        )
        state.username = username
    },
    afterExecution: async () => {
        // if (state.username) {
        //     await usersHelpers.hardDeleteMockUser(
        //         officeDbClient,
        //         state.username
        //     )
        // }

        await officeDbClient.disconnect()
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
                By.xpath(`//*[@id="__next"]/div/main/div/div/div[2]/div/button`)
            ),
            WAIT_TIMEOUT
        )
        await loginButton.click()

        console.log(TEST_USER_PLAIN_PASSWORD, state)

        await driver.sleep(5000)

        const firstNoteLabel = await (
            await driver.wait(
                until.elementLocated(
                    By.xpath(
                        `//*[@id="__next"]/div/main/div[3]/div/div[2]/div[2]/div/div[1]/div[2]/div/button`
                    )
                ),
                WAIT_TIMEOUT
            )
        ).getText()

        console.log(firstNoteLabel)

        assert.equal(firstNoteLabel, `FIRST NOTE`)
    },
}
