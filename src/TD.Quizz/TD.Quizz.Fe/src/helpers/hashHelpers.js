import { SALT_ROUNDS } from '@/constants/generalConstants'
import bcrypt from 'bcryptjs'
import { logServerError, logServerErrorAndReject } from '@/helpers/errorhelpers'

const hashHelpers = {
    hashPassword: (password) =>
        new Promise((resolve, reject) => {
            bcrypt.hash(password, SALT_ROUNDS, (err, hash) => {
                if (logServerErrorAndReject(err, reject)) return
                resolve(hash)
            })
        }),
}

export default hashHelpers
