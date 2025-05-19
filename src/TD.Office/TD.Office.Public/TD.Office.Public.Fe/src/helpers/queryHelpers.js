import dayjs from 'dayjs'

const queryHelpers = {
    serialize(params = {}) {
        const result = {}

        for (const key in params) {
            const value = params[key]

            if (value instanceof Date) {
                result[key] = value.toISOString()
                continue
            }

            if (Array.isArray(value)) {
                const r = value.map((item) => {
                    if (item instanceof Date) {
                        return item.toISOString()
                    }

                    if (Array.isArray(item)) {
                        throw new Error('Not implemented yet.') //Not sure if available to add array in array in query
                    }

                    return item?.toString()
                })

                if (r.length != 0) result[key] = r

                continue
            }

            result[key] = value?.toString()
        }

        return result
    },
    parse(query) {
        return Object.fromEntries(
            Object.entries(query).map(([key, value]) => {
                // Check if value is a date string (ISO format)
                if (
                    typeof value === 'string' &&
                    /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?Z?$/.test(
                        value
                    )
                ) {
                    const date = dayjs(value)
                    if (date.isValid()) return [key, date.toDate()]
                }
                return [key, value]
            })
        )
    },
}

export default queryHelpers
