import dayjs from 'dayjs'

const queryHelpers = {
    normalizeParams(params = {}) {
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
    parseQuery(query = {}, defaultValues = {}) {
        return Object.fromEntries(
            Object.entries(defaultValues).map(([key, defaultValue]) => {
                const value = query[key]

                if (value == null) return [key, defaultValue]

                if (defaultValue instanceof Date) {
                    return [key, dayjs(value).toDate() || defaultValue]
                }

                if (typeof defaultValue === 'number') {
                    const parsedNumber = parseFloat(value)
                    return [
                        key,
                        isNaN(parsedNumber) ? defaultValue : parsedNumber,
                    ]
                }

                return [key, value]
            })
        )
    },
}

export default queryHelpers
