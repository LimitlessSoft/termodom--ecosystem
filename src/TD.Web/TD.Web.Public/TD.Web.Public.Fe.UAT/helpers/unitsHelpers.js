import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const unitsHelpers = {
    async createMockUnit(webDbClient) {
        return await webDbClient.unitsRepository.create(
            faker.string.alphanumeric(5)
        )
    },
    async hardDeleteMockUnit(webDbClient, id) {
        await webDbClient.unitsRepository.hardDelete(id)
    },
}

export default unitsHelpers
