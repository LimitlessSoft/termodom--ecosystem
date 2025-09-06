import { quizzRepository } from '@/superbase/quizzRepository'
import { NextResponse } from 'next/server'
import { logServerError } from '@/helpers/errorhelpers'

export async function GET(request) {
    try {
        const id = request.nextUrl.searchParams.get('id')
        if (id) return NextResponse.json(await quizzRepository.getById(id))
        return NextResponse.json(await quizzRepository.getMultiple())
    } catch {
        return NextResponse.json(null, { status: 500 })
    }
}

export async function POST(request) {
    try {
        const body = await request.json()
        if (!body || !body.name) {
            return NextResponse.json(
                { error: 'Invalid request body' },
                { status: 400 }
            )
        }
        const newQuiz = await quizzRepository.create(body.name)
        return NextResponse.json(newQuiz, { status: 201 })
    } catch (error) {
        logServerError(error)
        return NextResponse.json(null, { status: 500 })
    }
}

export async function PUT(request) {
    try {
        const body = await request.json()
        if (!body || !body.id || !body.name || !body.questions) {
            return NextResponse.json(
                { error: 'Invalid request body' },
                { status: 400 }
            )
        }
        const updatedQuiz = await quizzRepository.update(body)
        return NextResponse.json(updatedQuiz, { status: 200 })
    } catch (error) {
        logServerError(error)
        return NextResponse.json(null, { status: 500 })
    }
}
