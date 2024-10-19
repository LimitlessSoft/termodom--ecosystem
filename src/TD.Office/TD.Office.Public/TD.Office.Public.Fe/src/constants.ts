export const FORMAT_NUMBER_DECIMAL_COUNT = 2

// ===============================
// COOKIES
// ===============================
export const COOKIES = {
    TOKEN: {
        NAME: 'token',
        DEFAULT_VALUE: undefined,
    },
}

// ===============================
// ENDPOINTS
// ===============================
export const ENDPOINTS = {
    LOGIN: `/logovanje`,
    STORES: {
        GET_MULTIPLE: `/stores`,
        GET: (id: number) => `/stores/${id}`,
        put: `/stores`,
    },
    PARTNERS: {
        GET_MULTIPLE: `/partners`,
        GET_MESTA: `/partners-mesta`,
        GET_KATEGORIJE: `/partners-kategorije`,
        POST: `/partners`,
        RECENTLY_CREATED: '/partners-recently-created',
    },
}

// ===============================
// PERMISSIONS
// ===============================
export const PERMISSIONS_GROUPS = {
    NALOG_ZA_PREVOZ: 'nalog-za-prevoz',
    NAV_BAR: 'nav-bar',
    PARTNERI: 'partneri',
}

export const USER_PERMISSIONS = {
    NALOG_ZA_PREVOZ: {
        READ: 'NalogZaPrevozRead',
        NEW: 'NalogZaPrevozNovi',
        REPORT_PRINT: 'NalogZaPrevozStampaIzvestaja',
        INDIVIDUAL_ORDER_PRINT: 'NalogZaPrevozStampaPojedinacnogNaloga',
        ALL_WAREHOUSES: 'NalogZaPrevozRadSaSvimMagacinima',
        PREVIOUS_DATES: 'NalogZaPrevozPrethodniDatumi',
    },
    WEB_SHOP: {
        READ: 'WebRead',
    },
    KORISNICI: {
        READ: 'KorisniciRead',
    },
    PARTNERI: {
        READ: 'PartneriRead',
        VIDI_MOBILNI: 'PartneriVidiMobilni',
        SKORO_KREIRANI: 'PartneriSkoroKreirani',
    },
    IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA: {
        READ: 'IzvestajUkupneKolicinePoRobiUFiltriranimDokumentimaRead',
    },
}

export const PRINT_CLASSNAMES = {
    NO_PRINT: 'no-print',
}
