module.exports = class UserPermissionsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ permission, userId }) {
        await this.#dbClient.query({
            text: `INSERT INTO "UserPermissions" ("Permission", "UserId", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5) RETURNING *;`,
            values: [permission, userId, true, new Date(), 0],
        })
    }

    async softDelete(id) {
        await this.#dbClient.query({
            text: `UPDATE "UserPermissions" SET "IsActive" = FALSE WHERE "Id" = $1;`,
            values: [id],
        })
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "UserPermissions" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
