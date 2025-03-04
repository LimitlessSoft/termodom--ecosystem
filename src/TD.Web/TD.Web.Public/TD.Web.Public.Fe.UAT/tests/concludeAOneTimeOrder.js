import { generateAutomationWebDbClient } from '../configs/dbConfig.js'
import { appSecrets, PROJECT_URL, WAIT_TIMEOUT } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'
import imagesHelpers from '../helpers/imagesHelpers.js'

const webDbClient = await generateAutomationWebDbClient()
const state = {}

console.log(
    'Secrets' + appSecrets
    // appSecrets['web/public/api'],
    // appSecrets['office/public/api']
)

export default {
    beforeExecution: async () => {
        const { Id: unitId } = await webDbClient.unitsRepository.create(
            faker.string.alphanumeric(5)
        )

        state.unitId = unitId

        const imageFilename = await imagesHelpers.uploadImageToMinio()

        state.imageFilename = imageFilename

        const { Id: productPriceGroupId } =
            await webDbClient.productPriceGroupsRepository.create(
                faker.string.alphanumeric(5)
            )

        state.productPriceGroupId = productPriceGroupId

        const { Id: productId } = await webDbClient.productsRepository.create({
            name: faker.string.alphanumeric(5),
            src: faker.string.alphanumeric(5).toLowerCase(),
            image: imageFilename,
            catalogId: faker.string.alphanumeric(5),
            unitId: +unitId,
            classification: faker.number.int(2),
            vat: 20,
            priceId: 0,
            productPriceGroupId: +productPriceGroupId,
            description: '',
            shortDescription: '',
            priorityIndex: 50,
            status: 0,
            stockType: 0,
        })

        state.productId = productId

        const { Id: priceId, Max: maxPrice } =
            await webDbClient.productPricesRepository.create({
                min: faker.number.float({
                    min: 10,
                    max: 10000,
                    fractionDigits: 2,
                }),
                max: faker.number.float({
                    min: 10000,
                    max: 100000,
                    fractionDigits: 2,
                }),
                productId: +productId,
            })

        state.priceId = priceId

        const { Id: orderId, OneTimeHash: orderOneTimeHash } =
            await webDbClient.ordersRepository.create({
                oneTimeHash: faker.string.alphanumeric(32).toUpperCase(),
            })

        state.orderOneTimeHash = orderOneTimeHash

        const { Id: orderItemId } =
            await webDbClient.orderItemsRepository.create({
                price: maxPrice,
                orderId,
                productId,
            })

        state.orderItem = orderItemId
    },
    afterExecution: async () => {
        await webDbClient.productPricesRepository.hardDelete(
            state.productPriceId
        )

        await webDbClient.productsRepository.hardDelete(state.productId)

        await webDbClient.unitsRepository.hardDelete(state.unitId)

        await webDbClient.productPriceGroupsRepository.hardDelete(
            state.productPriceGroupId
        )

        await webDbClient.orderItemsRepository.hardDelete(state.orderItemId)

        await webDbClient.ordersRepository.hardDelete(state.orderOneTimeHash)

        await imagesHelpers.removeImageFromMinio(state.imageFilename)

        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)
        await driver
            .manage()
            .addCookie({ name: 'cartId', value: state.orderOneTimeHash })
        await driver.get(`${PROJECT_URL}/korpa`)
        await driver.sleep(1000)
        const addressInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="adresa-dostave"]')),
            WAIT_TIMEOUT
        )
        await addressInput.sendKeys('Selenium test address')
        const nameAndUsernameInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="ime-i-prezime"]')),
            WAIT_TIMEOUT
        )
        await nameAndUsernameInput.sendKeys('Selenium Test')
        const phoneInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="mobilni"]')),
            WAIT_TIMEOUT
        )
        await phoneInput.sendKeys('0693691472')
        const noteInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="napomena"]')),
            WAIT_TIMEOUT
        )
        await noteInput.sendKeys('Selenium test note')
        const paymentTypeSelectInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="nacini-placanja"]')),
            WAIT_TIMEOUT
        )
        await paymentTypeSelectInput.click()
        const paymentTypeSelectInputFirstOption = await driver.wait(
            until.elementLocated(By.xpath('/html/body/div[2]/div[3]/ul/li')),
            WAIT_TIMEOUT * 3
        )
        await paymentTypeSelectInputFirstOption.click()
        await driver.sleep(1000)
        const concludeOrderButton = await driver.wait(
            until.elementLocated(
                By.xpath('//*[@id="__next"]/div/main/div[2]/div[6]/div/button')
            ),
            WAIT_TIMEOUT
        )
        await concludeOrderButton.click()
        const buyerNoteLabelElement = await driver.wait(
            until.elementLocated(
                By.xpath(
                    '//*[@id="__next"]/div/main/div[2]/div[2]/div[1]/p[1]/span'
                )
            ),
            WAIT_TIMEOUT
        )
        const buyerNoteLabel = (
            await buyerNoteLabelElement.getText()
        ).toLowerCase()
        const expectedNoteLabel = 'kupac je ostavio napomenu:'

        await assert.equal(
            buyerNoteLabel,
            expectedNoteLabel,
            `Expected text to include '${expectedNoteLabel}', but got: '${buyerNoteLabel}'`
        )
    },
}
