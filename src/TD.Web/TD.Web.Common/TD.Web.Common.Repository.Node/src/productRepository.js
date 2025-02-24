module.exports = class ProductRepository {
    constructor(dbClient) {
        this.dbClient = dbClient;
    }
    
    async create(name, src, image, catalogId, unitId, classification, vat, priceId, productPriceGroupId, description, shortDescription, priorityIndex, status, stockType) {
        await this.dbClient.query({
            text: `INSERT INTO "Products" (
                        "Name", "Src", "Image", "CatalogId", "UnitId",
                        "Classification", "Vat", "PriceId",
                        "ProductPriceGroupId", "Description",
                        "ShortDescription", "PriorityIndex", "Status",
                        "StockType", "IsActive", "CreatedBy", "CreatedAt")
                    VALUES (
                        $1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14, $15, $16, $17);`,
            values: [name, src, image, catalogId, unitId,classification, vat,
                priceId, productPriceGroupId, description, shortDescription,
                priorityIndex, status, stockType, true, 0, new Date()]
        });
    }
    
    async hardDelete(src) {
        await this.dbClient.query({
            text: 'DELETE FROM "Products" WHERE "Src" = $1;',
            values: [src]
        })
    }
}