module.exports = class ProductGroupEntityUserEntityRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({ managingProductGroupsId, managingUsersId }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "ProductGroupEntityUser" ("ManaginProductGroupsId", "ManagingUsersId") VALUES ($1, $2) RETURNING *`,
            values: [managingProductGroupsId, managingUsersId],
        })
        return response.rows[0]
    }

    async hardDelete({ managingProductGroupsId, managingUsersId }) {
        const conditions = []
        const values = []

        if (managingProductGroupsId) {
            conditions.push(`"ManaginProductGroupsId" = $${values.length + 1}`)
            values.push(managingProductGroupsId)
        }

        if (managingUsersId) {
            conditions.push(`"ManagingUsersId" = $${values.length + 1}`)
            values.push(managingUsersId)
        }

        if (conditions.length === 0) {
            throw new Error(
                'At least one parameter (groupsId or productsId) must be provided.'
            )
        }

        await this.#dbClient.query({
            text: `DELETE FROM "ProductGroupEntityUser" WHERE ${conditions.join(
                ' OR '
            )};`,
            values,
        })
    }
}
