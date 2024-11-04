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
        GET: (id: number) => `/partners/${id}`,
        GET_MULTIPLE: `/partners`,
        GET_MESTA: `/partners-mesta`,
        GET_KATEGORIJE: `/partners-kategorije`,
        POST: `/partners`,
        RECENTLY_CREATED: '/partners-recently-created',
    },
    PRORACUNI: {
        POST: `/proracuni`,
        GET_MULTIPLE: `/proracuni`,
        GET: (id: number) => `/proracuni/${id}`,
        STATE: (id: number) => `/proracuni/${id}/state`,
        PPID: (id: number) => `/proracuni/${id}/ppid`,
        NUID: (id: number) => `/proracuni/${id}/nuid`,
        POST_ITEM: (id: number) => `/proracuni/${id}/items`,
        DELETE_ITEM: (proracunId: number, itemId: number) =>
            `/proracuni/${proracunId}/items/${itemId}`,
        FORWARD_TO_KOMERCIJALNO: (id: number) =>
            `/proracuni/${id}/forward-to-komercijalno`,
    },
}

// ===============================
// PERMISSIONS
// ===============================
export const PERMISSIONS_GROUPS = {
    NALOG_ZA_PREVOZ: 'nalog-za-prevoz',
    NAV_BAR: 'nav-bar',
    PARTNERI: 'partneri',
    IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA:
        'izvestaj-ukupne-kolicine-po-robi-u-filtriranim-dokumentima',
    PRORACUNI: 'proracuni',
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
    PRORACUNI: {
        READ: 'ProracuniRead',
        CREATE_MP: 'ProracuniNewMp',
        CREATE_VP: 'ProracuniNewVp',
        RAD_SA_SVIM_MAGACINIMA: 'ProracuniReadSviMagacini',
        LOCK: 'ProracuniLock',
        UNLOCK: 'ProracuniUnlock',
    },
}

export const PRINT_CLASSNAMES = {
    NO_PRINT: 'no-print',
}
