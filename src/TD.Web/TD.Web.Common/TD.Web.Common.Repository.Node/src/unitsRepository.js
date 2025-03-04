module.exports = class UnitsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create(name) {
        const results = await this.#dbClient.query({
            text: 'INSERT INTO "Units" ("Name", "IsActive", "CreatedBy", "CreatedAt") VALUES ($1, $2, $3, $4) RETURNING *;',
            values: [name, true, 0, new Date()],
        })
        return results.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: 'DELETE FROM "Units" WHERE "Id" = $1;',
            values: [id],
        })
    }
}
