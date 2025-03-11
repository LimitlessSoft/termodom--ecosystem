module.exports = class ProductGroupsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ name, parentId, src }) {
        const result = await this.#dbClient.query({
            text: 'INSERT INTO "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "Src") VALUES ($1, $2, $3, $4, $5, $6) RETURNING *;',
            values: [name, parentId, true, 0, new Date(), src],
        })
        return result.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: 'DELETE FROM "ProductGroups" WHERE "Id" = $1;',
            values: [id],
        })
    }
}
