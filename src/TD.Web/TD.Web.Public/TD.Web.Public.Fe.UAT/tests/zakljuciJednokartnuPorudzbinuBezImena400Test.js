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
        // await driver.wait(until.elementLocated(productLocator), WAIT_TIMEOUT)
        // const product = await driver.findElement(productLocator)
        // await product.click()
        //
        // const dodajUKorpuButtonLocator = By.xpath(
        //     `/html/body/div/div/main/div[2]/div/div[2]/div[2]/div/div[1]/button`
        // )
        // await driver.wait(
        //     until.elementLocated(dodajUKorpuButtonLocator),
        //     WAIT_TIMEOUT
        // )
        // const dodajUKorpuButton = await driver.findElement(
        //     dodajUKorpuButtonLocator
        // )
        // await dodajUKorpuButton.click()
        //
        // const mobilniTelefonInputLocator = By.xpath(`//*[@id="mobilni"]`)
        // await driver.wait(
        //     until.elementLocated(mobilniTelefonInputLocator),
        //     WAIT_TIMEOUT
        // )
        // const mobilniTelefonInput = await driver.findElement(
        //     mobilniTelefonInputLocator
        // )
        // await mobilniTelefonInput.sendKeys('0693691472')
        //
        // const napomenaInputLocator = By.xpath(`//*[@id="napomena"]`)
        // await driver.wait(
        //     until.elementLocated(napomenaInputLocator),
        //     WAIT_TIMEOUT
        // )
        // const napomenaInput = await driver.findElement(napomenaInputLocator)
        // await napomenaInput.sendKeys('Selenium test napomena jednokratni')
        //
        // const nacinPlacanjaLocator = By.xpath(`//*[@id="nacini-placanja"]`)
        // await driver.wait(
        //     until.elementLocated(nacinPlacanjaLocator),
        //     WAIT_TIMEOUT
        // )
        // const nacinPlacanja = await driver.findElement(nacinPlacanjaLocator)
        // await nacinPlacanja.click()
        //
        // const nacinPlacanjaFirstOptionLocator = By.xpath(
        //     `/html/body/div[2]/div[3]/ul/li[1]`
        // )
        // await driver.wait(
        //     until.elementLocated(nacinPlacanjaFirstOptionLocator),
        //     WAIT_TIMEOUT
        // )
        // const nacinPlacanjaFirstOption = await driver.findElement(
        //     nacinPlacanjaFirstOptionLocator
        // )
        // await nacinPlacanjaFirstOption.click()
        //
        // await driver.sleep(1000)
        //
        // const zakljuciPorudzbinuButtonLocator = By.xpath(
        //     `/html/body/div/div/main/div[2]/div[5]/div/button`
        // )
        // await driver.wait(
        //     until.elementLocated(zakljuciPorudzbinuButtonLocator),
        //     WAIT_TIMEOUT
        // )
        // const zakljuciPorudzbinuButton = await driver.findElement(
        //     zakljuciPorudzbinuButtonLocator
        // )
        // await zakljuciPorudzbinuButton.click()
        //
        // const toastMessageLocator = By.xpath(
        //     `/html/body/div/div/main/div[1]/div/div[2]/div[1]/div[2]`
        // )
        // await driver.wait(
        //     until.elementLocated(toastMessageLocator),
        //     WAIT_TIMEOUT
        // )
        // await driver.sleep(1000)
        // const toastMessage = await driver.findElement(toastMessageLocator)
        // const message = await toastMessage.getText()
        // await assert.equal(message, "'Name' je obavezno polje.")
    },
}
