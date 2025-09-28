export const questionHelpers = {
    isCorrectAnswer: (quizzQuestion, answerIndex) => {
        try {
            if (answerIndex === -1) return false
            return quizzQuestion.answers[answerIndex].isCorrect
        } catch (e) {
            console.error(
                `Tried to check if answer ${answerIndex} is correct for question ${quizzQuestion.id}`
            )
            console.error(`Available answers:`, quizzQuestion.answers)
            console.error(e)
            return false
        }
    },
    getStartCountTime: (quizSession) => {
        // if somehow user started a quiz but didn't answer question or he quit half way making last question
        // time too long back, should we go to next question?
        // If not, how will I track time of start given that I shouldn't modify anything?
        // skipping that question would be a way to go.
        // start quizz > quit quizz > come back after 5 min > skip current question as unanswered???
        // or track start time.... but again, he did fetch question and quit it... same sht.... yea, given it is same sht, just skip question as time did run out
        if (quizSession.quizz_session_answer.length === 0)
            return quizSession.created_at

        return quizSession.quizz_session_answer.reduce((max, a) => {
            return a.created_at > max ? a.created_at : max
        }, quizSession.quizz_session_answer[0].created_at)
    },
    didAnswerTimeOut: (startTimeUtc, questionDuration) => {
        const now = new Date()
        const start = new Date(startTimeUtc)
        const elapsedSeconds = (now - start) / 1000
        console.log(
            `Answer timeout: ${elapsedSeconds > questionDuration}.`,
            now,
            start,
            elapsedSeconds
        )
        return elapsedSeconds > questionDuration
    },
}
