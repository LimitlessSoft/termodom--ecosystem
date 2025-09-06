import { superbaseSchema } from '@/superbase/index'
import bcrypt from 'bcryptjs'
import hashHelpers from '@/helpers/hashHelpers'
import { quizzRepository } from '@/superbase/quizzRepository'
import usersQuizzRepository from '@/superbase/usersQuizzRepository'

export const userRepository = {
    tableName: 'users',
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
                        .from(userRepository.tableName)
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
                .from(userRepository.tableName)
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

            const hashedPassword = await hashHelpers.hashPassword(password)

            const { data, error } = await superbaseSchema
                .from(userRepository.tableName)
                .insert([{ username, password: hashedPassword, type }])
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
        }),
    getMultiple: async () =>
        new Promise(async (resolve, reject) => {
            const { data, error } = await superbaseSchema
                .from(userRepository.tableName)
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
                .from(userRepository.tableName)
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
                    .from(userRepository.tableName)
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
    assignQuizzes: async (userId, quizzIds) =>
        new Promise(async (resolve, reject) => {
            const { error } = await superbaseSchema
                .from(usersQuizzRepository.tableName)
                .insert(
                    quizzIds.map((quizzId) => ({
                        user_id: userId,
                        quizz_schema_id: quizzId,
                    }))
                )

            if (error) {
                const msg = 'Assigning quizzes to user error: ' + error.message
                console.error(msg)
                reject(new Error())
            }

            resolve(null)
        }),
    getAssignedQuizzes: async (userId) =>
        new Promise(async (resolve, reject) => {
            const { data, error } = await quizzRepository
                .asQueryable(
                    'id, name, quizz_session(*), users_quizz_schemas!inner()'
                )
                .eq('users_quizz_schemas.user_id', userId)
                .eq('quizz_session.created_by', userId)

            if (error) {
                console.error('Error fetching quizzes: ' + error.message)
                reject(new Error())
                return
            }

            const adjustedData = data.map(({ quizz_session, ...rest }) => {
                const hasAtLeastOneLockedSession = quizz_session.some(
                    (session) =>
                        session.type === 'ocenjivanje' &&
                        session.ignore_run === false &&
                        !!session.completed_at
                )

                return {
                    ...rest,
                    hasAtLeastOneLockedSession,
                }
            })

            resolve(adjustedData)
        }),
    getUnassignedQuizzes: async (userId) =>
        new Promise(async (resolve, reject) => {
            const { data, error } = await quizzRepository.asQueryable(
                `id, name, ${usersQuizzRepository.tableName}(*)`
            )

            if (error) {
                console.error('Error fetching quizzes: ' + error.message)
                reject(new Error())
                return
            }
            resolve(
                data
                    .filter(
                        (x) =>
                            !x[usersQuizzRepository.tableName]?.some(
                                (y) =>
                                    y.user_id.toString() === userId.toString()
                            )
                    )
                    .map((x) => {
                        return { id: x.id, name: x.name }
                    })
            )
        }),
}
