module.exports = class NotesRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ name, content, userId }) {
        const result = await this.#dbClient.query({
            text: `INSERT INTO "Notes" ("Name", "Content", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5) RETURNING *;`,
            values: [name, content, true, new Date(), userId],
        })
        return result.rows[0]
    }

    async softDelete(id) {
        await this.#dbClient.query({
            text: `UPDATE "Notes" SET "IsActive" = FALSE WHERE "Id" = $1;`,
            values: [id],
        })
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "Notes" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
