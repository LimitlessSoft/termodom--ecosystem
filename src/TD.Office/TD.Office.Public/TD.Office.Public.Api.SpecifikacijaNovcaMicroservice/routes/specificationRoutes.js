const { Router } = require('express')
const {
    updateSpecification,
    getCurrentSpecification,
} = require('./controllers/specificationController')

const router = Router()

router.put('/specifications', updateSpecification)
router.get('/specifications/:specificationId', getCurrentSpecification)

module.exports = router
