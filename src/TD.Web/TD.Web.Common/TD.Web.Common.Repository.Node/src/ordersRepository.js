module.exports = class OrdersRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ oneTimeHash, createdBy }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "Orders" ("OneTimeHash", "StoreId", "PaymentTypeId", "Status", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5, $6, $7) RETURNING *;`,
            values: [oneTimeHash, -5, 1, 0, true, new Date(), createdBy ?? 0],
        })
        return response.rows[0]
    }

    async hardDeleteByHash(oneTimeHash) {
        await this.#dbClient.query({
            text: `DELETE FROM "Orders" WHERE "OneTimeHash" = $1;`,
            values: [oneTimeHash],
        })
    }
}
