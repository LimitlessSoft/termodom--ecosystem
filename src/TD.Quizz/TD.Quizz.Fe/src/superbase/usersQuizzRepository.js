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
    async getMultipleByQuizzId(quizzId) {
        const { data, error } = await superbaseSchema
            .from(this.tableName)
            .select('user_id')
            .eq('quizz_schema_id', quizzId)

        if (error) throw error

        return data
    },
}

export default usersQuizzRepository
