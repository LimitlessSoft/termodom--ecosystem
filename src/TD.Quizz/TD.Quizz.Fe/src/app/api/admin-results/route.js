import { quizzSessionRepository } from '@/superbase/quizzSessionRepository'
import { NextResponse } from 'next/server'

export async function GET(request) {
    try {
        return NextResponse.json(await quizzSessionRepository.getCompleted())
    } catch (error) {
        console.error(`Failed to fetch admin results: ${error.message}`)
        return NextResponse.json(null, {
            status: 500,
        })
    }
}
