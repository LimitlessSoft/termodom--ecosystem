import { quizzRepository } from '@/superbase/quizzRepository'
import { quizzSessionRepository } from '@/superbase/quizzSessionRepository'

const mapQuizz = (quizz) => {
    return {
        id: quizz.id,
        name: quizz.name,
    }
}

export async function GET(request) {
    const { searchParams } = new URL(request.url)
    const sessionId = searchParams.get('sessionId')
    if (!sessionId)
        return Response.json((await quizzRepository.getActive()).map(mapQuizz))

    // fetch either next quesiton or quizz result
    try {
        const quizz = await quizzSessionRepository.getById(sessionId)
        if (quizz.completed_at) return Response.json(quizz)
        try {
            return Response.json(
                await quizzSessionRepository.getNextQuestion(sessionId)
            )
        } catch (err) {
            if (err === 'completed')
                return Response.json(
                    await quizzSessionRepository.getById(sessionId)
                )
            return Response.json(null, { status: 500 })
        }
    } catch (error) {
        if (error === 404) return Response.json(null, { status: 404 })
        return Response.json(null, { status: 500 })
    }
}

export async function POST(request) {
    const body = await request.json()
    if (!body.sessionId || !body.questionId || body.answerIndex === undefined)
        return Response.json(
            { error: 'sessionId, answerIndex and questionId are required' },
            { status: 400 }
        )
    const { sessionId, questionId, answerIndex } = body
    try {
        const result = await quizzSessionRepository.setAnswer(
            sessionId,
            questionId,
            parseInt(answerIndex)
        )
        return Response.json(result)
    } catch (error) {
        if (error === 'completed')
            return Response.json(
                await quizzSessionRepository.getById(sessionId)
            )
        if (error instanceof Error) return Response.json(null, { status: 500 })
        return Response.json(error, { status: 400 })
    }
}
