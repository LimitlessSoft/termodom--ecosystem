import { PROJECT_URL, ELEMENT_AWAITER_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import { webDbClientFactory } from '../configs/dbConfig.js'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const webDbClient = await webDbClientFactory.create()

export default {
    beforeExecution: async () => {},
    afterExecution: async () => {
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(`${PROJECT_URL}/registrovanje`)

        const userTypeSwitchInput = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="user-type-switch"]`)),
            ELEMENT_AWAITER_TIMEOUT
        )
        await userTypeSwitchInput.click()

        const mbInput = await driver.wait(
            until.elementLocated(By.xpath(`//*[@id="mb"]`)),
            ELEMENT_AWAITER_TIMEOUT
        )
        await mbInput.sendKeys(faker.string.numeric({ length: 8 }))

        let errorTextVisible = false
        try {
            const mbInputValidationErrorElement = await driver.findElement(
                By.xpath(`//*[@id="mb-helper-text"]`)
            )
            errorTextVisible = await mbInputValidationErrorElement.isDisplayed()
        } catch {
            errorTextVisible = false
        }

        if (errorTextVisible) {
            throw new Error(
                '❌ Error should not be shown for input longer than 8 digits.'
            )
        }
    },
}
