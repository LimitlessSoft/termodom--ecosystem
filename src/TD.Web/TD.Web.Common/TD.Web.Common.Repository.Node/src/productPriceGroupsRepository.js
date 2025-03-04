module.exports = class ProductPriceGroupsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create(name) {
        const result = await this.#dbClient.query({
            text: `INSERT INTO "ProductPriceGroups" ("Name", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4) RETURNING *;`,
            values: [name, true, new Date(), 0],
        })
        return result.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "ProductPriceGroups" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
