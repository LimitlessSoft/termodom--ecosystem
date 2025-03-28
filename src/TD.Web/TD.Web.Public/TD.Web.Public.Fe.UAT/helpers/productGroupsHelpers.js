import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const productGroupsHelpers = {
    async createMockProductGroup(webDbClient, { parentId }) {
        return await webDbClient.productGroupsRepository.create({
            name: faker.string.alpha(10),
            parentId,
            src: faker.string.alpha({ length: 10, casing: 'lower' }),
        })
    },
}

export default productGroupsHelpers
