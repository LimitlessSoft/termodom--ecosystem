import { logServerError } from '@/helpers/errorhelpers'
import userService from '@/services/userService'
import { userRepository } from '@/superbase/userRepository'
import { NextResponse } from 'next/server'

export async function GET(_request, { params }) {
    try {
        const { userId } = await params
        if (!userId)
            return Response.json(
                { error: 'UserId url param is required' },
                { status: 400 }
            )

        return NextResponse.json(await userService.getUser(userId))
    } catch (err) {
        logServerError(err)
        return NextResponse.json(null, { status: 500 })
    }
}

export async function PUT(request, { params }) {
    try {
        const { userId } = await params
        if (!userId)
            return Response.json(
                { error: 'userId url param is required' },
                { status: 400 }
            )

        const body = await request.json()
        if (!body || !body.username) {
            return Response.json(
                { error: 'username is required' },
                { status: 400 }
            )
        }

        return NextResponse.json(
            await userRepository.updateById(userId, {
                username: body.username,
                password: body.password,
            })
        )
    } catch {
        return NextResponse.json(null, { status: 500 })
    }
}
