import { quizzRepository } from '@/superbase/quizzRepository'
import { NextResponse } from 'next/server'
import { auth } from '@/auth'

const mapQuizz = (quizz) => {
    return {
        id: quizz.id,
        name: quizz.name,
    }
}

export async function GET(_request, { params }) {
    const currentUser = await auth()
    const userId = currentUser.user.id

    const activeQuizzes = (await quizzRepository.getByUserId(userId)).map(
        mapQuizz
    )

    return NextResponse.json(activeQuizzes)
}
