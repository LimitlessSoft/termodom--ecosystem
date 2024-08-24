import { By, until } from 'selenium-webdriver'
import { PROJECT_URL, WAIT_TIMEOUT } from '../constants'
import { generateRandomCharacters } from '../helpers/generateRandomCharacters'
import assert from 'assert'

export default async (driver) => {
    await driver.get(PROJECT_URL)

    const searchInputLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div/div[3]/div[1]/div/div/div/input'
    )
    await driver.wait(until.elementLocated(searchInputLocator), WAIT_TIMEOUT)
    const searchInput = await driver.findElement(searchInputLocator)
    await searchInput.sendKeys(generateRandomCharacters('string'))

    const resetSearchButtonLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div/div[3]/div[1]/div/h6/button'
    )
    await driver.wait(
        until.elementLocated(resetSearchButtonLocator),
        WAIT_TIMEOUT
    )
    const resetSearchButton = await driver.findElement(resetSearchButtonLocator)

    assert(resetSearchButton)
}
