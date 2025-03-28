import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const paymentTypesHelpers = {
    async createMockPaymentType(webDbClient) {
        return await webDbClient.paymentTypesRepository.create({
            name: faker.string.alpha(10),
        })
    },
}

export default paymentTypesHelpers
