module.exports = class ProductPricesRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ min, max, productId }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "ProductPrices" ("Min", "Max", "ProductId", "IsActive", "CreatedAt", "CreatedBy", "UpdatedBy", "UpdatedAt")
                    VALUES ($1, $2, $3, $4, $5, $6, $7, $8) RETURNING *;`,
            values: [min, max, productId, true, new Date(), 0, 0, new Date()],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: 'DELETE FROM "ProductPrices" WHERE "Id" = $1;',
            values: [id],
        })
    }
}
