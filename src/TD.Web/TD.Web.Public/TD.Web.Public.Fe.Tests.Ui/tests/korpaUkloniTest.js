import { By, until } from 'selenium-webdriver'
import { PROJECT_URL, WAIT_TIMEOUT } from '../constants'
import assert from 'assert'

export default async (driver) => {
    await driver.get(PROJECT_URL)

    const removeFromCartButtonLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div[2]/div/table/tbody/tr/td[5]/button'
    )
    await driver.wait(
        until.elementLocated(removeFromCartButtonLocator),
        WAIT_TIMEOUT
    )
    const removeFromCartButton = await driver.findElement(
        removeFromCartButtonLocator
    )
    await removeFromCartButton.click()

    const toastMessageLocator = By.xpath(
        '/html/body/div[1]/div/main/div[1]/div/div/div[1]/div[2]'
    )
    await driver.wait(until.elementLocated(toastMessageLocator), WAIT_TIMEOUT)
    await driver.sleep(1000)
    const toastMessage = await driver.findElement(toastMessageLocator)
    const message = await toastMessage.getText()
    await assert.equal(message, 'Proizvod je uklonjen iz korpe')
}
