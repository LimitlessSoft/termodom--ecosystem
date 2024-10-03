const express = require('express')
const app = express()
const port = 3000
const routes = require('./routes/specificationRoutes')
const bodyParser = require('body-parser')
const errorHandler = require('./middleware/errorHandler')

app.use(bodyParser.json())
app.use(
    bodyParser.urlencoded({
        extended: true,
    })
)

app.use('/', routes)

app.use(errorHandler)

app.listen(port, () => {
    console.log('Server started on port ' + port)
})
