module.exports = class OrderOneTimeInformationRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ name, mobile, orderId }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "OrderOneTimeInformation" ("Name", "Mobile", "OrderId" ,"IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5, $6) RETURNING *`,
            values: [name, mobile, orderId, true, new Date(), 0],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "OrderOneTimeInformation" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
