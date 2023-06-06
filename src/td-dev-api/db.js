const { MongoClient } = require('mongodb')

let client
let db

async function connect() {
    if(!client) {
        client = await MongoClient.connect('mongodb://192.168.0.3:27017').catch(e => { console.log(e) })
        db = client.db('td-dev-api')
    }
    return client
}

const getClient = () => client
const getDb = () => db

connect()

module.exports = { getClient, getDb }