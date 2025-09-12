import settingsRepository from '../settingsRepository'

const quizzQuestionService = {
    async getDefaultDuration() {
        return await settingsRepository.getByKey('DEFAULT_QUESTION_DURATION')
    },
}

export default quizzQuestionService
