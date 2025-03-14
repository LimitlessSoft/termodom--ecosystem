import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const productsHelpers = {
    async createMockProduct(
        webDbClient,
        { unitId, productPriceGroupId, imageFilename }
    ) {
        return await webDbClient.productsRepository.create({
            name: faker.string.alphanumeric(5),
            src: faker.string.alphanumeric(5).toLowerCase(),
            image: imageFilename,
            catalogId: faker.string.alphanumeric(5),
            unitId: +unitId,
            classification: faker.number.int(2),
            vat: 20,
            priceId: 0,
            productPriceGroupId: +productPriceGroupId,
            description: '',
            shortDescription: '',
            priorityIndex: 50,
            status: 0,
            stockType: 0,
        })
    },
    async hardDeleteMockProduct(webDbClient, id) {
        await webDbClient.productsRepository.hardDelete(id)
    },
}

export default productsHelpers
