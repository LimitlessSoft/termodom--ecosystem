import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import { webDb } from '../vaultConfig.js'
import deleteTestUserQuery from '../queries/deleteTestUserQuery.js'
import registerUser from '../utils/registerUser.js'

const TEST_USER_USERNAME = 'proizvod'

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

        const productLocator = By.xpath(
            '//*[@id="__next"]/div/main/div[2]/div/div[3]/div[1]/div[1]'
        )
        await driver.wait(
            until.elementLocated(productLocator),
            WAIT_TIMEOUT * 3
        )
        const product = await driver.findElement(productLocator)
        await product.click()

        const pageLocator = By.xpath(`//*[@id="__next"]/div/main/div[2]/div`)
        await driver.wait(until.elementLocated(pageLocator), WAIT_TIMEOUT * 3)
        const page = await driver.findElement(pageLocator)

        const hasAnyChild = await page
            .findElements(By.xpath('./*'))
            .then((elements) => elements.length > 0)
        assert(hasAnyChild)
    },
}
