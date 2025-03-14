import { faker } from '@faker-js/faker/locale/sr_RS_latin'
import { vaultClient } from '../configs/vaultConfig.js'

const { TEST_USER_HASHED_PASSWORD } = await vaultClient.getSecret(
    'web/admin/api'
)

const usersHelpers = {
    async createMockUser(webDbClient) {
        return await webDbClient.usersRepository.create({
            cityId: 51,
            mail: faker.internet.email(),
            mobile: `06${10000000 + Math.floor(Math.random() * 90000000)}`,
            type: 1,
            address: faker.location.street(),
            username: faker.string.alpha(10),
            password: TEST_USER_HASHED_PASSWORD,
            nickname: faker.string.alpha(10).toLowerCase(),
            favoriteStoreId: -5,
            dateOfBirth: faker.date.birthdate({
                min: 18,
                max: 65,
                mode: 'age',
            }),
        })
    },
    async hardDeleteMockUser(webDbClient, username) {
        await webDbClient.usersRepository.hardDelete(username)
    },
}

export default usersHelpers
