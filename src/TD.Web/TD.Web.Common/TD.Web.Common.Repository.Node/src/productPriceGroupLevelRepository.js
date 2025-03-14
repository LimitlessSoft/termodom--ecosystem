module.exports = class ProductPriceGroupLevelRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ userId, level, productPriceGroupId }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "ProductPriceGroupLevel" ("UserId", "Level", "ProductPriceGroupId", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5, $6) RETURNING *`,
            values: [userId, level, productPriceGroupId, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "ProductPriceGroupLevel" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
