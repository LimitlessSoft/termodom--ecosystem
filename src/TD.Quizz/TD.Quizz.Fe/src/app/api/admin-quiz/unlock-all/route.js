import { quizzSessionRepository } from '@/superbase/quizzSessionRepository'
import { NextResponse } from 'next/server'
import { logServerError } from '@/helpers/errorhelpers'

export async function POST(request) {
    try {
        const body = await request.json()
        if (!body || !body.schemaId) {
            return NextResponse.json(
                { error: 'Invalid request body' },
                { status: 400 }
            )
        }

        await quizzSessionRepository.unlockRatingSessions(body.schemaId)

        return NextResponse.json(null)
    } catch (error) {
        logServerError(error)
        return NextResponse.json(null, { status: 500 })
    }
}
