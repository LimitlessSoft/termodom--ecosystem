import { quizzRepository } from '@/superbase/quizzRepository'
import usersQuizzRepository from '@/superbase/usersQuizzRepository'
import { NextResponse } from 'next/server'
import { userRepository } from '@/superbase/userRepository'

export async function GET(_request, { params }) {
    const { userId } = await params
    if (!userId)
        return NextResponse.json(
            { error: 'userId url param is required' },
            { status: 400 }
        )

    return NextResponse.json(await userRepository.getAssignedQuizzes(userId))
}

export async function POST(request, { params }) {
    const { userId } = await params
    if (!userId)
        return NextResponse.json(
            { error: 'userId url param is required' },
            { status: 400 }
        )

    const body = await request.json()
    if (!body || body.quizzIds.length === 0) {
        return NextResponse.json(
            { error: 'Morate izabrati bar jedan kviz' },
            { status: 400 }
        )
    }

    return NextResponse.json(
        await userRepository.assignQuizzes(userId, body.quizzIds)
    )
}
