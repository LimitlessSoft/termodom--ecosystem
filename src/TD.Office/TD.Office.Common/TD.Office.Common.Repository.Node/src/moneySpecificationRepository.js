module.exports = class MoneySpecificationRepository {
    #dbClient

    constructor(dbClient) {
        this.#dbClient = dbClient
    }

    async create({
        magacinId,
        comment,
        eur1Pieces,
        eur1ExchangeRate,
        eur2Pieces,
        eur2ExchangeRate,
        banknote5000Pieces,
        banknote2000Pieces,
        banknote1000Pieces,
        banknote500Pieces,
        banknote200Pieces,
        banknote100Pieces,
        banknote50Pieces,
        banknote20Pieces,
        banknote10Pieces,
        banknote5Pieces,
        banknote2Pieces,
        banknote1Pieces,
        cards,
        cardsComment,
        cheques,
        chequesComment,
        papers,
        papersComment,
        costs,
        costsComment,
        drivers,
        driversComment,
        sasa,
        sasaComment,
    }) {
        await this.#dbClient.query({
            text: `INSERT INTO "SpecifikacijaNovca" ("MaganicId", "Datum", "Komentar", "Eur1Komada", "Eur1Kurs", "Eur2Komada", "Eur2Kurs", "Novcanica5000komada", "Novcanica2000komada", "Novcanica1000komada", "Novcanica500komada", "Novcanica200komada", "Novcanica100komada", "Novcanica50komada", "Novcanica20komada", "Novcanica10komada", "Novcanica5komada", "Novcanica2komada", "Novcanica1komada", "Kartice", "KarticeKomentar", "Cekovi", "CekoviKomentar", "Papiri", "PapiriKomentar", "Troskovi", "TroskoviKomentar", "Vozaci", "VozaciKomentar", "Sasa", "SasaKomentar", "IsActive", "CreatedAt", "CreatedBy") VALUES ($1, $2, $3, $4, $5, $6. $7, $8, $9, $10, $11, $12, $13, $14, $15, $16, $17, $18, $19, $20, $21, $22, $23, $24, $25, $26, $27, $28, $29, $30, $31, $32, $33, $34) RETURNING *;`,
            values: [
                magacinId,
                new Date(),
                comment,
                eur1Pieces,
                eur1ExchangeRate,
                eur2Pieces,
                eur2ExchangeRate,
                banknote5000Pieces,
                banknote2000Pieces,
                banknote1000Pieces,
                banknote500Pieces,
                banknote200Pieces,
                banknote100Pieces,
                banknote50Pieces,
                banknote20Pieces,
                banknote10Pieces,
                banknote5Pieces,
                banknote2Pieces,
                banknote1Pieces,
                cards,
                cardsComment,
                cheques,
                chequesComment,
                papers,
                papersComment,
                costs,
                costsComment,
                drivers,
                driversComment,
                sasa,
                sasaComment,
                true,
                new Date(),
                0,
            ],
        })
    }

    async softDelete(id) {
        await this.#dbClient.query({
            text: `UPDATE "SpecifikacijaNovca" SET "IsActive" = FALSE WHERE "Id" = $1;`,
            values: [id],
        })
    }

    async hardDelete(id) {
        await this.#dbClient.query({
            text: `DELETE FROM "SpecifikacijaNovca" WHERE "Id" = $1;`,
            values: [id],
        })
    }
}
