module.exports = class KomercijalnoWebProductLinksRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ robaId, webId }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "KomercijalnoWebProductLinks" ("RobaId", "WebId", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5) RETURNING *`,
            values: [robaId, webId, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "KomercijalnoWebProductLinks" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
