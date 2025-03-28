const settingsHelpers = {
    async createMockSetting(webDbClient, { key, value }) {
        return await webDbClient.settingsRepository.create({
            key,
            value,
        })
    },
}

export default settingsHelpers
