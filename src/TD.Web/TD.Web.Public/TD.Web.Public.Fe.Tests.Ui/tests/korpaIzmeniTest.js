import { By, until } from 'selenium-webdriver'
import { PROJECT_URL, WAIT_TIMEOUT } from '../constants'
import assert from 'assert'

export default async (driver) => {
    await driver.get(PROJECT_URL)

    const editCartItemQuantityButtonLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div[2]/div/table/tbody/tr/td[2]/button'
    )
    await driver.wait(
        until.elementLocated(editCartItemQuantityButtonLocator),
        WAIT_TIMEOUT
    )
    const editCartItemQuantityButton = await driver.findElement(
        editCartItemQuantityButtonLocator
    )
    await editCartItemQuantityButton.click()

    const updateCartItemQuantityButtonLocator = By.xpath(
        '/html/body/div[3]/div[3]/div/div[2]/button[1]'
    )
    await driver.wait(
        until.elementLocated(updateCartItemQuantityButtonLocator),
        WAIT_TIMEOUT
    )
    const updateCartItemQuantityButton = await driver.findElement(
        updateCartItemQuantityButtonLocator
    )
    await updateCartItemQuantityButton.click()

    const toastMessageLocator = By.xpath(
        '/html/body/div[1]/div/main/div[1]/div/div/div[1]/div[2]'
    )
    await driver.wait(until.elementLocated(toastMessageLocator), WAIT_TIMEOUT)
    await driver.sleep(1000)
    const toastMessage = await driver.findElement(toastMessageLocator)
    const message = await toastMessage.getText()
    await assert.equal(message, 'Količina je izmenjena na 1')
}
