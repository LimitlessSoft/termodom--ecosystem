import { superbaseSchema } from '@/superbase/index'
import { auth } from '@/auth'
import { logServerErrorAndReject } from '@/helpers/errorhelpers'
import usersQuizzRepository from './usersQuizzRepository'
import { questionHelpers } from '@/helpers/questionHelpers'
import { TIMEOUT_ANSWER_INDEX } from '@/constants/generalConstants'

const tableName = 'quizz_session'
export const quizzSessionRepository = {
    getCurrent: async (schemaId, type) =>
        new Promise(async (resolve, reject) => {
            const currentUser = await auth()
            if (!currentUser) {
                reject('Niste prijavljeni')
                return
            }

            if (
                type !== 'proba' &&
                type !== 'ocenjivanje' &&
                type !== 'ucenje'
            ) {
                reject('Neispravan tip kviza')
                return
            }

            const { data, error } = await superbaseSchema
                .from(tableName)
                .select('*')
                .eq('created_by', currentUser.user.id)
                .eq('type', type)
                .eq('quizz_schema_id', schemaId)

            if (logServerErrorAndReject(error, reject)) return
            const isDisabled = () =>
                data.some(
                    (session) =>
                        session.ignore_run === false && session.completed_at
                )

            if (type === 'ocenjivanje' && isDisabled()) {
                reject(
                    'Ne možete započeti ocenjivanu sesiju jer ste je već odradili. Kontaktirajte administratora.'
                )

                return
            }

            const unfinished = data.find((session) => !session.completed_at)
            if (unfinished) {
                resolve(unfinished.id)
                return
            }

            const newOne = await quizzSessionRepository.create(schemaId, type)
            resolve(newOne.id)
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

            if (logServerErrorAndReject(error, reject)) return
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
                .single()

            if (logServerErrorAndReject(error, (_) => reject(404))) return
            if (
                currentUser.user.isAdmin !== true &&
                data.created_by != currentUser.user.id
            ) {
                reject(404)
                return
            }
            data.questions = Object.values(
                Object.groupBy(data.quizz_session_answer, (x) => x.question_id)
            ).map((answers) => {
                const { quizz_question } = answers[0]
                const isCorrect = answers.every((a) =>
                    questionHelpers.isCorrectAnswer(
                        quizz_question,
                        a.answer_index
                    )
                )
                return { id: quizz_question.id, answered_correctly: isCorrect }
            })
            data.user = data.users.username
            delete data.quizz_session_answer
            resolve(data)
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

            if (logServerErrorAndReject(error, reject)) return
            if (data.completed_at) {
                reject(`completed`)
                return
            }
            if (
                data.quizz_session_answer.length >=
                data.quizz_schema.quizz_question.reduce(
                    (a, v) => a + v.answers.filter((z) => z.isCorrect),
                    0
                ).length
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
            const startTimer = questionHelpers.getStartCountTime(data)
            if (
                questionHelpers.didAnswerTimeOut(
                    startTimer,
                    nextQuestion.duration
                )
            ) {
                await quizzSessionRepository.setAnswer(
                    sessionId,
                    nextQuestion.id,
                    [TIMEOUT_ANSWER_INDEX]
                )
                return quizzSessionRepository
                    .getNextQuestion(sessionId)
                    .then(resolve)
                    .catch(reject)
            }
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
            nextQuestion.answeredCount =
                (Object.values(
                    Object.groupBy(
                        data.quizz_session_answer,
                        (x) => x.question_id
                    )
                ).length || 0) + 1
            // add total count
            nextQuestion.totalCount =
                data.quizz_schema.quizz_question.length || 0
            // add quizz schema name
            nextQuestion.quizzSchemaName = data.quizz_schema.name
            // add session id
            nextQuestion.sessionId = sessionId
            nextQuestion.startCountTime = startTimer
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
            if (logServerErrorAndReject(error, reject)) return
            resolve(data)
        }),
    async unlockRatingSessions(schemaId, userId) {
        const usersQuizzes = await usersQuizzRepository.getMultipleByQuizzId(
            schemaId
        )

        if (usersQuizzes.length === 0) {
            return
        }

        const userIds = usersQuizzes.map((uq) => uq.user_id)

        let query = superbaseSchema
            .from(tableName)
            .update({ ignore_run: true })
            .eq('quizz_schema_id', schemaId)
            .eq('type', 'ocenjivanje')
            .is('ignore_run', false)

        if (userId) {
            query = query.eq('created_by', userId)
        } else {
            query = query.in('created_by', userIds)
        }

        const { error } = await query

        if (error) throw error
    },
    setAnswer: async (sessionId, questionId, answerIndexes) =>
        new Promise(async (resolve, reject) => {
            const currentUser = await auth()
            if (!currentUser) {
                reject('Niste prijavljeni')
                return
            }

            const { data: session, error: sessionError } = await superbaseSchema
                .from(tableName)
                .select(
                    '*, quizz_schema(quizz_question(*)), quizz_session_answer(*)'
                )
                .eq('id', sessionId)
                .eq('created_by', currentUser.user.id)
                .eq('quizz_schema.quizz_question.id', questionId)
                .maybeSingle()

            const numberOfRequiredAnswers =
                session.quizz_schema.quizz_question[0].answers.filter(
                    (question) => question.isCorrect
                ).length

            const numberOfMissingAnswers =
                numberOfRequiredAnswers - answerIndexes.length

            for (let i = 0; i < numberOfMissingAnswers; i++) {
                answerIndexes.push(-1)
            }

            if (logServerErrorAndReject(sessionError, reject)) return
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

            const { error } = await superbaseSchema
                .from('quizz_session_answer')
                .insert(
                    answerIndexes.map((answerIndex) => ({
                        quizz_session_id: sessionId,
                        question_id: questionId,
                        answer_index: answerIndex,
                    }))
                )

            if (logServerErrorAndReject(error, reject)) return

            if (session.type === 'ucenje') {
                const { data, error } = await superbaseSchema
                    .from('quizz_question')
                    .select('answers')
                    .eq('id', questionId)
                    .single()

                if (logServerErrorAndReject(error, reject)) return

                const correctAnswerIndexes = data?.answers
                    ?.map((answer, index) => (answer.isCorrect ? index : null))
                    .filter((index) => index != null)

                resolve({ correctAnswers: correctAnswerIndexes })
                return
            }

            resolve(null)
        }),
    getCompleted: async (userId) =>
        new Promise(async (resolve, reject) => {
            let query = superbaseSchema
                .from(tableName)
                .select(
                    '*, users(*), quizz_schema(*), quizz_session_answer(*, quizz_question(*))'
                )
                .not('completed_at', 'is', null)
                .order('completed_at', { ascending: false })
                .limit(20)

            if (userId) {
                query = query.eq('created_by', userId)
            }

            const { data, error } = await query

            if (logServerErrorAndReject(error, reject)) return
            const result = data.map((session) => {
                const answers = session.quizz_session_answer.map((a) => {
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
                    id: session.id,
                    created_at: session.created_at,
                    completed_at: session.completed_at,
                    answers,
                    user: session.users.username,
                    quizzSchemaName: session.quizz_schema.name,
                }
            })

            resolve(result)
        }),
}
