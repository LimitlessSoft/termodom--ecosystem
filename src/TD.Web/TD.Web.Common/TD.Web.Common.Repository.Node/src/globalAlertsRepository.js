module.exports = class GlobalAlertsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ application, text }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "GlobalAlerts" ("Application", "Text", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5) RETURNING *`,
            values: [application, text, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "GlobalAlerts" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
