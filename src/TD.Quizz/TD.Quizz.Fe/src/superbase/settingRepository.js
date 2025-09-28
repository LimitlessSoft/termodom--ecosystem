import { superbaseSchema } from '@/superbase/index'

export const settingRepository = {
    tableName: 'settings',
    getByKey: async (key) => {
        const { data, error } = await superbaseSchema
            .from(settingRepository.tableName)
            .select('value')
            .eq('key', key)
            .single()
        if (error) throw error
        return data.value
    },
}
