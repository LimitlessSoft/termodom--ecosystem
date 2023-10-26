const express = require('express')
const app = express()
const port = 43100

global.__basedir = __dirname;

const softwareRouter = require('./routers/software')
app.use('/software', softwareRouter)

app.get('/', (req, res) => {
    res.send('Hello world!')
})

app.listen(port, () => {
    console.log(`Started on port ${port}!`)
})