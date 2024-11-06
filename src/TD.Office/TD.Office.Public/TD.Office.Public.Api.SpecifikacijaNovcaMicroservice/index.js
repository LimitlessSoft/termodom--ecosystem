const express = require('express')
const port = 5000
const routes = require('./routes/specificationRoutes')
const bodyParser = require('body-parser')
const errorHandler = require('./middleware/errorHandler')
const cors = require('cors')
const cookieParser = require('cookie-parser')

const TARGET_URL = 'https://api-office-develop.termodom.rs'
const BASE_PATH = '/specifikacija-novca'

const app = express()

const corsOptions = {
    origin: 'http://localhost:3000',
    methods: ['GET', 'POST', 'PUT', 'PATCH', 'DELETE', 'OPTIONS'],
    credentials: true,
}

app.use(cors(corsOptions))
// app.options('*', cors(corsOptions))

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: true }))
app.use(cookieParser())

app.use(BASE_PATH, routes)

app.use((req, res) => {
    const targetUrl = `${TARGET_URL}${req.path}`
    console.log(`Request method: ${req.method}`)

    if (req.method === 'GET') {
        return res.redirect(302, targetUrl)
    } else if (req.method === 'POST') {
        return res.redirect(307, targetUrl)
    } else {
        return res.status(405).json({ error: 'Method Not Allowed' })
    }
})

app.use(errorHandler)

app.listen(port, () => {
    console.log('Server started on port ' + port)
})
