module.exports = class TransportOrdersRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({
        mobile,
        transportPriceWithoutVAT,
        weChargedTheCustomWithoutVAT,
        note,
        address,
        vrDok,
        brDok,
        storeId,
    }) {
        const result = await this.#dbClient.query({
            text: `INSERT INTO "NaloziZaPrevoz" ("Mobilni", "CenaPrevozaBezPdv", "MiNaplatiliKupcuBezPdv", "Note", "Address", "VrDok", "BrDok", "StoreId", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11) RETURNING *;`,
            values: [
                mobile,
                transportPriceWithoutVAT,
                weChargedTheCustomWithoutVAT,
                note,
                address,
                vrDok,
                brDok,
                storeId,
                true,
                new Date(),
                0,
            ],
        })
        return result.rows[0]
    }

    async softDelete(id) {
        await this.#dbClient.query({
            text: `UPDATE "NaloziZaPrevoz" SET "IsActive" = FALSE WHERE "Id" = $1;`,
            values: [id],
        })
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "NaloziZaPrevoz" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
