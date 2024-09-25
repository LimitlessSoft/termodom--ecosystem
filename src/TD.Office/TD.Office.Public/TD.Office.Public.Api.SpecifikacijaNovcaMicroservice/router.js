const { Router } = require('express')
const { updateSpecification, getSpecification } = require('./controller')

const router = Router()

router.put('/specifications/:specificationId/update', updateSpecification)
router.get('/specifications/:specificationId', getSpecification)

module.exports = router
