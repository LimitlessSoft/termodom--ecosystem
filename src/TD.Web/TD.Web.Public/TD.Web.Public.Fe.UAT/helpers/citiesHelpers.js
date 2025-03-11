import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const citiesHelpers = {
    async createMockCity(webDbClient) {
        return await webDbClient.citiesRepository.create({
            name: faker.string.alpha(10),
        })
    },
    async hardDeleteMockCity(webDbClient, id) {
        await webDbClient.citiesRepository.hardDelete(id)
    },
}

export default citiesHelpers
