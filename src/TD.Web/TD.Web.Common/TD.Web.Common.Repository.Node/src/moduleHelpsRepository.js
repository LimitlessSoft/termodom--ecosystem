module.exports = class ModuleHelpsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ text, moduleType }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "ModuleHelps" ("Text", "ModuleType", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5) RETURNING *`,
            values: [text, moduleType, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "ModuleHelps" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
