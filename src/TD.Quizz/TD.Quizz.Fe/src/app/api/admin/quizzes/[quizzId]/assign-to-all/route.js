import { quizzRepository } from '@/superbase/quizzRepository'
import { NextResponse } from 'next/server'

export async function POST(_request, { params }) {
    try {
        const { quizzId } = await params
        if (!quizzId)
            return NextResponse.json(
                { error: 'quizzId url param is required' },
                { status: 400 }
            )

        return NextResponse.json(
            await quizzRepository.assignToAllUsers(quizzId)
        )
    } catch {
        return NextResponse.json(null, { status: 500 })
    }
}
