import { NextResponse } from 'next/server'
import { superbaseSchema } from '@/superbase'

export async function PUT(request) {
    try {
        const body = await request.json()
        if (!body || !body.id || typeof body.visible !== 'boolean') {
            return NextResponse.json(
                { error: 'Invalid request body' },
                { status: 400 }
            )
        }

        const { data, error } = await superbaseSchema
            .from('quizz_schema')
            .update({ is_active: body.visible })
            .eq('id', body.id)
            .select()

        if (error) {
            console.error('Error updating quiz visibility:', error.message)
            return NextResponse.json(null, { status: 500 })
        }

        return NextResponse.json(null, { status: 200 })
    } catch (error) {
        console.error('Error in PUT handler:', error)
        return NextResponse.json(null, { status: 500 })
    }
}
