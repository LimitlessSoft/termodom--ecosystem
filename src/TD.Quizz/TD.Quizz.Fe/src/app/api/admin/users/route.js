import { userRepository } from '@/superbase/userRepository'
import { NextResponse } from 'next/server'
import { logServerError } from '@/helpers/errorhelpers'

export async function GET(request) {
    try {
        const id = request.nextUrl.searchParams.get('id')
        if (id) return NextResponse.json(await userRepository.getById(id))
        return NextResponse.json(await userRepository.getMultiple())
    } catch {
        return NextResponse.json(null, { status: 500 })
    }
}

export async function POST(request) {
    try {
        const body = await request.json()
        const vErrors = []
        if (!body.username) vErrors.push('Korisničko ime je obavezno')
        if (!body.password) vErrors.push('Lozinka je obavezna')
        if (vErrors.length)
            return NextResponse.json({ error: vErrors }, { status: 400 })
        const newUser = await userRepository.create(
            body.username,
            body.password
        )
        return NextResponse.json(newUser, { status: 201 })
    } catch (error) {
        if (typeof error === 'string')
            return NextResponse.json({ error }, { status: 400 })
        logServerError(error)
        return NextResponse.json(null, { status: 500 })
    }
}
