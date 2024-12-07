export const moduleHelpsConstants = {
    getByUrl: (url) => {
        for (const key in moduleHelpsConstants) {
            if (moduleHelpsConstants[key].url === url) {
                return moduleHelpsConstants[key]
            }
        }
        return null
    },
    PROIZVODI_LIST: {
        id: 0,
        url: '/proizvodi',
    },
    PORUDZBINE_LIST: {
        id: 1,
        url: '/porudzbine',
    },
    KORISNICI_LIST: {
        id: 2,
        url: '/korisnici',
    },
}
