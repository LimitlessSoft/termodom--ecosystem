module.exports = class StoresRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ name }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "Stores" ("Name", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4) RETURNING *`,
            values: [name, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "Stores" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
