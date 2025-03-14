module.exports = class CalculatorItemsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({
        productId,
        quantity,
        calculatorType,
        isPrimary,
        order,
        isHobi,
        isProfi,
        isStandard,
    }) {
        const response = await this.#dbClient.query({
            text: `INSERT INTO "CalculatorItems" ("ProductId", "Quantity", "CalculatorType", "IsPrimary", "Order", "IsHobi", "IsProfi", "isStandard", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11) RETURNING *`,
            values: [
                productId,
                quantity,
                calculatorType,
                isPrimary,
                order,
                isHobi,
                isProfi,
                isStandard,
                true,
                new Date(),
                0,
            ],
        })
        return response.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "CalculatorItems" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
