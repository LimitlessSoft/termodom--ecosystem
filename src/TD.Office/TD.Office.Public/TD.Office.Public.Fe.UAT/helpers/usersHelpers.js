import { faker } from '@faker-js/faker/locale/sr_RS_latin'
import { vaultClient } from '../configs/vaultConfig.js'

const { TEST_USER_HASHED_PASSWORD } = await vaultClient.getSecret(
    'office/public/api'
)

const usersHelpers = {
    async createMockUser(officeDbClient) {
        return await officeDbClient.usersRepository.create({
            username: faker.string.alpha(10),
            password: TEST_USER_HASHED_PASSWORD,
            nickname: faker.string.alpha(11),
        })
    },
    async hardDeleteMockUser(officeDbClient, username) {
        await officeDbClient.usersRepository.hardDelete(username)
    },
}

export default usersHelpers
