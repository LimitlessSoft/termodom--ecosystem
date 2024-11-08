const { body, query, param } = require('express-validator')

const NOVCANICE_KEYS = [5000, 2000, 1000, 500, 200, 100, 50, 20, 10, 5, 2, 1]
const EUR_KEYS = ['komada', 'kurs']
const OSTALO_KEYS = ['key', 'vrednost', 'komentar']

const validateUpdateSpecificationRequest = [
    body('id').isInt({ gt: 0 }).withMessage('ID mora biti veći od 0.').bail(),
    body('magacinId')
        .isInt({ gt: 0 })
        .withMessage('Magacin ID mora biti veći od 0.')
        .bail(),
    body('datumUTC').isISO8601().withMessage('Datum mora biti u ISO formatu.'),

    body('specifikacijaNovca.eur1')
        .exists()
        .withMessage('EUR1 mora biti prisutan.')
        .bail()
        .custom((value) => {
            EUR_KEYS.forEach((key) => {
                if (!(key in value) || value[key] < 0) {
                    throw new Error(
                        `EUR1 mora imati ${key} sa pozitivnom vrednošću.`
                    )
                }
            })
            return true
        }),

    body('specifikacijaNovca.eur2')
        .exists()
        .withMessage('EUR2 mora biti prisutan.')
        .bail()
        .custom((value) => {
            EUR_KEYS.forEach((key) => {
                if (!(key in value) || value[key] < 0) {
                    throw new Error(
                        `EUR2 mora imati ${key} sa pozitivnom vrednošću.`
                    )
                }
            })
            return true
        }),

    body('specifikacijaNovca.novcanice')
        .exists()
        .withMessage('Novčanice moraju biti prisutne.')
        .bail()
        .isArray()
        .withMessage('Novčanice moraju biti niz.')
        .custom((value) => {
            if (value.length === 0) {
                throw new Error('Novčanice ne smeju biti prazan niz.')
            }
            NOVCANICE_KEYS.forEach((key) => {
                const found = value.find((item) => item.key === key)
                if (
                    !found ||
                    typeof found.value !== 'number' ||
                    found.value < 0
                ) {
                    throw new Error(
                        `Novčanica ${key} mora biti prisutna i mora imati pozitivan broj.`
                    )
                }
            })
            return true
        }),

    body('specifikacijaNovca.ostalo')
        .exists()
        .withMessage('Ostalo mora biti prisutno.')
        .bail()
        .isArray()
        .withMessage('Ostalo mora biti niz.')
        .custom((value) => {
            if (value.length === 0) {
                throw new Error('Ostalo ne sme biti prazan niz.')
            }
            value.forEach((item) => {
                OSTALO_KEYS.forEach((key) => {
                    if (!(key in item)) {
                        throw new Error(
                            `Svaki item u Ostalo mora imati ${key}.`
                        )
                    }
                    if (
                        key === 'vrednost' &&
                        (typeof item.vrednost !== 'number' || item.vrednost < 0)
                    ) {
                        throw new Error('Vrednost mora biti pozitivna.')
                    }
                    if (
                        key === 'komentar' &&
                        item.komentar &&
                        typeof item.komentar !== 'string'
                    ) {
                        throw new Error(
                            'Komentar mora biti string ako je prisutan.'
                        )
                    }
                })
            })
            return true
        }),

    body('komentar')
        .optional()
        .isString()
        .withMessage('Komentar mora biti string.'),
]

const validateGetSingleSpecificationById = [
    param('specificationId')
        .isInt({ gt: 0 })
        .withMessage('ID mora biti veći od 0.'),
]

const validateGetPrevOrNextSpecification = [
    query('currentSpecificationId')
        .isInt({ gt: 0 })
        .withMessage('ID mora biti veći od 0.'),
    query('magacinSelected')
        .isBoolean()
        .withMessage('Da li je magacin selektovan mora biti boolean'),
]

module.exports = {
    validateUpdateSpecificationRequest,
    validateGetSingleSpecificationById,
    validateGetPrevOrNextSpecification,
}
