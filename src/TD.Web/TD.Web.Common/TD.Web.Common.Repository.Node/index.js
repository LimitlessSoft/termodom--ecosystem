const { Pool } = require('pg')
const UserRepository = require('./src/userRepository')

class WebDb {
    constructor(host, user, password, port, database) {
        this.pool = new Pool({
            user,
            host,
            database,
            password,
            port,
        })
        this.initialize = async () => {
            // Stateful
            this.client = await this.pool.connect()
            // End stateful
            
            // Stateless
            this.disconnect = async () => await this.client.end()
            this.userRepository = new UserRepository(this.client)
            // End stateless
        }
    }
}

module.exports = {
    GenerateWebDbClient:
        async (host, user, password, port, database) => {
            const webDbClient = new WebDb(host, user, password, port, database)
            await webDbClient.initialize()
            return webDbClient
        }
}