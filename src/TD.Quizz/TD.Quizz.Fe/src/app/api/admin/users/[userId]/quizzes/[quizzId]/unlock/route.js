import { quizzSessionRepository } from '@/superbase/quizzSessionRepository'
import { NextResponse } from 'next/server'

export async function POST(_request, { params }) {
    try {
        const { userId, quizzId } = await params
        if (!userId || !quizzId)
            return Response.json(
                { error: 'UserId and QuizzId url params are required' },
                { status: 400 }
            )

        return NextResponse.json(
            await quizzSessionRepository.unlockRatingSessions(quizzId, userId)
        )
    } catch (err) {
        console.log(err)
        return NextResponse.json(null, { status: 500 })
    }
}
