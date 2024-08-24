import { By, until } from 'selenium-webdriver'
import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import assert from 'assert'

export default async (driver) => {
    await driver.get(PROJECT_URL)

    const profiKutakButtonLocator = By.xpath(
        `/html/body/div/div/header/div[2]/a[4]`
    )
    await driver.wait(
        until.elementLocated(profiKutakButtonLocator),
        WAIT_TIMEOUT
    )
    const profiKutakButton = await driver.findElement(profiKutakButtonLocator)
    await profiKutakButton.click()

    const usernameInputLocator = By.xpath(`//*[@id="username"]`)
    await driver.wait(until.elementLocated(usernameInputLocator), WAIT_TIMEOUT)
    const usernameInput = await driver.findElement(usernameInputLocator)
    await usernameInput.sendKeys('ALEKS13')

    const passwordInputLocator = By.xpath(`//*[@id="password"]`)
    await driver.wait(until.elementLocated(passwordInputLocator), WAIT_TIMEOUT)
    const passwordInput = await driver.findElement(passwordInputLocator)
    await passwordInput.sendKeys('Test123.')

    const loginButtonLocator = By.xpath(
        `/html/body/div/div/main/div[2]/div/button[1]`
    )
    await driver.wait(until.elementLocated(loginButtonLocator), WAIT_TIMEOUT)
    const loginButton = await driver.findElement(loginButtonLocator)
    await loginButton.click()

    const productLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div/div[3]/div/div[1]/div[1]/a'
    )
    await driver.wait(until.elementLocated(productLocator), WAIT_TIMEOUT)
    const product = await driver.findElement(productLocator)
    await product.click()

    await driver.sleep(4000)

    const dodajUKorpuButtonLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div/div[2]/div[2]/div/div[1]/button'
    )
    await driver.wait(
        until.elementLocated(dodajUKorpuButtonLocator),
        WAIT_TIMEOUT
    )
    const dodajUKorpuButton = await driver.findElement(dodajUKorpuButtonLocator)
    await dodajUKorpuButton.click()

    const nacinPlacanjaLocator = By.xpath(`//*[@id="nacini-placanja"]`)
    await driver.wait(until.elementLocated(nacinPlacanjaLocator), WAIT_TIMEOUT)
    const nacinPlacanja = await driver.findElement(nacinPlacanjaLocator)
    await nacinPlacanja.click()

    const nacinPlacanjaFirstOptionLocator = By.xpath(`//*[@id=":r5:"]/li[1]`)
    await driver.wait(
        until.elementLocated(nacinPlacanjaFirstOptionLocator),
        WAIT_TIMEOUT
    )

    const nacinPlacanjaFirstOption = await driver.findElement(
        nacinPlacanjaFirstOptionLocator
    )
    await nacinPlacanjaFirstOption.click()

    const zakljuciPorudzbinuButtonLocator = By.xpath(
        `//*[@id="__next"]/div/main/div[2]/div[4]/div/button`
    )
    await driver.wait(
        until.elementLocated(zakljuciPorudzbinuButtonLocator),
        WAIT_TIMEOUT
    )
    const zakljuciPorudzbinuButton = await driver.findElement(
        zakljuciPorudzbinuButtonLocator
    )

    await zakljuciPorudzbinuButton.click()

    const imeKupcaLabelLocator = By.xpath(
        '//*[@id="__next"]/div/main/div[2]/div[2]/div[1]/p[3]/span'
    )
    await driver.wait(until.elementLocated(imeKupcaLabelLocator), WAIT_TIMEOUT)
    const imeKupcaLabel = await driver.findElement(imeKupcaLabelLocator)
    const imeKupcaLabelText = await imeKupcaLabel.getText()
    assert.equal(imeKupcaLabelText, 'Ime kupca')
}
