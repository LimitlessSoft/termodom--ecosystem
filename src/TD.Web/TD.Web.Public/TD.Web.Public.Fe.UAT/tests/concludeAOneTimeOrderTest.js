import { webDbClientFactory } from '../configs/dbConfig.js'
import { ELEMENT_AWAITER_TIMEOUT, PROJECT_URL } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import imagesHelpers from '../helpers/imagesHelpers.js'
import productsHelpers from '../helpers/productsHelpers.js'
import ordersHelpers from '../helpers/ordersHelpers.js'
import orderItemsHelpers from '../helpers/orderItemsHelpers.js'

const webDbClient = await webDbClientFactory.create()

const state = {}

export default {
    beforeExecution: async () => {
        const {
            product,
            unit,
            productPriceGroup,
            imageFilename,
            productPrice,
        } = await productsHelpers.createProduct(webDbClient)

        state.product = product
        state.unit = unit
        state.productPriceGroup = productPriceGroup
        state.imageFilename = imageFilename
        state.productPrice = productPrice

        const { Id: orderId, OneTimeHash: orderOneTimeHash } =
            await ordersHelpers.createMockOrder(webDbClient)
        state.orderOneTimeHash = orderOneTimeHash

        const { Id: orderItemId } = await orderItemsHelpers.createMockOrderItem(
            webDbClient,
            {
                price: state.productPrice.Max,
                orderId,
                productId: state.product.Id,
            }
        )
        state.orderItemId = orderItemId
    },
    afterExecution: async () => {
        await webDbClient.productPricesRepository.hardDelete(state.productPrice.Id)
        await webDbClient.productsRepository.hardDelete(state.product.Id)
        await webDbClient.unitsRepository.hardDelete(state.unit.id)
        await webDbClient.productPriceGroupsRepository.hardDelete(state.productPriceGroup.Id)
        await webDbClient.orderItemsRepository.hardDelete(state.orderItemId)
        await webDbClient.ordersRepository.hardDeleteByHash(state.orderOneTimeHash)
        await imagesHelpers.removeImageFromMinio(state.imageFilename)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await driver.get(PROJECT_URL)

        await driver.manage().addCookie({
            name: 'cartId',
            value: state.orderOneTimeHash,
            path: '/',
        })

        await driver.sleep(500)

        await driver.get(`${PROJECT_URL}/korpa`)

        const addressInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="adresa-dostave"]')),
            ELEMENT_AWAITER_TIMEOUT
        )
        await addressInput.sendKeys('Selenium test address')

        const nameAndUsernameInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="ime-i-prezime"]')),
            ELEMENT_AWAITER_TIMEOUT
        )
        await nameAndUsernameInput.sendKeys('Selenium Test')

        const phoneInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="mobilni"]')),
            ELEMENT_AWAITER_TIMEOUT
        )
        await phoneInput.sendKeys('0693691472')

        const noteInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="napomena"]')),
            ELEMENT_AWAITER_TIMEOUT
        )
        await noteInput.sendKeys('Selenium test note')

        const paymentTypeSelectInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="nacini-placanja"]')),
            ELEMENT_AWAITER_TIMEOUT
        )
        await paymentTypeSelectInput.click()

        const paymentTypeSelectInputFirstOption = await driver.wait(
            until.elementLocated(By.xpath('/html/body/div[2]/div[3]/ul/li')),
            ELEMENT_AWAITER_TIMEOUT * 3
        )
        await paymentTypeSelectInputFirstOption.click()

        const concludeOrderButton = await driver.wait(
            until.elementLocated(
                By.xpath('//*[@id="__next"]/div/main/div[2]/div[6]/div/button')
            ),
            ELEMENT_AWAITER_TIMEOUT
        )
        await concludeOrderButton.click()

        const buyerNoteLabel = (
            await (
                await driver.wait(
                    until.elementLocated(
                        By.xpath(
                            '//*[@id="__next"]/div/main/div[2]/div[2]/div[1]/p[1]/span'
                        )
                    ),
                    ELEMENT_AWAITER_TIMEOUT
                )
            ).getText()
        ).toLowerCase()

        const expectedNoteLabel = 'kupac je ostavio napomenu:'

        assert.equal(
            buyerNoteLabel,
            expectedNoteLabel,
            `Expected text to include '${expectedNoteLabel}', but got: '${buyerNoteLabel}'`
        )
    },
}
