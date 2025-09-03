import { quizzRepository } from '@/superbase/quizzRepository'
import { NextResponse } from 'next/server'
import { userRepository } from '@/superbase/userRepository'

export async function GET(_request, { params }) {
    const { userId } = await params
    if (!userId)
        return NextResponse.json(
            { error: 'userId url param is required' },
            { status: 400 }
        )

    return NextResponse.json(
        await userRepository.getUnassignedQuizzes(userId)
    )
}
