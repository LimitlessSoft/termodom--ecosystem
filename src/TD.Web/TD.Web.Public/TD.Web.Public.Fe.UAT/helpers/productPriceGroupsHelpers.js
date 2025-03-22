import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const productPriceGroupsHelpers = {
    async createMockProductPriceGroup(webDbClient) {
        return await webDbClient.productPriceGroupsRepository.create({
            name: faker.string.alphanumeric(5),
        })
    }
}

export default productPriceGroupsHelpers
