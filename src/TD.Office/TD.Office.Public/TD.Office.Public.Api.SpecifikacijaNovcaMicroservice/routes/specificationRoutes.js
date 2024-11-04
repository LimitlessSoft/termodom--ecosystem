const { Router } = require('express')
const {
    updateSpecification,
    getSpecification,
    getNextOrPrevSpecification,
} = require('../controllers/specificationController')
const {
    validateUpdateSpecificationRequest,
    validateGetPrevOrNextSpecification,
    validateGetSingleSpecificationById,
} = require('../validators/specificationRequestValidator')
const validationHandler = require('../middleware/validationHandler')

const router = Router()

router.put(
    '/',
    // validateUpdateSpecificationRequest,
    // validationHandler,
    // updateSpecification
    (req, res, next) => {
        console.log('asl')
    }
)
router.get(
    '/next',
    validateGetPrevOrNextSpecification,
    validationHandler,
    getNextOrPrevSpecification
)
router.get(
    '/prev',
    validateGetPrevOrNextSpecification,
    validationHandler,
    getNextOrPrevSpecification
)
router.get(
    '/:specificationId',
    validateGetSingleSpecificationById,
    validationHandler,
    getSpecification
)

module.exports = router
