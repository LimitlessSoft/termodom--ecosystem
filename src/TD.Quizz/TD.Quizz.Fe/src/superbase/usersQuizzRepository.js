import { superbaseSchema } from '@/superbase/index'

const usersQuizzRepository = {
    tableName: 'users_quizz_schemas',
    hardDelete: async (userId, quizzId) =>
        new Promise(async (resolve, reject) => {
            const { error } = await superbaseSchema
                .from(usersQuizzRepository.tableName)
                .delete()
                .eq('user_id', userId)
                .eq('quizz_schema_id', quizzId)

            if (error) {
                const msg = 'Failed to delete quizz for user: ' + error.message
                console.error(msg)
                reject(new Error())
            }

            resolve(null)
        }),
}

export default usersQuizzRepository
