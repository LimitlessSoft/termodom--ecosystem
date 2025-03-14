module.exports = class UsersRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ username, password, nickname }) {
        const result = await this.#dbClient.query({
            text: `INSERT INTO "Users" ("Username", "Password", "Nickname", "Type", "IsActive", "CreatedAt", "CreatedBy", "StoreId") VALUES ($1, $2, $3, $4, $5, $6, $7, $8) RETURNING *;`,
            values: [username, password, nickname, 0, true, new Date(), 0, 112],
        })
        return result.rows[0]
    }

    async softDelete(username) {
        await this.#dbClient.query({
            text: `UPDATE "Users" SET "IsActive" = FALSE WHERE "Username" = $1;`,
            values: [username],
        })
    }

    async hardDelete(username) {
        await this.#dbClient.query({
            text: `DELETE FROM "Users" WHERE "Username" = $1;`,
            values: [username],
        })
    }
}
