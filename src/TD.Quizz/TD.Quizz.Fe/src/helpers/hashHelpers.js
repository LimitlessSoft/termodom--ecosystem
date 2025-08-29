import { SALT_ROUNDS } from '@/constants/generalConstants'
import bcrypt from 'bcryptjs'

const hashHelpers = {
    hashPassword: (password) =>
        new Promise((resolve, reject) => {
            bcrypt.hash(password, SALT_ROUNDS, (err, hash) => {
                if (err) return reject(err)
                resolve(hash)
            })
        }),
}

export default hashHelpers
