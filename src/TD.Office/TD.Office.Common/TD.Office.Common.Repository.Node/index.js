const { Pool } = require('pg')
const UsersRepository = require('./src/usersRepository')
const UserPermissionsRepository = require('./src/userPermissionsRepository')
const TransportOrdersRepository = require('./src/transportOrdersRepository')
const MoneySpecificationRepository = require('./src/moneySpecificationRepository')
const NotesRepository = require('./src/notesRepository')

class OfficeDbClient {
    #pgClient

    constructor(pgClient) {
        this.#pgClient = pgClient
        this.#initializeRepositories()
    }

    #initializeRepositories() {
        this.usersRepository = new UsersRepository(this.#pgClient)
        this.userPermissionsRepository = new UserPermissionsRepository(
            this.#pgClient
        )
        this.transportOrdersRepository = new TransportOrdersRepository(
            this.#pgClient
        )
        this.moneySpecificationRepository = new MoneySpecificationRepository(
            this.#pgClient
        )
        this.notesRepository = new NotesRepository(this.#pgClient)
    }

    async disconnect() {
        await this.#pgClient.release()
    }
}

class OfficeDbClientFactory {
    #pool

    constructor({ host, user, password, port, database }) {
        this.#pool = new Pool({
            user,
            host,
            database,
            password,
            port,
            connectionTimeoutMillis: 5000,
        })
    }

    async create() {
        return new OfficeDbClient(await this.#pool.connect())
    }
}

module.exports = OfficeDbClientFactory
