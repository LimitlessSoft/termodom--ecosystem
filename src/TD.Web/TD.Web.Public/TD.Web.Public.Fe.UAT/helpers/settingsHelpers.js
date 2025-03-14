const settingsHelpers = {
    async createMockSetting(webDbClient, { key, value }) {
        return await webDbClient.settingsRepository.create({
            key,
            value,
        })
    },
    async hardDeleteMockSetting(webDbClient, id) {
        await webDbClient.settingsRepository.hardDelete(id)
    },
}

export default settingsHelpers
