import { superbaseSchema } from '.'

const settingsRepository = {
    tableName: 'settings',
    async getByKey(key) {
        const { data, error } = await superbaseSchema
            .from(this.tableName)
            .select('value')
            .eq('key', key)
            .single()

        if (error) throw error

        return data.value
    },
}

export default settingsRepository
