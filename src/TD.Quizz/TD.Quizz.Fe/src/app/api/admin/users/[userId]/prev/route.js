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

        const prevUserId = await userRepository.getPrevUserId(userId)

        if (!prevUserId) {
            return NextResponse.json(
                { error: 'Ne postoji prethodni korisnik' },
                { status: 404 }
            )
        }

        return NextResponse.json(prevUserId)
    } catch (err) {
        logServerError(err)
        return NextResponse.json(null, { status: 500 })
    }
}
