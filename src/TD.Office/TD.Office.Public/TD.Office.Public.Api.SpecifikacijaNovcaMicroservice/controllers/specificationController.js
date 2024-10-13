const { officeDb } = require('td-office-common-repository-node')
const asyncHandler = require('express-async-handler')
const throwError = require('../helpers/throwError')
const { enums, entities } = require('td-office-common-contracts-node')
const {
    NOVCANICE,
    SPECIFICATION_TYPES,
    SPECIFICATION_OPERATORS,
} = require('../constants')
const {
    NOT_FOUND_MESSAGE,
} = require('../validationCodes/specificationValidationCodes')

require('../helpers/commonHelpers')
require('../helpers/novcaniceHelpers')

const { columns } = entities.specificationEntity
const { tableName } = entities.specificationEntity

const getSpecification = asyncHandler(async (req, res, next) => {
    const { specificationId } = req.params

    if (!specificationId) return

    const query = `SELECT * FROM "${tableName}" WHERE "${columns.id}" = $1 AND "${columns.isActive}" = $2 LIMIT 1`

    const values = [specificationId, true]

    const specificationResult = await officeDb.query(query, values)

    const [currentSpecification] = specificationResult.rows
    if (!currentSpecification) return throwError(NOT_FOUND_MESSAGE, 404)

    return res.status(200).json(currentSpecification)
})

const getNextOrPrevSpecification = asyncHandler(async (req, res, next) => {
    const { currentSpecificationId, magacinSelected } = req.query

    if (!currentSpecificationId) return

    const getCurrentSpecificationQuery = `SELECT * FROM "${tableName}" WHERE "${columns.id}" = $1 AND "${columns.isActive}" = $2 LIMIT 1`
    const getCurrentSpecificationValues = [currentSpecificationId, true]

    const currentSpecificationQueryResult = await officeDb.query(
        getCurrentSpecificationQuery,
        getCurrentSpecificationValues
    )

    const [currentSpecification] = currentSpecificationQueryResult.rows

    if (!currentSpecification) return

    const operator = req.originalUrl
        .toLowerCase()
        .includes(SPECIFICATION_TYPES.NEXT)
        ? SPECIFICATION_OPERATORS.NEXT
        : SPECIFICATION_OPERATORS.PREV

    const getNextOrPrevSpecificationQuery = `
    SELECT *
    FROM "${tableName}"
    WHERE "${columns.id}" ${operator} $1 AND "${columns.isActive}" = $2
    ${magacinSelected ? `AND "${columns.storeId}" = $3` : ''}
    ORDER BY "${columns.id}" ${
        operator === SPECIFICATION_OPERATORS.NEXT ? 'ASC' : 'DESC'
    }
    LIMIT 1
`
    const getNextOrPrevSpecificationValues = magacinSelected
        ? [currentSpecificationId, true, magacinSelected]
        : [currentSpecificationId, true]

    const getNextOrPrevSpecificationResult = await officeDb.query(
        getNextOrPrevSpecificationQuery,
        getNextOrPrevSpecificationValues
    )
    const [nextOrPrevSpecification] = getNextOrPrevSpecificationResult.rows

    if (!nextOrPrevSpecification) {
        return throwError('Specification not found', 404)
    }

    return res.status(200).json(nextOrPrevSpecification)
})

const updateSpecification = asyncHandler(async (req, res, next) => {
    const { id, specifikacijaNovca, komentar } = req.body

    if (!id || !specifikacijaNovca) {
        return throwError('All fields are required.', 400)
    }

    const getSpecificationQuery = `SELECT * FROM "${tableName}" WHERE "${columns.id}" = $1 AND "${columns.isActive}" = $2 LIMIT 1`
    const getSpecificationValues = [id, true]

    const specificationResult = await officeDb.query(
        getSpecificationQuery,
        getSpecificationValues
    )

    const [specification] = specificationResult.rows

    if (!specification) return

    const novcanice = specifikacijaNovca.novcanice
    const ostaloItems = specifikacijaNovca.ostalo

    if (!Array.isArray(novcanice) || novcanice.length === 0)
        return throwError('Novčanice must be a non-empty array.', 400)

    if (!Array.isArray(ostaloItems) || ostaloItems.length === 0)
        return throwError('Ostalo items must be a non-empty array.', 400)

    const values = [
        komentar || '',
        ...Object.keys(NOVCANICE)
            .toSorted((a, b) => b - a)
            .map(
                (key) =>
                    novcanice.find((item) => item.key === NOVCANICE[key])
                        ?.value || 0
            ),
        ...ostaloItems.map((item) => item.vrednost),
        ...ostaloItems.map((item) => item.komentar),
        specifikacijaNovca.eur1.komada,
        specifikacijaNovca.eur1.kurs,
        specifikacijaNovca.eur2.komada,
        specifikacijaNovca.eur2.kurs,
        id,
        true,
    ]

    const query = `UPDATE "${tableName}"
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
           "${columns.cheques}" = $15,
           "${columns.papers}" = $16,
           "${columns.costs}" = $17,
           "${columns.drivers}" = $18,
           "${columns.sasa}" = $19,
           "${columns.cardsComment}" = $20,
           "${columns.chequesComment}" = $21,
           "${columns.papersComment}" = $22,
           "${columns.costsComment}" = $23,
           "${columns.driversComment}" = $24,
           "${columns.sasaComment}" = $25,
           "${columns.eur1Pieces}" = $26,
           "${columns.eur1ExchangeRate}" = $27,
           "${columns.eur2Pieces}" = $28,
           "${columns.eur2ExchangeRate}" = $29
         WHERE "${columns.id}" = $30
           AND "${columns.isActive}" = $31
         RETURNING *`

    await officeDb.query(query, values)

    res.status(200).json({ message: 'Uspesno updateovano' })
})

module.exports = {
    getSpecification,
    getNextOrPrevSpecification,
    updateSpecification,
}
