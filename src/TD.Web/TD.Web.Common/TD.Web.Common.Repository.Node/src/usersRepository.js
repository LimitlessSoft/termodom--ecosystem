module.exports = class UsersRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async setProcessingDate(username, processingDate) {
        await this.#dbClient.query({
            text: 'UPDATE "Users" SET "ProcessingDate" = $1 WHERE "Username" = $2;',
            values: [processingDate, username],
        })
    }

    async softDelete(username) {
        await this.#dbClient.query({
            text: 'UPDATE "Users" SET "IsActive" = FALSE WHERE "Username" = $1;',
            values: [username],
        })
    }

    async hardDelete(username) {
        await this.#dbClient.query({
            text: 'DELETE FROM "Users" WHERE "Username" = $1;',
            values: [username],
        })
    }
}
