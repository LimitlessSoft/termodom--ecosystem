import { By, until } from 'selenium-webdriver'
import { PROJECT_URL, WAIT_TIMEOUT } from '../constants'
import assert from 'assert'

export default async (driver) => {
    await driver.get(PROJECT_URL)

    const groupButtonLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div/div[1]/div[1]/button'
    )
    await driver.wait(until.elementLocated(groupButtonLocator), WAIT_TIMEOUT)
    const groupButton = await driver.findElement(groupButtonLocator)
    await groupButton.click()

    const backButtonSvgLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div/div[1]/div/button/svg'
    )
    await driver.wait(until.elementLocated(backButtonSvgLocator), WAIT_TIMEOUT)
    const backButtonSvg = await driver.findElement(backButtonSvgLocator)

    assert(backButtonSvg)
}
