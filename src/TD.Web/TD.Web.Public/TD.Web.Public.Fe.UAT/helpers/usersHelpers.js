import { vaultClient } from '../configs/vaultConfig.js'
import { PROJECT_URL, PUBLIC_API_CLIENT } from '../constants.js'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const { TEST_USER_PLAIN_PASSWORD } = await vaultClient.getSecret(
    'web/public/api'
)

const usersHelpers = {
    async registerMockUser(callback) {
        return await this.registerMockUserCore(null, callback)
    },
    async registerMockUserCore(data, callback) {
        const username = faker.string.alpha(10)

        await PUBLIC_API_CLIENT.users.register({
            username,
            password: data?.password || TEST_USER_PLAIN_PASSWORD,
            nickname: faker.string.alpha(10).toLowerCase(),
            dateOfBirth: faker.date.birthdate({
                min: 18,
                max: 65,
                mode: 'age',
            }),
            mobile: `06${10000000 + Math.floor(Math.random() * 90000000)}`,
            address: faker.location.street(),
            cityId: 3,
            favoriteStoreId: 121,
            mail: faker.internet.email(),
        })
        return callback(username)
    },
    async registerAndConfirmMockUser(webDbClient, data) {
        return await this.registerMockUserCore(data, async (username) => {
            return await webDbClient.usersRepository.setProcessingDate(
                username,
                new Date()
            )
        })
    },
    async uatLogin(driver, token) {
        await driver.get(PROJECT_URL)

        await driver.manage().addCookie({
            name: 'token',
            value: token,
            path: '/',
        })
    },
}

export default usersHelpers
