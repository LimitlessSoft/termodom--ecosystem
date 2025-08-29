import { superbaseSchema } from '@/superbase/index'
import bcrypt from 'bcryptjs'
import { SALT_ROUNDS } from '@/constants/generalConstants'
import hashHelpers from '@/helpers/hashHelpers'

const tableName = 'users'
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
                        .from(tableName)
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
                            isAdmin: data.type === 'admin',
                        })
                    })
                })
        }),
    count: () =>
        new Promise(async (resolve, reject) => {
            const { count, error } = await superbaseSchema
                .from(tableName)
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
                    .from(tableName)
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
    getMultiple: async () =>
        new Promise(async (resolve, reject) => {
            const { data, error } = await superbaseSchema
                .from(tableName)
                .select(`*`)
                .order(`username`)

            if (error) {
                console.error('Error fetching users: ' + error.message)
                reject(new Error())
                return
            }

            // Exclude password from the returned data
            const users = data.map(({ password, ...rest }) => rest)
            resolve(users)
        }),
    getById: async (userId) =>
        new Promise(async (resolve, reject) => {
            const { data, error } = await superbaseSchema
                .from(tableName)
                .select(`id, username`)
                .eq('id', userId)
                .single()

            if (error) {
                console.error('Error fetching user: ' + error.message)
                reject(new Error())
                return
            }

            resolve(data)
        }),
    updateById: async (userId, { username, password }) =>
        new Promise(async (resolve, reject) => {
            try {
                if (!username || username.length < 3) {
                    const msg = 'Korisničko ime mora imati najmanje 3 karaktera'
                    console.warn(msg)
                    reject(msg)
                    return
                }

                if (password && password.length < 3) {
                    const msg = 'Lozinka mora imati najmanje 3 karaktera'
                    console.warn(msg)
                    reject(msg)
                    return
                }

                const updateData = { username }

                if (password) {
                    const hashedPassword = await hashHelpers.hashPassword(
                        password
                    )
                    updateData.password = hashedPassword
                }

                const { error } = await superbaseSchema
                    .from(tableName)
                    .update(updateData)
                    .eq('id', userId)

                if (error) {
                    const msg = 'User update error: ' + error.message
                    console.error(msg)
                    reject(new Error())
                    return
                }

                resolve(null)
            } catch (err) {
                console.error(err.message)
                reject(err)
            }
        }),
}
