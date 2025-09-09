import { quizzSessionRepository } from '@/superbase/quizzSessionRepository'
import { NextResponse } from 'next/server'
import { logServerError } from '@/helpers/errorhelpers'

export async function GET(request) {
    try {
        return NextResponse.json(await quizzSessionRepository.getCompleted())
    } catch (error) {
        logServerError(error)
        return NextResponse.json(null, {
            status: 500,
        })
    }
}
