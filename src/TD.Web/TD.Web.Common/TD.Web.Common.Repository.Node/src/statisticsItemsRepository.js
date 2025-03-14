module.exports = class StatisticsItemsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ type, value }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "StatisticsItems" ("Type", "Value", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5) RETURNING *`,
            values: [type, value, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "StatisticsItems" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
