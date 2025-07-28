'use server'
import { userRepository } from '@/superbase/userRepository'

export async function loginAction(username, password) {
    try {
        return { data: await userRepository.login(username, password), status: 200 }
    } catch (error) {
        if (error instanceof Error)
            return { status: 500 }
        return { error: error, status: 400 }
    }
}