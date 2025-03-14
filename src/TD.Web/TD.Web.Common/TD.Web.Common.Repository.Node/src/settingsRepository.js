module.exports = class SettingsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ key, value }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "Settings" ("Key", "Value", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5) RETURNING *`,
            values: [key, value, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "Settings" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
