module.exports = class ProductsRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({
        name,
        src,
        image,
        catalogId,
        unitId,
        classification,
        vat,
        priceId,
        productPriceGroupId,
        description,
        shortDescription,
        priorityIndex,
        status,
        stockType,
    }) {
        const result = await this.#dbClient.query({
            text: `INSERT INTO "Products" (
                        "Name", "Src", "Image", "CatalogId", "UnitId",
                        "Classification", "VAT", "PriceId",
                        "ProductPriceGroupId", "Description",
                        "ShortDescription", "PriorityIndex", "Status",
                        "StockType", "IsActive", "CreatedBy", "CreatedAt")
                    VALUES (
                        $1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14, $15, $16, $17) RETURNING *;`,
            values: [
                name,
                src,
                image,
                catalogId,
                unitId,
                classification,
                vat,
                priceId,
                productPriceGroupId,
                description,
                shortDescription,
                priorityIndex,
                status,
                stockType,
                true,
                0,
                new Date(),
            ],
        })
        return result.rows[0]
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: 'DELETE FROM "Products" WHERE "Id" = $1;',
            values: [id],
        })
    }
}
