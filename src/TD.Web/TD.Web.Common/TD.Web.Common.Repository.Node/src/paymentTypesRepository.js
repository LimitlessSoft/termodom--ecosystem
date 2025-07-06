module.exports = class PaymentTypesRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ id, name }) {
        const fields = ['"Name"', '"IsActive"', '"CreatedAt"', '"CreatedBy"']
        const values = [name, true, new Date(), 0]

        if (id != null) {
            fields.unshift('"Id"')
            values.unshift(id)
        }

        const placeholders = fields
            .map((_, index) => `$${index + 1}`)
            .join(', ')

        const response = await this.#dbClient.query({
            text: `INSERT INTO "PaymentTypes" (${fields.join(
                ', '
            )}) VALUES (${placeholders}) RETURNING *`,
            values,
        })

        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "PaymentTypes" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
