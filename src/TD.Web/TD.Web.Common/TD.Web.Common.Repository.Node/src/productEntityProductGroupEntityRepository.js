module.exports = class ProductEntityProductGroupEntityRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ groupsId, productsId }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "ProductEntityProductGroupEntity" ("GroupsId", "ProductsId") VALUES ($1, $2) RETURNING *`,
            values: [groupsId, productsId],
        })
        return response.rows[0]
    }

    async hardDelete({ groupsId, productsId }) {
        const conditions = []
        const values = []

        if (groupsId !== undefined) {
            conditions.push(`"GroupsId" = $${values.length + 1}`)
            values.push(groupsId)
        }

        if (productsId !== undefined) {
            conditions.push(`"ProductsId" = $${values.length + 1}`)
            values.push(productsId)
        }

        if (conditions.length === 0) {
            throw new Error(
                'At least one parameter (groupsId or productsId) must be provided.'
            )
        }

        await this.#dbClient.query({
            text: `DELETE FROM "ProductEntityProductGroupEntity" WHERE ${conditions.join(
                ' OR '
            )};`,
            values,
        })
    }
}
