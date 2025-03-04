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

class WebDbClient {
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
        this.productsRepository = new ProductsRepository(this.#pgClient)
        this.productPriceGroupsRepository = new ProductPriceGroupsRepository(
            this.#pgClient
        )
        this.productPricesRepository = new ProductPricesRepository(this.#pgClient)
        this.productGroupsRepository = new ProductGroupsRepository(this.#pgClient)
        this.productPriceGroupLevelRepository =
            new ProductPriceGroupLevelRepository(this.#pgClient)
        this.productEntityProductGroupEntityRepository =
            new ProductEntityProductGroupEntityRepository(this.#pgClient)
        this.productGroupEntityUserEntityRepository =
            new ProductGroupEntityUserEntityRepository(this.#pgClient)
        this.komercijalnoWebProductLinksRepository =
            new KomercijalnoWebProductLinksRepository(this.#pgClient)
        this.ordersRepository = new OrdersRepository(this.#pgClient)
        this.orderItemsRepository = new OrderItemsRepository(this.#pgClient)
        this.orderOneTimeInformationRepository =
            new OrderOneTimeInformationRepository(this.#pgClient)
        this.unitsRepository = new UnitsRepository(this.#pgClient)
        this.storesRepository = new StoresRepository(this.#pgClient)
        this.statisticsItemsRepository = new StatisticsItemsRepository(
            this.#pgClient
        )
        this.paymentTypesRepository = new PaymentTypesRepository(this.#pgClient)
        this.settingsRepository = new SettingsRepository(this.#pgClient)
        this.professionsRepository = new ProfessionsRepository(this.#pgClient)
        this.calculatorItemsRepository = new CalculatorItemsRepository(
            this.#pgClient
        )
        this.citiesRepository = new CitiesRepository(this.#pgClient)
        this.globalAlertsRepository = new GlobalAlertsRepository(this.#pgClient)
        this.moduleHelpsRepository = new ModuleHelpsRepository(this.#pgClient)
    }

    async disconnect() {
        await this.#pgClient.release()
    }
}

class WebDbClientFactory {
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
        return new WebDbClient(await this.#pool.connect())
    }
}

module.exports = WebDbClientFactory
