import { faker } from '@faker-js/faker/locale/sr_RS_latin'
import unitsHelpers from './unitsHelpers.js'
import imagesHelpers from './imagesHelpers.js'
import productPriceGroupsHelpers from './productPriceGroupsHelpers.js'
import productPricesHelpers from './productPricesHelpers.js'
import orderItemsHelpers from './orderItemsHelpers.js'
import ordersHelpers from './ordersHelpers.js'

const productsHelpers = {
    async createMockProductCore(
        webDbClient,
        { unitId, productPriceGroupId, imageFilename }
    ) {
        // This shouldn't be helper since it does nothing more than proxy to repository
        // Do not use in the future, but instead use createMockProduct or directly repository
        // When have time, replace usage of this function with repository call
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
    async createProduct(webDbClient) {
        const unit = await unitsHelpers.createMockUnit(webDbClient)
        const imageFilename = await imagesHelpers.uploadImageToMinio()
        const productPriceGroup =
            await productPriceGroupsHelpers.createMockProductPriceGroup(
                webDbClient
            )
        const product = await productsHelpers.createMockProductCore(
            webDbClient,
            {
                unitId: unit.Id,
                productPriceGroupId: productPriceGroup.Id,
                imageFilename,
            }
        )
        const productPrice = await productPricesHelpers.createMockProductPrice(
            webDbClient,
            {
                productId: product.Id,
            }
        )
        return {
            product,
            unit,
            productPriceGroup,
            imageFilename,
            productPrice,
        }
    },
    async hardDeleteMockProduct(webDbClient, id) {
        await webDbClient.productsRepository.hardDelete(id)
    },
}

export default productsHelpers
