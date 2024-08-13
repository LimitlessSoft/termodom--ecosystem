import { PROJECT_URL } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'

export default async (driver) => {
    await driver.get(PROJECT_URL)

    const productLocator = By.xpath('/html/body/div/div/main/div[2]/div/div[4]/div/div[1]/div[1]/a/div/button')
    await driver.wait(until.elementLocated(productLocator), 10 * 1000)
    const product = await driver.findElement(productLocator)
    await product.click()

    const dodajUKorpuButtonLocator = By.xpath(`/html/body/div/div/main/div[2]/div/div[2]/div[2]/div/div[1]/button`)
    await driver.wait(until.elementLocated(dodajUKorpuButtonLocator), 10 * 1000)
    const dodajUKorpuButton = await driver.findElement(dodajUKorpuButtonLocator)
    await dodajUKorpuButton.click()
    
    const imeIPrezimeInputLocator = By.xpath(`//*[@id="ime-i-prezime"]`)
    await driver.wait(until.elementLocated(imeIPrezimeInputLocator), 10 * 1000)
    const imeIPrezimeInput = await driver.findElement(imeIPrezimeInputLocator)
    await imeIPrezimeInput.sendKeys("Selena Jovanovic")
    
    const mobilniTelefonInputLocator = By.xpath(`//*[@id="mobilni"]`)
    await driver.wait(until.elementLocated(mobilniTelefonInputLocator), 10 * 1000)
    const mobilniTelefonInput = await driver.findElement(mobilniTelefonInputLocator)
    await mobilniTelefonInput.sendKeys("0693691472")
    
    const napomenaInputLocator = By.xpath(`//*[@id="napomena"]`)
    await driver.wait(until.elementLocated(napomenaInputLocator), 10 * 1000)
    const napomenaInput = await driver.findElement(napomenaInputLocator)
    await napomenaInput.sendKeys("Selenium test napomena jednokratni")
    
    const nacinPlacanjaLocator = By.xpath(`//*[@id="nacini-placanja"]`)
    await driver.wait(until.elementLocated(nacinPlacanjaLocator), 10 * 1000)
    const nacinPlacanja = await driver.findElement(nacinPlacanjaLocator)
    await nacinPlacanja.click()
    
    const nacinPlacanjaFirstOptionLocator = By.xpath(`/html/body/div[2]/div[3]/ul/li[1]`)
    await driver.wait(until.elementLocated(nacinPlacanjaFirstOptionLocator), 10 * 1000)
    const nacinPlacanjaFirstOption = await driver.findElement(nacinPlacanjaFirstOptionLocator)
    await nacinPlacanjaFirstOption.click()
    
    const zakljuciPorudzbinuButtonLocator = By.xpath(`/html/body/div/div/main/div[2]/div[5]/div/button`)
    await driver.wait(until.elementLocated(zakljuciPorudzbinuButtonLocator), 10 * 1000)
    const zakljuciPorudzbinuButton = await driver.findElement(zakljuciPorudzbinuButtonLocator)
    await zakljuciPorudzbinuButton.click()
    
    const kupacJeOstavioNapomenuLabelLocator = By.xpath(`/html/body/div/div/main/div[2]/div[2]/p[1]/span`)
    await driver.wait(until.elementLocated(kupacJeOstavioNapomenuLabelLocator), 10 * 1000)
    const kupacJeOstavioNapomenuLabel = await driver.findElement(kupacJeOstavioNapomenuLabelLocator)
    const kupacJeOstavioNapomenuLabelText = await kupacJeOstavioNapomenuLabel.getText()
    const labelIncludes = kupacJeOstavioNapomenuLabelText.toLocaleLowerCase().includes("kupac je ostavio napomenu:")
    assert(labelIncludes)
}