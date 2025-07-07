import { officeDbClientFactory } from '../configs/dbConfig.js'
import { PROJECT_URL, ELEMENT_AWAITER_TIMEOUT } from '../constants.js'
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
        await usersHelpers.hardDeleteMockUser(officeDbClient, state.username)
        await officeDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(`${PROJECT_URL}/logovanje`)

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

        const leftMenuLayout = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="__next"]/div/main/div[2]`)),
            ELEMENT_AWAITER_TIMEOUT
        )

        const actions = driver.actions({ actions: true })
        await actions.move({ origin: leftMenuLayout }).perform()

        const logoutButtonLabel = await (
            await driver.wait(
                until.elementLocated(By.xpath(`//*[@id="logout"]`)),
                ELEMENT_AWAITER_TIMEOUT
            )
        ).getText()

        assert.equal(logoutButtonLabel, `Odjavi se`)
    },
}
