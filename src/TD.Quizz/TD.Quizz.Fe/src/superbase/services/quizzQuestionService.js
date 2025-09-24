import settingsRepository from '../settingsRepository'

const quizzQuestionService = {
    async getDefaultDuration() {
        const value = await settingsRepository.getByKey(
            'DEFAULT_QUESTION_DURATION'
        )

        if (!value) {
            throw new Error('Default duration is missing in the database')
        }

        return +value
    },
}

export default quizzQuestionService
