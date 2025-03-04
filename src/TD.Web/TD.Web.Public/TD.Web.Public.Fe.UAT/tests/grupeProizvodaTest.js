import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'
import { GenerateAutomationWebDbClient } from '../configs/dbConfig.js'

const webDbClient = await GenerateAutomationWebDbClient()

const expected = {
    productGroup: {
        name: faker.string.fromCharacters('abcdefghijklmnopqrstuvwxyz', 10),
        parentId: null,
        src: faker.string.fromCharacters('abcdefghijklmnopqrstuvwxyz', 10),
    },
}

export default {
    beforeExecution: async () => {
        await webDbClient.productGroupsRepository.create(
            expected.productGroup.name,
            null,
            expected.productGroup.src
        )
    },
    afterExecution: async () => {
        await webDbClient.productGroupsRepository.hardDelete(
            expected.productGroup.name
        )
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        const buttonGroupLocator = By.xpath(
            `/html/body/div/div/main/div[2]/div/div[1]//div[./button[text()="${expected.productGroup.name}"]]`
        )
        await driver.wait(
            until.elementLocated(buttonGroupLocator),
            WAIT_TIMEOUT
        )

        const buttonGroup = await driver.findElement(buttonGroupLocator)
        await buttonGroup.click()

        await driver.sleep(1000)

        const backButtonLocator = By.xpath(
            `/html/body/div/div/main/div[2]/div/div[1]/div/button`
        )
        await driver.wait(until.elementLocated(backButtonLocator), WAIT_TIMEOUT)

        const backButtonText = await (
            await driver.findElement(backButtonLocator)
        ).getText()
        assert.strictEqual(backButtonText, '')

        const currentUrl = await driver.getCurrentUrl()
        assert.ok(currentUrl.includes(expected.productGroup.src))
    },
}
