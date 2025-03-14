module.exports = class UserPermissionsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ permission, userId }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "UserPermissions" ("Permission", "UserId", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5) RETURNING *`,
            values: [permission, userId, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "UserPermissions" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
