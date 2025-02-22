import { PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import { GenerateAndConnectToWebDbClient } from '../db.js'

export default {
    beforeExecution: () => {},
    afterExecution: () => {},
    execution: async (driver) => {

        const response = await (await GenerateAndConnectToWebDbClient()).query('SELECT * FROM "Users" LIMIT 1')
        console.log(response.rows[0])
        return
        await driver.get(PROJECT_URL)

        const productsListLocator = By.xpath(
            `//*[@id="__next"]/div/main/div[2]/div/div[4]/div`
        )
        await driver.wait(
            until.elementLocated(productsListLocator),
            WAIT_TIMEOUT * 5
        )
        const productsGroups = await driver.findElement(productsListLocator)

        const products = await productsGroups
            .findElements(By.xpath(`./*`))
            .then((elements) => elements.length)

        for (let i = 0; i < products; i++) {
            const unavailablePriceButtonLocator = By.xpath(
                `//*[@id="__next"]/div/main/div[2]/div/div[4]/div[1]/div[${
                    i + 1
                }]/a/div/button/div/div[2]/div/div[2]/p`
            )

            await driver.wait(
                until.elementLocated(unavailablePriceButtonLocator),
                WAIT_TIMEOUT * 3
            )

            const unavailablePriceButton = await driver.findElement(
                unavailablePriceButtonLocator
            )

            const buttonText = await unavailablePriceButton.getText()

            console.log('Button text' + buttonText)

            const hasUnavailablePriceButton =
                buttonText.includes(`Klikni za cenu`)

            console.log(hasUnavailablePriceButton)

            if (hasUnavailablePriceButton) continue

            const productLocator = By.xpath(
                `//*[@id="__next"]/div/main/div[2]/div/div[4]/div[1]/div[${
                    i + 1
                }]`
            )
            await driver.wait(
                until.elementLocated(productLocator),
                WAIT_TIMEOUT * 5
            )

            const product = await driver.findElement(productLocator)

            console.log('Clicking product without unavailable price button...')
            await product.click()
            break
        }

        const dodajUKorpuButtonLocator = By.xpath(
            `//*[@id="__next"]/div/main/div[2]/div/div[2]/div[2]/div/div[1]/button`
        )
        await driver.wait(until.elementLocated(dodajUKorpuButtonLocator), 20000)
        const dodajUKorpuButton = await driver.findElement(
            dodajUKorpuButtonLocator
        )
        await dodajUKorpuButton.click()

        const addressInputLocator = By.xpath(`//*[@id="adresa-dostave"]`)
        await driver.wait(
            until.elementLocated(addressInputLocator),
            WAIT_TIMEOUT
        )
        const addressInput = await driver.findElement(addressInputLocator)
        await addressInput.sendKeys(`Selenium test adresa`)

        const imeIPrezimeInputLocator = By.xpath(`//*[@id="ime-i-prezime"]`)
        await driver.wait(
            until.elementLocated(imeIPrezimeInputLocator),
            WAIT_TIMEOUT
        )
        const imeIPrezimeInput = await driver.findElement(
            imeIPrezimeInputLocator
        )
        await imeIPrezimeInput.sendKeys('Selena Jovanovic')

        const mobilniTelefonInputLocator = By.xpath(`//*[@id="mobilni"]`)
        await driver.wait(
            until.elementLocated(mobilniTelefonInputLocator),
            WAIT_TIMEOUT
        )
        const mobilniTelefonInput = await driver.findElement(
            mobilniTelefonInputLocator
        )
        await mobilniTelefonInput.sendKeys('0693691472')

        const napomenaInputLocator = By.xpath(`//*[@id="napomena"]`)
        await driver.wait(
            until.elementLocated(napomenaInputLocator),
            WAIT_TIMEOUT
        )
        const napomenaInput = await driver.findElement(napomenaInputLocator)
        await napomenaInput.sendKeys('Selenium test napomena jednokratni')

        const nacinPlacanjaLocator = By.xpath(`//*[@id="nacini-placanja"]`)
        await driver.wait(
            until.elementLocated(nacinPlacanjaLocator),
            WAIT_TIMEOUT
        )
        const nacinPlacanja = await driver.findElement(nacinPlacanjaLocator)
        await nacinPlacanja.click()

        const nacinPlacanjaFirstOptionLocator = By.xpath(
            `//*[@id=":r9:"]/li[1]`
        )
        await driver.wait(
            until.elementLocated(nacinPlacanjaFirstOptionLocator),
            WAIT_TIMEOUT
        )
        const nacinPlacanjaFirstOption = await driver.findElement(
            nacinPlacanjaFirstOptionLocator
        )
        await nacinPlacanjaFirstOption.click()

        await driver.sleep(1000)

        const zakljuciPorudzbinuButtonLocator = By.xpath(
            `//*[@id="__next"]/div/main/div[2]/div[6]/div/button`
        )
        await driver.wait(
            until.elementLocated(zakljuciPorudzbinuButtonLocator),
            WAIT_TIMEOUT
        )
        const zakljuciPorudzbinuButton = await driver.findElement(
            zakljuciPorudzbinuButtonLocator
        )
        await zakljuciPorudzbinuButton.click()

        const kupacJeOstavioNapomenuLabelLocator = By.xpath(
            `//*[@id="__next"]/div/main/div[2]/div[2]/div[1]/p[1]/span`
        )
        await driver.wait(
            until.elementLocated(kupacJeOstavioNapomenuLabelLocator),
            WAIT_TIMEOUT
        )
        const kupacJeOstavioNapomenuLabel = await driver.findElement(
            kupacJeOstavioNapomenuLabelLocator
        )
        const kupacJeOstavioNapomenuLabelText =
            await kupacJeOstavioNapomenuLabel.getText()
        const labelIncludes = kupacJeOstavioNapomenuLabelText
            .toLocaleLowerCase()
            .includes('Kupac je ostavio napomenu:')
        assert(labelIncludes)
    },
}
