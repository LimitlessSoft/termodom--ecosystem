import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import { webDbClientFactory } from '../configs/dbConfig.js'
import productGroupsHelpers from '../helpers/productGroupsHelpers.js'

const webDbClient = await webDbClientFactory.create()

const expected = {}
const state = {}

export default {
    beforeExecution: async () => {
        const {
            Id: productGroupId,
            Name: productGroupName,
            Src: productGroupSrc,
        } = await productGroupsHelpers.createMockProductGroup(webDbClient, {
            parentId: null,
        })

        state.productGroupId = productGroupId

        expected.productGroup = {
            name: productGroupName,
            src: productGroupSrc,
        }
    },
    afterExecution: async () => {
        if (state.productGroupId) {
            await productGroupsHelpers.hardDeleteMockProductGroup(
                webDbClient,
                state.productGroupId
            )
        }

        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        const groupButton = await driver.wait(
            until.elementLocated(
                By.xpath(
                    `//*[@id="__next"]/div/main/div[2]/div/div[1]/div[./button[text()="${expected.productGroup.name}"]]`
                )
            ),
            WAIT_TIMEOUT
        )
        await groupButton.click()

        const backButtonText = await (
            await driver.wait(
                until.elementLocated(
                    By.xpath(
                        `/html/body/div/div/main/div[2]/div/div[1]/div/button`
                    )
                ),
                WAIT_TIMEOUT
            )
        ).getText()

        assert.strictEqual(backButtonText, '')

        const currentUrl = await driver.getCurrentUrl()
        assert.ok(currentUrl.includes(expected.productGroup.src))
    },
}
