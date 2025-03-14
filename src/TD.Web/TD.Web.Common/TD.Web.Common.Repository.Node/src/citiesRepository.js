module.exports = class CitiesRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ name }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "Cities" ("Name", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4) RETURNING *`,
            values: [name, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "Cities" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
