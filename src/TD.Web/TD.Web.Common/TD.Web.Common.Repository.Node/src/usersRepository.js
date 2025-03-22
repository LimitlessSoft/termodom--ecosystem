module.exports = class UsersRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({
        cityId,
        mail,
        mobile,
        type,
        address,
        username,
        password,
        nickname,
        favoriteStoreId,
        dateOfBirth,
    }) {
        await this.#dbClient.query({
            text: `INSERT INTO "Users" ("CityId", "Mail", "Mobile", "Type", "Address", "Username", "Password", "Nickname", "FavoriteStoreId", "DateOfBirth", "IsActive", "CreatedAt", "CreatedBy", "ProfessionId", "DefaultPaymentTypeId") VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14, $15) RETURNING *;`,
            values: [
                cityId,
                mail,
                mobile,
                type,
                address,
                username,
                password,
                nickname,
                favoriteStoreId,
                dateOfBirth,
                true,
                new Date(),
                0,
                1,
                0,
            ],
        })
        return await this.setProcessingDate(username, new Date())
    }

    async setProcessingDate(username, processingDate) {
        const result = await this.#dbClient.query({
            text: 'UPDATE "Users" SET "ProcessingDate" = $1 WHERE "Username" = $2 RETURNING *;',
            values: [processingDate, username],
        })
        return result.rows[0]
    }

    async softDelete(username) {
        await this.#dbClient.query({
            text: 'UPDATE "Users" SET "IsActive" = FALSE WHERE "Username" = $1;',
            values: [username],
        })
    }

    async hardDeleteByUsername(username) {
        await this.#dbClient.query({
            text: 'DELETE FROM "Users" WHERE "Username" = $1;',
            values: [username],
        })
    }
}
