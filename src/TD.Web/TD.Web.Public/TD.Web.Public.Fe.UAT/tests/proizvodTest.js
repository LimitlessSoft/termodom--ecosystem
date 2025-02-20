import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'

export default {
    beforeExecution: () => {},
    afterExecution: () => {},
    execution: async (driver) => {
        // await driver.get(PROJECT_URL)
        //
        // const productLocator = By.xpath(
        //     '/html/body/div/div/main/div[2]/div/div[4]/div/div[1]/div[1]/a/div/button'
        // )
        // await driver.wait(
        //     until.elementLocated(productLocator),
        //     WAIT_TIMEOUT * 3
        // )
        // const product = await driver.findElement(productLocator)
        // await product.click()
        //
        // const pageLocator = By.xpath(`/html/body/div/div/main/div[2]/div`)
        // await driver.wait(until.elementLocated(pageLocator), WAIT_TIMEOUT * 3)
        // const page = await driver.findElement(pageLocator)
        //
        // const hasAnyChild = await page
        //     .findElements(By.xpath('./*'))
        //     .then((elements) => elements.length > 0)
        // assert(hasAnyChild)
    },
}
