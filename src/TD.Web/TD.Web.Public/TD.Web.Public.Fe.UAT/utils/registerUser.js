import { webPublicApi } from '../api/webPublicApi.js'
import confirmUserRegistrationQuery from '../queries/insertTestUserQuery.js'

const registerUser = async (username, client) =>
    await webPublicApi
        .put('/register', {
            username,
            password: 'Test123!',
            nickname: username,
            dateOfBirth: new Date(new Date().getFullYear() - 19),
            mobile: `06${Math.floor(10000000 + Math.random() * 90000000)}`,
            address: `${username} address`,
            cityId: 3,
            favoriteStoreId: 121,
            mail: `${username.toLowerCase()}@test.com`,
        })
        .then(
            async () =>
                await client.query(confirmUserRegistrationQuery(username))
        )

export default registerUser
