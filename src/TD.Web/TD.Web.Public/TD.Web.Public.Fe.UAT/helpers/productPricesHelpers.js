import { faker } from '@faker-js/faker/locale/sr_RS_latin'

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
    }
}

export default productPricesHelpers
