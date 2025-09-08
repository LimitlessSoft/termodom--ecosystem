import { logServerError } from '@/helpers/errorhelpers'
import { userRepository } from '@/superbase/userRepository'
import { NextResponse } from 'next/server'

export async function GET(_request, { params }) {
    try {
        const { userId } = await params
        if (!userId)
            return NextResponse.json(
                { error: 'userId url param is required' },
                { status: 400 }
            )

        const nextUserId = await userRepository.getNextUserId(userId)

        if (!nextUserId) {
            return NextResponse.json(
                { error: 'Ne postoji sledeći korisnik' },
                { status: 404 }
            )
        }

        return NextResponse.json(nextUserId)
    } catch (err) {
        logServerError(err)
        return NextResponse.json(null, { status: 500 })
    }
}
