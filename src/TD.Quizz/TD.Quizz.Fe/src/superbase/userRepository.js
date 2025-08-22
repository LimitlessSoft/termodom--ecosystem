import { superbaseSchema } from '@/superbase/index'
import bcrypt from 'bcryptjs'
import { SALT_ROUNDS } from '@/constants/generalConstants'

export const userRepository = {
    login: async (username, password) =>
        new Promise(async (resolve, reject) => {
            if (!username || !password) {
                reject('Korisničko ime i lozinka su obavezni')
                return
            }
            if (username.length < 3 || password.length < 3) {
                reject(
                    'Korisničko ime i lozinka moraju imati najmanje 3 karaktera'
                )
                return
            }
            userRepository
                .count()
                .catch(reject)
                .then(async (count) => {
                    if (count === 0) {
                        await userRepository
                            .create(username, password, 'admin')
                            .catch(reject)
                            .then(resolve)
                        return
                    }

                    const { data, error } = await superbaseSchema
                        .from('users')
                        .select('*')
                        .eq('username', username)
                        .single()
                    if (error) {
                        const msg = 'Login error: ' + error.message
                        console.error(msg)
                        reject(new Error())
                    }

                    if (!data) {
                        console.warn(
                            'No user found with the provided credentials'
                        )
                        reject()
                        return
                    }

                    bcrypt.compare(password, data.password, (err, isMatch) => {
                        if (err) {
                            console.error(
                                'Bcrypt compare error: ' + err.message
                            )
                            reject(new Error())
                            return
                        }
                        if (!isMatch) {
                            console.warn('Password does not match')
                            reject('Pogrešno korisničko ime ili lozinka')
                            return
                        }
                        resolve({
                            id: data.id,
                            username: data.username,
                            isAdmin: true,
                        })
                    })
                })
        }),
    count: () =>
        new Promise(async (resolve, reject) => {
            const { count, error } = await superbaseSchema
                .from('users')
                .select('*', { count: 'exact' })

            if (error) reject(new Error())

            resolve(count)
        }),
    create: (username, password, type = 'user') =>
        new Promise(async (resolve, reject) => {
            if (!username || !password) {
                const msg = 'Korisničko ime i lozinka su obavezni'
                console.warn(msg)
                reject(msg)
                return
            }
            if (username.length < 3 || password.length < 3) {
                const msg =
                    'Korisničko ime i lozinka moraju imati najmanje 3 karaktera'
                console.warn(msg)
                reject(msg)
                return
            }
            bcrypt.hash(password, SALT_ROUNDS, async (err, hash) => {
                if (err) {
                    console.error('Hashing error: ' + err.message)
                    reject(new Error())
                    return
                }
                const { data, error } = await superbaseSchema
                    .from('users')
                    .insert([{ username, password: hash, type }])
                    .select('*')
                    .single()

                if (error) {
                    const msg = 'User creation error: ' + error.message
                    console.error(msg)
                    reject(new Error())
                }

                resolve({
                    id: data.id,
                    username: data.username,
                    isAdmin: data.type === 'admin',
                })
            })
        }),
}
