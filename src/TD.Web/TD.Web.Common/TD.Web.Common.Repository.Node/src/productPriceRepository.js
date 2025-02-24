module.exports = class ProductPriceRepository {
    constructor(dbClient) {
        this.dbClient = dbClient;
    }
    
    async create(min, max, productId) {
        await this.dbClient.query({
            text: `INSERT INTO "ProductPrices" ("Min", "Max", "ProductId", "IsActive", "CreatedAt", "CreatedBy", "UpdatedBy", "UpdatedAt")
                    VALUES ($1, $2, $3, $4, $5, $6, $7, $8);`,
            values: [min, max, productId, true, new Date(), 0, 0, new Date()]
        });
    }
}