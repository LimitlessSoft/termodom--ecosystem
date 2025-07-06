import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const paymentTypesHelpers = {
    async createMockPaymentType(webDbClient) {
        return await webDbClient.paymentTypesRepository.create({
            name: faker.string.alpha(10),
        })
    },
    async createWireTransferPaymentType(webDbClient) {
        return await webDbClient.paymentTypesRepository.create({
            id: 6,
            name: 'Virmanom',
        })
    },
}

export default paymentTypesHelpers
