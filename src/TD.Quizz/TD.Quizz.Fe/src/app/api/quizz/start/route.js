import { quizzSessionRepository } from '@/superbase/quizzSessionRepository'

export async function GET(request) {
    try {
        const type = request.nextUrl.searchParams.get('type')
        const schemaId = request.nextUrl.searchParams.get('schemaId')
        if (!type)
            return Response.json(
                { error: 'Type parameter is required' },
                { status: 400 }
            )
        if (!schemaId)
            return Response.json(
                { error: 'SchemaId parameter is required' },
                { status: 400 }
            )
        return Response.json(
            await quizzSessionRepository.getCurrent(schemaId, type)
        )
    } catch (error) {
        console.error('Error fetching current quizz session:', error)
        if (error instanceof Error) return Response.json(null, { status: 500 })
        return Response.json({ error: error }, { status: 400 })
    }
}
