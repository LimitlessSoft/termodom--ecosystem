import { quizzSessionRepository } from '@/superbase/quizzSessionRepository'
import { NextResponse } from 'next/server'

export async function POST(request) {
    try {
        const body = await request.json()
        if (!body || !body.schemaId) {
            return NextResponse.json(
                { error: 'Invalid request body' },
                { status: 400 }
            )
        }

        await quizzSessionRepository.unlockAll(body.schemaId)
        return NextResponse.json(null, { status: 200 })
    } catch (error) {
        console.error('Error in POST handler:', error)
        return NextResponse.json(null, { status: 500 })
    }
}
