import { logServerError } from '@/helpers/errorhelpers'
import quizzQuestionService from '@/superbase/services/quizzQuestionService'
import { NextResponse } from 'next/server'

export async function GET() {
    try {
        return NextResponse.json(
            await quizzQuestionService.getDefaultDuration()
        )
    } catch (err) {
        logServerError(err)
        return NextResponse.json(null, { status: 500 })
    }
}
