module.exports = class OrderItemsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ price, orderId, productId }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "OrderItems" ("Price", "Quantity", "PriceWithoutDiscount", "VAT", "OrderId", "ProductId", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9) RETURNING *;`,
            values: [price, 1, price, 20, orderId, productId, 1, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "OrderItems" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
