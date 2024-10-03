const { officeDb } = require('td-office-common-repository-node')
const asyncHandler = require('express-async-handler')
const throwError = require('../helpers/throwError')
const { enums, entities } = require('td-office-common-contracts-node')

require('../helpers/commonHelpers')
require('../helpers/novcaniceHelpers')

const { columns } = entities.specificationEntity
const { tableName } = entities.specificationEntity

const getCurrentSpecification = asyncHandler(async (req, res, next) => {
    const { specificationId } = req.params

    const specificationResult = await officeDb.query(
        `SELECT * FROM "${tableName}" WHERE "${columns.id}" = $1`,
        [specificationId]
    )

    const arr = [12, 15]

    console.log(arr.sum())

    const [currentSpecification] = specificationResult.rows

    if (!currentSpecification) {
        return throwError('Specification not found', 404)
    }

    res.status(200).json(currentSpecification)
})

const updateSpecification = asyncHandler(async (req, res, next) => {
    const { magacinSelected } = req.query
    const { id, racunar, poreska, specifikacijaNovca, komentar, racunTrazi } =
        req.body

    const novcanice = specifikacijaNovca.novcanice

    const arr = [{ val1: 1 }, { val1: 2 }]

    const v = [1, 2]

    const values = [
        komentar,
        novcanice.novcanicaFind(NOVCANICE[0]),
        novcanice.novcanicaFind(NOVCANICE[1]),
    ]

    const updateResult = await officeDb.query(
        `UPDATE "${tableName}"
             SET
               "${columns.comment}" = $1,
               "${columns.piecesOf5000}" = $2,
               "${columns.piecesOf2000}" = $3,
               "${columns.piecesOf1000}" = $4,
               "${columns.piecesOf500}" = $5,
               "${columns.piecesOf200}" = $6,
               "${columns.piecesOf100}" = $7,
               "${columns.piecesOf50}" = $8,
               "${columns.piecesOf20}" = $9,
               "${columns.piecesOf10}" = $10,
               "${columns.piecesOf5}" = $11,
               "${columns.piecesOf2}" = $12,
               "${columns.piecesOf1}" = $13,
               "${columns.cards}" = $14,
               "${columns.cardsComment}" = $15,
               "${columns.cheques}" = $16,
               "${columns.chequesComment}" = $17,
               "${columns.papers}" = $18,
               "${columns.papersComment}" = $19,
               "${columns.costs}" = $20,
               "${columns.costsComment}" = $21,
               "${columns.drivers}" = $22,
               "${columns.driversComment}" = $23,
               "${columns.sasa}" = $24,
               "${columns.sasaComment}" = $25
             WHERE "${columns.id}" = ${id}
               AND "${columns.isActive}" = true`,
        values
    )

    console.log('Successfully updated specification')
})

module.exports = {
    getCurrentSpecification,
    updateSpecification,
}
