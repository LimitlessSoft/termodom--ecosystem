import { faker } from '@faker-js/faker/locale/sr_RS_latin'
import { PERCENT_OF_DIFFERENCE_TO_PLAY_WITH } from '../constants.js'
import numbersHelpers from './numbersHelpers.js'

const productPricesHelpers = {
    async createMockProductPrice(webDbClient, { productId }) {
        return await webDbClient.productPricesRepository.create({
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
    },
    async calculateProfiProductPrice({ minPrice, maxPrice, vat, level }) {
        const maxDiscount =
            (maxPrice - minPrice) * PERCENT_OF_DIFFERENCE_TO_PLAY_WITH
        const levelValue = maxDiscount / 3
        const price = numbersHelpers.formatNumber(
            (maxPrice - levelValue * level) * (1 + (vat || 0) / 100)
        )
        return price
    },
}

export default productPricesHelpers
