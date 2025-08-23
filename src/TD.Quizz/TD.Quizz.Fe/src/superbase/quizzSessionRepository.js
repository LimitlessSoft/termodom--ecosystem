import { superbaseSchema } from '@/superbase/index'
import { auth } from '@/auth'

const tableName = 'quizz_session'
export const quizzSessionRepository = {
    getCurrent: async (schemaId, type) =>
        new Promise(async (resolve, reject) => {
            const currentUser = await auth()
            if (!currentUser) {
                reject('Niste prijavljeni')
                return
            }

            if (type !== 'proba' && type !== 'ocenjivanje') {
                reject('Neispravan tip kviza')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .select('*')
                .eq('created_by', currentUser.user.id)
                .eq('type', type)
                .eq('quizz_schema_id', schemaId)
                .is('completed_at', null)
                .maybeSingle()

            if (error) {
                console.error(
                    'Error fetching current quizz session: ' + error.message
                )
                reject(new Error())
                return
            }

            if (!data) {
                const newOne = await quizzSessionRepository.create(
                    schemaId,
                    type
                )
                resolve(newOne.id)
            }

            resolve(data.id)
        }),
    create: async (schemaId, type) =>
        new Promise(async (resolve, reject) => {
            const currentUser = await auth()
            if (!currentUser) {
                reject('Niste prijavljeni')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .insert({
                    created_by: currentUser.user.id,
                    type: type,
                    quizz_schema_id: schemaId,
                })
                .select('*')
                .single()

            if (error) {
                console.error('Error creating quizz session: ' + error.message)
                reject(new Error())
                return
            }

            resolve(data)
        }),
    getById: async (sessionId) =>
        new Promise(async (resolve, reject) => {
            if (!sessionId) {
                reject('ID sesije kviza je obavezan')
                return
            }

            const currentUser = await auth()
            if (!currentUser) {
                reject('Niste prijavljeni')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .select(
                    '*, users(*), quizz_schema(*), quizz_session_answer(*, quizz_question(*))'
                )
                .eq('id', sessionId)
                .eq('created_by', currentUser.user.id)
                .single()

            if (error) {
                console.error(
                    'Error fetching quizz session by ID: ' + error.message
                )
                reject(404)
                return
            }

            console.log('AAAAAAAAA')

            const { users, quizz_schema, quizz_session_answer, ...rest } = data

            const mappedAnswers = new Map()
            for (const answer of quizz_session_answer) {
                const questionId = answer.question_id

                if (!mappedAnswers.has(questionId)) {
                    const correctAnswers = []
                    const correctAnswerTexts = []

                    answer.quizz_question.answers.forEach((ansObj, index) => {
                        if (ansObj.isCorrect) {
                            correctAnswers.push(index)
                            correctAnswerTexts.push(ansObj.text)
                        }
                    })

                    mappedAnswers.set(questionId, {
                        question: answer.quizz_question.text,
                        pickedAnswers: [answer.answer_index],
                        correctAnswers,
                        correctAnswerTexts,
                    })
                } else {
                    mappedAnswers
                        .get(questionId)
                        .pickedAnswers.push(answer.answer_index)
                }
            }

            for (const dto of mappedAnswers.values()) {
                dto.isCorrect =
                    dto.pickedAnswers.length === dto.correctAnswers.length &&
                    dto.pickedAnswers.every((val) =>
                        dto.correctAnswers.includes(val)
                    )
            }

            console.log(Array.from(mappedAnswers.values()))

            resolve(Array.from(mappedAnswers.values()))

            // const answers = quizz_session_answer.map((a) => {
            //     console.log(a.quizz_question.answers)
            //     const question = a.quizz_question.text
            //     const pickedAnswer = a.answer_index
            //     const correctAnswer = a.quizz_question.answers.findIndex(
            //         (z) => z.isCorrect
            //     )
            //     const isCorrect = pickedAnswer === correctAnswer
            //     const correctAnswerText =
            //         a.quizz_question.answers[correctAnswer]?.text
            //     return {
            //         question,
            //         pickedAnswer,
            //         correctAnswer,
            //         isCorrect,
            //         correctAnswerText,
            //     }
            // })
            // resolve({
            //     ...rest,
            //     answers: answers,
            //     user: data.users.username,
            //     quizzSchemaName: data.quizz_schema.name,
            // })
        }),
    getNextQuestion: async (sessionId) =>
        new Promise(async (resolve, reject) => {
            if (!sessionId) {
                reject('ID sesije kviza je obavezan')
                return
            }

            const currentUser = await auth()
            if (!currentUser) {
                reject('Niste prijavljeni')
                return
            }

            const { data, error } = await superbaseSchema
                .from('quizz_session')
                .select(
                    '*, quizz_schema(*, quizz_question(*)), quizz_session_answer(*)'
                )
                .eq('id', sessionId)
                .single()

            if (error) {
                console.error('Error fetching next question: ' + error.message)
                reject(new Error())
                return
            }
            if (data.completed_at) {
                reject(`completed`)
                return
            }
            if (
                data.quizz_session_answer.length >=
                data.quizz_schema.quizz_question.length
            ) {
                await quizzSessionRepository.setCompleted(sessionId)
                reject(`completed`)
            }
            const notAnswered = data.quizz_schema.quizz_question.filter(
                (q) =>
                    !data.quizz_session_answer.some(
                        (a) => a.question_id === q.id
                    )
            )
            if (notAnswered.length === 0) {
                await quizzSessionRepository.setCompleted(sessionId)
                reject(`completed`)
                return
            }

            const nextQuestion = notAnswered[0]

            // add number of required correct answers
            nextQuestion.requiredAnswers = nextQuestion.answers.filter(
                (answer) => answer.isCorrect
            ).length
            // filter out `isCorrect` from .answers
            nextQuestion.answers = nextQuestion.answers.map((a) => {
                const { isCorrect, ...rest } = a
                return rest
            })
            // add answered count
            nextQuestion.answeredCount = data.quizz_session_answer.length || 0
            // add total count
            nextQuestion.totalCount =
                data.quizz_schema.quizz_question.length || 0
            // add quizz schema name
            nextQuestion.quizzSchemaName = data.quizz_schema.name
            // add session id
            nextQuestion.sessionId = sessionId
            console.log(nextQuestion)
            resolve(nextQuestion)
        }),
    setCompleted: async (sessionId) =>
        new Promise(async (resolve, reject) => {
            if (!sessionId) {
                reject('ID sesije kviza je obavezan')
                return
            }

            const currentUser = await auth()
            if (!currentUser) {
                reject('Niste prijavljeni')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .update({ completed_at: new Date() })
                .eq('id', sessionId)
                .eq('created_by', currentUser.user.id)
                .select('*')
                .single()

            if (error) {
                console.error(
                    'Error setting quizz session as completed: ' + error.message
                )
                reject(new Error())
                return
            }

            resolve(data)
        }),
    setAnswer: async (sessionId, questionId, answerIndexes) =>
        new Promise(async (resolve, reject) => {
            if (!sessionId) {
                reject('ID sesije kviza je obavezan')
                return
            }
            if (!answerIndexes || answerIndexes.length === 0) {
                reject('Broj odgovora je obavezan')
                return
            }

            const currentUser = await auth()
            if (!currentUser) {
                reject('Niste prijavljeni')
                return
            }

            const { data: session, error: sessionError } = await superbaseSchema
                .from(tableName)
                .select('*, quizz_session_answer(*)')
                .eq('id', sessionId)
                .eq('created_by', currentUser.user.id)
                .maybeSingle()
            if (sessionError) {
                console.error(
                    'Error fetching quizz session: ' + sessionError.message
                )
                reject(new Error())
                return
            }
            if (!session) {
                reject('Sesija kviza ne postoji')
                return
            }
            if (session.completed_at) {
                reject('Sesija kviza je već završena')
                return
            }
            const existingAnswer = session.quizz_session_answer.find(
                (a) => a.question_id === questionId
            )
            if (existingAnswer) {
                reject('Već ste odgovorili na ovo pitanje')
                return
            }

            const { data, error } = await superbaseSchema
                .from('quizz_session_answer')
                .insert(
                    answerIndexes.map((answerIndex) => ({
                        quizz_session_id: sessionId,
                        question_id: questionId,
                        answer_index: answerIndex,
                    }))
                )
                .select('*')
            // .single()

            if (error) {
                console.error(
                    'Error setting answer for quizz session: ' + error.message
                )
                reject(new Error())
                return
            }

            resolve(data)
        }),
    getCompleted: async () =>
        new Promise(async (resolve, reject) => {
            const { data, error } = await superbaseSchema
                .from(tableName)
                .select(
                    '*, users(*), quizz_schema(*), quizz_session_answer(*, quizz_question(*))'
                )
                .not('completed_at', 'is', null)
                .order('completed_at', { ascending: false })
                .limit(20)

            if (error) {
                console.error(
                    'Error fetching completed quizz sessions: ' + error.message
                )
                reject(new Error())
                return
            }

            const result = data.map((session) => {
                const { users, quizz_schema, quizz_session_answer, ...rest } =
                    session
                const answers = quizz_session_answer.map((a) => {
                    const question = a.quizz_question.text
                    const pickedAnswer = a.answer_index
                    const correctAnswer = a.quizz_question.answers.findIndex(
                        (z) => z.isCorrect
                    )
                    const isCorrect = pickedAnswer === correctAnswer
                    const correctAnswerText =
                        a.quizz_question.answers[correctAnswer]?.text
                    return {
                        question,
                        pickedAnswer,
                        correctAnswer,
                        isCorrect,
                        correctAnswerText,
                    }
                })
                return {
                    ...rest,
                    answers: answers,
                    user: session.users.username,
                    quizzSchemaName: session.quizz_schema.name,
                }
            })

            resolve(result)
        }),
}
