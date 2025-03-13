import { webDbClientFactory } from '../configs/dbConfig.js'
import { ELEMENT_AWAITER_TIMEOUT, PROJECT_URL } from '../constants.js'
import { By, until } from 'selenium-webdriver'
import assert from 'assert'
import imagesHelpers from '../helpers/imagesHelpers.js'
import productsHelpers from '../helpers/productsHelpers.js'
import unitsHelpers from '../helpers/unitsHelpers.js'
import productPriceGroupsHelpers from '../helpers/productPriceGroupsHelpers.js'
import productPricesHelpers from '../helpers/productPricesHelpers.js'
import ordersHelpers from '../helpers/ordersHelpers.js'
import orderItemsHelpers from '../helpers/orderItemsHelpers.js'

const webDbClient = await webDbClientFactory.create()
const state = {}

export default {
    beforeExecution: async () => {
        const { Id: unitId } = await unitsHelpers.createMockUnit(webDbClient)

        state.unitId = unitId

        const imageFilename = await imagesHelpers.uploadImageToMinio()

        state.imageFilename = imageFilename

        const { Id: productPriceGroupId } =
            await productPriceGroupsHelpers.createMockProductPriceGroup(
                webDbClient
            )

        state.productPriceGroupId = productPriceGroupId

        const { Id: productId } = await productsHelpers.createMockProduct(
            webDbClient,
            { unitId, productPriceGroupId, imageFilename }
        )

        state.productId = productId

        const { Id: priceId, Max: maxPrice } =
            await productPricesHelpers.createMockProductPrice(webDbClient, {
                productId,
            })

        state.priceId = priceId

        const { Id: orderId, OneTimeHash: orderOneTimeHash } =
            await ordersHelpers.createMockOrder(webDbClient)

        state.orderOneTimeHash = orderOneTimeHash

        const { Id: orderItemId } = await orderItemsHelpers.createMockOrderItem(
            webDbClient,
            {
                price: maxPrice,
                orderId,
                productId,
            }
        )

        state.orderItem = orderItemId
    },
    afterExecution: async () => {
        if (state.productPriceId) {
            await productPricesHelpers.hardDeleteMockProductPrice(
                webDbClient,
                state.productPriceId
            )
        }

        if (state.productId) {
            await productsHelpers.hardDeleteMockProduct(
                webDbClient,
                state.productId
            )
        }

        if (state.unitId) {
            await unitsHelpers.hardDeleteMockUnit(webDbClient, state.unitId)
        }

        if (state.productPriceGroupId) {
            await productPriceGroupsHelpers.hardDeleteMockProductPriceGroup(
                webDbClient,
                state.productPriceGroupId
            )
        }

        if (state.orderItemId) {
            await orderItemsHelpers.hardDeleteMockOrderItem(state.orderItemId)
        }

        if (state.orderOneTimeHash) {
            await ordersHelpers.hardDeleteMockOrder(
                webDbClient,
                state.orderOneTimeHash
            )
        }

        if (state.imageFilename) {
            await imagesHelpers.removeImageFromMinio(state.imageFilename)
        }

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
