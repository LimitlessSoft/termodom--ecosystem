const { Pool } = require('pg')
const UsersRepository = require('./src/usersRepository')
const ProductsRepository = require('./src/productsRepository')
const ProductGroupsRepository = require('./src/productGroupsRepository')
const UnitsRepository = require('./src/unitsRepository')
const ProductPriceGroupsRepository = require('./src/productPriceGroupsRepository')
const ProductPricesRepository = require('./src/productPricesRepository')
const OrdersRepository = require('./src/ordersRepository')
const OrderItemsRepository = require('./src/orderItemsRepository')
const UserPermissionsRepository = require('./src/userPermissionsRepository')
const StoresRepository = require('./src/storesRepository')
const StatisticsItemsRepository = require('./src/statisticsItemsRepository')
const SettingsRepository = require('./src/settingsRepository')
const ProfessionsRepository = require('./src/professionsRepository')
const ProductPriceGroupLevelRepository = require('./src/productPriceGroupLevelRepository')
const PaymentTypesRepository = require('./src/paymentTypesRepository')
const OrderOneTimeInformationRepository = require('./src/orderOneTimeInformationRepository')
const CalculatorItemsRepository = require('./src/calculatorItemsRepository')
const CitiesRepository = require('./src/citiesRepository')
const GlobalAlertsRepository = require('./src/globalAlertsRepository')
const KomercijalnoWebProductLinksRepository = require('./src/komercijalnoWebProductLinksRepository')
const ModuleHelpsRepository = require('./src/moduleHelpsRepository')
const ProductEntityProductGroupEntityRepository = require('./src/productEntityProductGroupEntityRepository')
const ProductGroupEntityUserEntityRepository = require('./src/productGroupEntityUserEntityRepository')

class WebDb {
    #pool
    #client

    constructor({ host, user, password, port, database }) {
        // return (async () => {
        this.#pool = new Pool({
            user,
            host,
            database,
            password,
            port,
            connectionTimeoutMillis: 5000,
        })

        //     await this.#connect()

        //     this.#initializeRepositories()

        //     return this
        // })()
    }

    static async create({ host, user, password, port, database }) {
        const instance = new WebDb({ host, user, password, port, database })
        await instance.#connect()
        instance.#initializeRepositories()
        return instance
    }

    async #connect() {
        try {
            this.#client = await this.#pool.connect()
        } catch (error) {
            throw new Error('Failed to connect to the database', {
                cause: error,
            })
        }
    }

    #initializeRepositories() {
        this.usersRepository = new UsersRepository(this.#client)
        this.userPermissionsRepository = new UserPermissionsRepository(
            this.#client
        )
        this.productsRepository = new ProductsRepository(this.#client)
        this.productPriceGroupsRepository = new ProductPriceGroupsRepository(
            this.#client
        )
        this.productPricesRepository = new ProductPricesRepository(this.#client)
        this.productGroupsRepository = new ProductGroupsRepository(this.#client)
        this.productPriceGroupLevelRepository =
            new ProductPriceGroupLevelRepository(this.#client)
        this.productEntityProductGroupEntityRepository =
            new ProductEntityProductGroupEntityRepository(this.#client)
        this.productGroupEntityUserEntityRepository =
            new ProductGroupEntityUserEntityRepository(this.#client)
        this.komercijalnoWebProductLinksRepository =
            new KomercijalnoWebProductLinksRepository(this.#client)
        this.ordersRepository = new OrdersRepository(this.#client)
        this.orderItemsRepository = new OrderItemsRepository(this.#client)
        this.orderOneTimeInformationRepository =
            new OrderOneTimeInformationRepository(this.#client)
        this.unitsRepository = new UnitsRepository(this.#client)
        this.storesRepository = new StoresRepository(this.#client)
        this.statisticsItemsRepository = new StatisticsItemsRepository(
            this.#client
        )
        this.paymentTypesRepository = new PaymentTypesRepository(this.#client)
        this.settingsRepository = new SettingsRepository(this.#client)
        this.professionsRepository = new ProfessionsRepository(this.#client)
        this.calculatorItemsRepository = new CalculatorItemsRepository(
            this.#client
        )
        this.citiesRepository = new CitiesRepository(this.#client)
        this.globalAlertsRepository = new GlobalAlertsRepository(this.#client)
        this.moduleHelpsRepository = new ModuleHelpsRepository(this.#client)
    }

    async disconnect() {
        if (this.#client) {
            try {
                await this.#client.release()
                this.#client = null
            } catch (error) {
                throw new Error('Failed to release the database connection', {
                    cause: error,
                })
            }
        }
    }
}

module.exports = {
    generateWebDbClient: async ({ host, user, password, port, database }) => {
        try {
            const webDbClient = await WebDb.create({
                host,
                user,
                password,
                port,
                database,
            })
            return webDbClient
        } catch (error) {
            throw new Error('Failed to generate WebDbClient:', { cause: error })
        }
    },
}
