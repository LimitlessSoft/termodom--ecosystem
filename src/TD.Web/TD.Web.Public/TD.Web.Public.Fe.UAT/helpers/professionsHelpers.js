import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const professionsHelpers = {
    async createMockProfession(webDbClient) {
        return await webDbClient.professionsRepository.create({
            name: faker.string.alpha(10),
        })
    },
}

export default professionsHelpers
