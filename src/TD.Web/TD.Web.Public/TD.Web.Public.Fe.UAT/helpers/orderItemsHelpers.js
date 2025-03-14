const orderItemsHelpers = {
    async createMockOrderItem(webDbClient, { price, orderId, productId }) {
        return await webDbClient.orderItemsRepository.create({
            price,
            orderId,
            productId,
        })
    },
    async hardDeleteMockOrderItem(webDbClient, id) {
        await webDbClient.orderItemsRepository.hardDelete(id)
    },
}

export default orderItemsHelpers
