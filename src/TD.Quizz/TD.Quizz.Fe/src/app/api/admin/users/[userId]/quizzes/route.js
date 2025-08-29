import { quizzRepository } from '@/superbase/quizzRepository'
import { NextResponse } from 'next/server'

export async function GET(_request, { params }) {
    try {
        const { userId } = await params
        if (!userId)
            return Response.json(
                { error: 'UserId url param is required' },
                { status: 400 }
            )

        return NextResponse.json(
            await quizzRepository.getActiveByUserId(userId)
        )
    } catch {
        return NextResponse.json(null, { status: 500 })
    }
}
