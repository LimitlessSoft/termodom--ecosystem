export const UIDimensions = {
    maxWidth: `1100px`,
}

// ===============================
// COOKIES
// ===============================
export const COOKIES = {
    TOKEN: {
        NAME: 'token',
        DEFAULT_VALUE: undefined,
    },
}

export const USER_FILTERS = {
    TYPES: 'Tipovi korisnika',
    STATUSES: 'Statusi korisnika',
    PROFESSIONS: 'Profesija korisnika',
    STORES: 'Radnje korisnika',
    CITIES: 'Gradovi korisnika',
}

export const USER_STATUSES = {
    ACTIVE: 'Aktivan',
    INACTIVE: 'Neaktivan',
}

// ===============================
// PERMISSIONS
// ===============================
export const PERMISSIONS_GROUPS = {
    NAV_BAR: 'nav-bar',
    PRODUCTS: 'products',
}

export const USER_PERMISSIONS = {
    PROIZVODI: {
        READ: 'Admin_Products_Access',
        EDIT_ALL: 'Admin_Products_EditAll',
        EDIT_SRC: 'Admin_Products_EditSrc',
        EDIT_META_TAGS: 'Admin_Products_EditMetaTags',
    },
    PORUDZBINE: { READ: 'Admin_Orders_Access' },
    KORISNICI: { READ: 'Admin_Users_Access' },
    PODESAVANJA: { READ: 'Admin_Settings_Access' },
}
