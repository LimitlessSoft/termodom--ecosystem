import usersQuizzRepository from '@/superbase/usersQuizzRepository'
import { NextResponse } from 'next/server'

export async function DELETE(_request, { params }) {
    const { userId, quizzId } = await params
    if (!userId || !quizzId)
        return NextResponse.json(
            { error: 'userId and quizzId url param is required' },
            { status: 400 }
        )

    return NextResponse.json(await usersQuizzRepository.hardDelete(userId, quizzId))
}
