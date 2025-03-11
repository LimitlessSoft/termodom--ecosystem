import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const ordersHelpers = {
    async createMockOrder(webDbClient) {
        return await webDbClient.ordersRepository.create({
            oneTimeHash: faker.string.alphanumeric(32).toUpperCase(),
        })
    },
    async hardDeleteMockOrder(webDbClient, oneTimeHash) {
        await webDbClient.ordersRepository.hardDelete(oneTimeHash)
    },
}

export default ordersHelpers
