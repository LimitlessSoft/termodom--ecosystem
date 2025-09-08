import { userRepository } from '../userRepository'

const userService = {
    async getUser(userId) {
        const user = await userRepository.getById(userId)
        const prev = await userRepository.getPrevUserId(userId)
        const next = await userRepository.getNextUserId(userId)

        return { ...user, prevId: prev?.id ?? null, nextId: next?.id ?? null }
    },
}

export default userService
