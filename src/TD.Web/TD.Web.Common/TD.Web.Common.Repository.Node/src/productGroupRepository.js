module.exports = class ProductGroupRepository {
    constructor(dbClient) {
        this.dbClient = dbClient;
    }
    
    async create(name, parentId, src) {
        await this.dbClient.query({
            text: 'INSERT INTO "ProductGroups" ("Name", "ParentGroupId", "IsActive", "CreatedBy", "CreatedAt", "Src") VALUES ($1, $2, $3, $4, $5, $6);',
            values: [name, parentId, true, 0, new Date(), src]
        });
    }
    
    async hardDelete(name) {
        await this.dbClient.query({
            text: 'DELETE FROM "ProductGroups" WHERE "Name" = $1;',
            values: [name]
        })
    }
}