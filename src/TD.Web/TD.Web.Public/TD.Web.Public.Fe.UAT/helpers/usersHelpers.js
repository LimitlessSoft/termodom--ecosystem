import { PUBLIC_API_CLIENT } from '../constants.js'
import { faker } from '@faker-js/faker/locale/sr_RS_latin'

const usersHelpers = {
    registerUser: async (webDbClient, callback) => {
        const username = faker.string.fromCharacters(
            'abcdefghijklmnopqrstuvwxyz',
            10
        )
        console.log(username)
        const password = 'Test123!'
        await PUBLIC_API_CLIENT.users.registerUser(
            username,
            password,
            faker.date.birthdate({ min: 18, max: 65, mode: 'age' }),
            `06${10000000 + Math.floor(Math.random() * 90000000)}`,
            faker.location.street(),
            3,
            121,
            faker.internet.email()
        )
        callback(username, password)
    },
    registerAndConfirmUser: async (webDbClient, callback) => {
        await usersHelpers.registerUser(
            webDbClient,
            async (username, password) => {
                await webDbClient.usersRepository.setProcessingDate(
                    username,
                    new Date()
                )
                callback(username, password)
            }
        )
    },
}

export default usersHelpers
