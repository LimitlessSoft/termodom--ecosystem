import { userRepository } from '@/superbase/userRepository'
import { NextResponse } from 'next/server'

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
        if (!body || !body.username || !body.password) {
            return NextResponse.json(
                { error: 'Invalid request body' },
                { status: 400 }
            )
        }
        const newUser = await userRepository.create(
            body.username,
            body.password
        )
        return NextResponse.json(newUser, { status: 201 })
    } catch (error) {
        console.error('Error creating user:', error)
        return NextResponse.json(null, { status: 500 })
    }
}
