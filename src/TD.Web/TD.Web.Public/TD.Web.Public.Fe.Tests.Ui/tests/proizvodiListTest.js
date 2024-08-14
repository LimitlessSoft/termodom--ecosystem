import { PROJECT_URL } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'

export default async (driver) => {
    return
    await driver.get(PROJECT_URL)

    const productsListLocator = By.xpath('/html/body/div/div/main/div[2]/div/div[4]/div/div[1]')
    await driver.wait(until.elementLocated(productsListLocator), 10 * 1000)
    const productsGroups = await driver.findElement(productsListLocator)
    
    const hasAnyChild = await productsGroups.findElements(By.xpath('./*')).then(elements => elements.length > 0)
    assert(hasAnyChild)
}