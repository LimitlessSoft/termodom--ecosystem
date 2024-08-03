export const FILTER_KEYS = {
    CITIES: 'filteredCities',
    PROFESSIONS: 'filteredProfessions',
    STATUSES: 'filteredStatuses',
    STORES: 'filteredStores',
    TYPES: 'filteredTypes',
    SEARCH: 'search',
} as const

export const USER_FILTERS = {
    TYPES: 'Tip',
    STATUSES: 'Status',
    PROFESSIONS: 'Profesija',
    STORES: 'Radnja',
    CITIES: 'Grad',
}

export const USER_STATUSES = {
    ACTIVE: 'Aktivan',
    INACTIVE: 'Neaktivan',
}

export const FILTER_CONFIG = [
    {
        label: USER_FILTERS.TYPES,
        key: FILTER_KEYS.TYPES,
        dataKey: 'userTypes',
    },
    {
        label: USER_FILTERS.STATUSES,
        key: FILTER_KEYS.STATUSES,
        dataKey: 'statuses',
    },
    {
        label: USER_FILTERS.PROFESSIONS,
        key: FILTER_KEYS.PROFESSIONS,
        dataKey: 'professions',
    },
    {
        label: USER_FILTERS.STORES,
        key: FILTER_KEYS.STORES,
        dataKey: 'stores',
    },
    {
        label: USER_FILTERS.CITIES,
        key: FILTER_KEYS.CITIES,
        dataKey: 'cities',
    },
]
