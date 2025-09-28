import { superbaseSchema } from '@/superbase/index'
import usersQuizzRepository from './usersQuizzRepository'
import { userRepository } from './userRepository'
import { logServerErrorAndReject } from '@/helpers/errorhelpers'
import { settingRepository } from '@/superbase/settingRepository'
import { SETTINGS_KEYS } from '@/constants/settingsConstants'

const tableName = 'quizz_schema'
export const quizzRepository = {
    async getMultiple() {
        const { data, error } = await quizzRepository
            .asQueryable(
                'id, name, quizz_question(count), quizz_session(*), users_quizz_schemas(user_id)'
            )
            .order('name')

        if (error) throw error

        const transformed = data.map(
            ({
                quizz_question,
                quizz_session,
                users_quizz_schemas,
                ...rest
            }) => {
                const assignedUserIds = users_quizz_schemas.map(
                    (uq) => uq.user_id
                )

                const hasAtLeastOneLockedSession = quizz_session.some(
                    (session) =>
                        session.type === 'ocenjivanje' &&
                        session.ignore_run === false &&
                        !!session.completed_at &&
                        assignedUserIds.includes(session.created_by)
                )

                return {
                    ...rest,

                    hasAtLeastOneLockedSession,
                    quizz_questions_count: quizz_question?.[0]?.count ?? 0,
                }
            }
        )

        return transformed
    },
    asQueryable: (columns) =>
        superbaseSchema
            .from(tableName)
            .select(columns || '*')
            .is('is_active', true),
    exists: async (name) =>
        new Promise(async (resolve, reject) => {
            if (!name || name.length < 3) {
                reject('Naziv kviza mora imati najmanje 3 karaktera')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .select('*')
                .eq('name', name)
                .single()

            if (logServerErrorAndReject(error, reject)) return
            resolve(!!data)
        }),
    create: async (name) =>
        new Promise(async (resolve, reject) => {
            if (!name || name.length < 3) {
                reject('Naziv kviza mora imati najmanje 3 karaktera')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .insert({ name })
                .select()

            if (logServerErrorAndReject(error, reject)) return
            resolve(data[0])
        }),
    assignToAllUsers: (quizzId) =>
        new Promise(async (resolve, reject) => {
            const { data: users, error: usersError } = await superbaseSchema
                .from(userRepository.tableName)
                .select('id')

            if (logServerErrorAndReject(usersError, reject)) return
            const { error: assigningError } = await superbaseSchema
                .from(usersQuizzRepository.tableName)
                .upsert(
                    users.map((user) => ({
                        user_id: user.id,
                        quizz_schema_id: quizzId,
                    })),
                    { ignoreDuplicates: true }
                )

            if (logServerErrorAndReject(assigningError, reject)) return
            resolve(null)
        }),
    getById: async (id) =>
        new Promise(async (resolve, reject) => {
            if (!id) {
                reject('ID kviza je obavezan')
                return
            }

            const queryTask = superbaseSchema
                .from(tableName)
                .select('*, quizz_question(*)')
                .eq('id', id)
                .single()
            const defaultDurationTask = settingRepository.getByKey(
                SETTINGS_KEYS.DEFAULT_QUESTION_DURATION
            )

            const { data, error } = await queryTask
            const defaultDuration = parseInt(await defaultDurationTask)

            if (isNaN(defaultDuration)) {
                throw new Error(
                    `Failed to parse defaultDuration: ${await defaultDurationTask}`
                )
            }

            if (logServerErrorAndReject(error, reject)) return
            data.defaultQuestionDuration = defaultDuration
            resolve(data)
        }),
    update: async (quizz) =>
        new Promise(async (resolve, reject) => {
            if (!quizz || !quizz.id || !quizz.name || !quizz.questions) {
                reject('ID, naziv i pitanja kviza su obavezni')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .update({
                    name: quizz.name,
                })
                .eq('id', quizz.id)
                .select()

            if (logServerErrorAndReject(error, reject)) return

            const oldQuestions = quizz.questions.filter((x) => x.id)
            const newQuestions = quizz.questions.filter((x) => !x.id)
            if (oldQuestions.length > 0) {
                const { data: questionData, error: questionError } =
                    await superbaseSchema.from('quizz_question').upsert(
                        oldQuestions.map((q) => ({
                            id: q.id,
                            title: q.title,
                            text: q.text,
                            image: q.image,
                            answers: q.answers,
                            duration: q.duration,
                            quizz_schema_id: q.quizz_schema_id,
                        }))
                    )
                if (logServerErrorAndReject(questionError, reject)) return
            }

            if (newQuestions.length > 0) {
                const { data: questionData, error: questionError } =
                    await superbaseSchema.from('quizz_question').insert(
                        newQuestions.map((q) => ({
                            title: q.title,
                            text: q.text,
                            image: q.image,
                            answers: q.answers,
                            duration: q.duration,
                            quizz_schema_id: quizz.id,
                        }))
                    )
                if (logServerErrorAndReject(questionError, reject)) return
            }

            resolve(data[0])
        }),
}
