import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const storesHelpers = {
    async createMockStore(webDbClient) {
        return await webDbClient.storesRepository.create({
            name: faker.string.alpha(10),
        })
    },
    async hardDeleteMockStore(webDbClient, id) {
        await webDbClient.storesRepository.hardDelete(id)
    },
}

export default storesHelpers
