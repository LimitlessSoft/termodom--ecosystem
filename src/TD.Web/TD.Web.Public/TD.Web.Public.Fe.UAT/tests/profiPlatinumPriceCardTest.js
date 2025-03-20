import { webDbClientFactory } from '../configs/dbConfig'

const webDbClient = await webDbClientFactory.create()

const state = {}

export default {
    beforeExecution: async () => {},
    afterExecution: async () => {},
    execution: async (driver) => {},
}
