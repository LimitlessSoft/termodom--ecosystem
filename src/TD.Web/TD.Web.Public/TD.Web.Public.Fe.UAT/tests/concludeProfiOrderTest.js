import {
    ELEMENT_AWAITER_TIMEOUT,
    PROJECT_URL,
    PUBLIC_API_CLIENT,
} from '../constants.js'
import { webDbClientFactory } from '../configs/dbConfig.js'
import imagesHelpers from '../helpers/imagesHelpers.js'
import productsHelpers from '../helpers/productsHelpers.js'
import { By, until } from 'selenium-webdriver'
import usersHelpers from '../helpers/usersHelpers.js'
import { vaultClient } from '../configs/vaultConfig.js'
import assert from 'assert'
import ordersHelpers from '../helpers/ordersHelpers.js'
import orderItemsHelpers from '../helpers/orderItemsHelpers.js'
import paymentTypesHelpers from '../helpers/paymentTypesHelpers.js'

const { TEST_USER_PLAIN_PASSWORD } = await vaultClient.getSecret(
    'web/public/api'
)

const state = {
    user: { password: TEST_USER_PLAIN_PASSWORD },
}
const expected = {}

const webDbClient = await webDbClientFactory.create()

export default {
    beforeExecution: async () => {
        const { TEST_USER_PLAIN_PASSWORD } = await vaultClient.getSecret(
            'web/public/api'
        )
        const { Username: username } =
            await usersHelpers.registerAndConfirmMockUser(webDbClient)

        state.user.username = username
        state.token = await PUBLIC_API_CLIENT.users.login({
            username,
            password: TEST_USER_PLAIN_PASSWORD,
        })

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

        const { Id: paymentTypeId, Name: paymentTypeName } =
            await paymentTypesHelpers.createWireTransferPaymentType(webDbClient)
        state.paymentTypeId = paymentTypeId
        expected.paymentTypeName = paymentTypeName
    },
    afterExecution: async () => {
        await webDbClient.productPricesRepository.hardDelete(
            state.productPrice.Id
        )
        await webDbClient.productsRepository.hardDelete(state.product.Id)
        await webDbClient.unitsRepository.hardDelete(state.unit.id)
        await webDbClient.productPriceGroupsRepository.hardDelete(
            state.productPriceGroup.Id
        )
        await webDbClient.orderItemsRepository.hardDelete(state.orderItemId)
        await webDbClient.ordersRepository.hardDeleteByHash(
            state.orderOneTimeHash
        )
        await webDbClient.paymentTypesRepository.hardDelete(state.paymentTypeId)
        await imagesHelpers.removeImageFromMinio(state.imageFilename)
        await webDbClient.disconnect()
    },
    execution: async (driver) => {
        await usersHelpers.uatLogin(driver, state.token)

        await driver.manage().addCookie({
            name: 'cartId',
            value: state.orderOneTimeHash,
            path: '/',
        })

        await driver.sleep(500)

        await driver.get(`${PROJECT_URL}/zavrsi-porudzbinu`)

        const pickupPlaceInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="pickup-place"]')),
            ELEMENT_AWAITER_TIMEOUT
        )

        await pickupPlaceInput.click()

        const deliveryPickupPlaceInputOption = await driver.wait(
            until.elementLocated(
                By.xpath(`/html/body/div[2]/div[3]/ul/li[@data-value="-5"]`)
            ),
            ELEMENT_AWAITER_TIMEOUT
        )

        await deliveryPickupPlaceInputOption.click()

        await driver.wait(
            until.stalenessOf(deliveryPickupPlaceInputOption),
            ELEMENT_AWAITER_TIMEOUT
        )

        const paymentTypeInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="payment-type"]')),
            ELEMENT_AWAITER_TIMEOUT
        )

        await paymentTypeInput.click()

        const wireTransferPaymentInputOption = await driver.wait(
            until.elementLocated(
                By.xpath(
                    `/html/body/div[2]/div[3]/ul/li[text()="${expected.paymentTypeName}"]`
                )
            ),
            ELEMENT_AWAITER_TIMEOUT
        )

        await wireTransferPaymentInputOption.click()

        const addressInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="delivery-address"]')),
            ELEMENT_AWAITER_TIMEOUT
        )

        await addressInput.sendKeys('Selenium test address')

        const companyInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="company"]')),
            ELEMENT_AWAITER_TIMEOUT
        )
        await companyInput.sendKeys('123456789')

        const noteInput = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="note"]')),
            ELEMENT_AWAITER_TIMEOUT
        )
        await noteInput.sendKeys('Selenium test note')

        const concludeOrderButton = await driver.wait(
            until.elementLocated(By.xpath('//*[@id="conclude-order__button"]')),
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
