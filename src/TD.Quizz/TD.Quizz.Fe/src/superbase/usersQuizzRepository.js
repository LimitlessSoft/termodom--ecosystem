import { superbaseSchema } from '@/superbase/index'
import { logServerErrorAndReject } from '@/helpers/errorhelpers'

const usersQuizzRepository = {
    tableName: 'users_quizz_schemas',
    hardDelete: async (userId, quizzId) =>
        new Promise(async (resolve, reject) => {
            const { error } = await superbaseSchema
                .from(usersQuizzRepository.tableName)
                .delete()
                .eq('user_id', userId)
                .eq('quizz_schema_id', quizzId)

            if (logServerErrorAndReject(error, reject)) return

            resolve(null)
        }),
}

export default usersQuizzRepository
