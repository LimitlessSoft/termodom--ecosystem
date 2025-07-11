const moduleHelpConstants = {
    getByUrl: (url) => {
        for (const key in moduleHelpConstants) {
            if (moduleHelpConstants[key].url === url) {
                return moduleHelpConstants[key]
            }
        }
        return null
    },
    NOVAC_PREGLED_I_UPLATA_PAZARA: {
        id: 0,
        url: '/novac/pregled-i-uplata-pazara',
    },
}

export default moduleHelpConstants
