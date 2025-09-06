import { quizzRepository } from '@/superbase/quizzRepository'
import { NextResponse } from 'next/server'

export async function POST(_request, { params }) {
    const { quizzId } = await params
    if (!quizzId)
        return NextResponse.json(
            { error: 'quizzId url param is required' },
            { status: 400 }
        )

    return NextResponse.json(await quizzRepository.assignToAllUsers(quizzId))
}
