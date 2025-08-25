import { superbaseSchema } from '@/superbase/index'

const tableName = 'quizz_schema'
export const quizzRepository = {
    getMultiple: async () =>
        new Promise(async (resolve, reject) => {
            const { data, error } = await superbaseSchema
                .from(tableName)
                .select('*, quizz_question(count), quizz_session(*)')
                .order(`name`)

            if (error) {
                console.error('Error fetching quizzes: ' + error.message)
                reject(new Error())
                return
            }

            const transformed = data.map(
                ({ quizz_question, quizz_session, ...rest }) => {
                    const hasAtLeastOneLockedSession = quizz_session.some(
                        (session) =>
                            session.type === 'ocenjivanje' &&
                            session.ignore_run === false &&
                            !!session.completed_at
                    )

                    return {
                        ...rest,
                        hasAtLeastOneLockedSession,
                        quizz_questions_count: quizz_question?.[0]?.count ?? 0,
                    }
                }
            )

            console.dir(data, { depth: 5 })

            resolve(transformed)
        }),
    getActive: async () =>
        new Promise(async (resolve, reject) => {
            const { data, error } = await superbaseSchema
                .from(tableName)
                .select('*')
                .eq('is_active', true)
                .order(`name`)
            if (error) {
                console.error('Error fetching active quizzes: ' + error.message)
                reject(new Error())
                return
            }
            resolve(data)
        }),
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

            if (error) {
                console.error('Error checking quiz existence: ' + error.message)
                reject(new Error())
                return
            }

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

            if (error) {
                console.error('Error creating quiz: ' + error.message)
                reject(new Error())
                return
            }

            resolve(data[0])
        }),
    getById: async (id) =>
        new Promise(async (resolve, reject) => {
            if (!id) {
                reject('ID kviza je obavezan')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .select('*, quizz_question(*)')
                .eq('id', id)
                .single()

            if (error) {
                console.error('Error fetching quiz by ID: ' + error.message)
                reject(new Error())
                return
            }

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

            if (error) {
                console.error('Error updating quiz: ' + error.message)
                reject(new Error())
                return
            }

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
                            quizz_schema_id: q.quizz_schema_id,
                        }))
                    )
                if (questionError) {
                    console.error(
                        'Error updating quiz questions: ' +
                            questionError.message
                    )
                    reject(new Error())
                    return
                }
            }

            if (newQuestions.length > 0) {
                const { data: questionData, error: questionError } =
                    await superbaseSchema.from('quizz_question').insert(
                        newQuestions.map((q) => ({
                            title: q.title,
                            text: q.text,
                            image: q.image,
                            answers: q.answers,
                            quizz_schema_id: quizz.id,
                        }))
                    )
                if (questionError) {
                    console.error(
                        'Error inserting new quiz questions: ' +
                            questionError.message
                    )
                    reject(new Error())
                    return
                }
            }

            resolve(data[0])
        }),
}
