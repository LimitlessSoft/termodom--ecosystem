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
// URL PREFIXES
// ===============================
export const URL_PREFIXES = {
    IZVESTAJI: '/izvestaji',
    PARTNERI: '/partneri',
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
    PRORACUNI: {
        POST: `/proracuni`,
        GET: `/proracuni`,
    },
}

// ===============================
// PERMISSIONS
// ===============================
export const PERMISSIONS_GROUPS = {
    NALOG_ZA_PREVOZ: 'nalog-za-prevoz',
    NAV_BAR: 'nav-bar',
    PARTNERI: 'partneri',
    PARTNERI_FINANSIJSKO_I_KOMERCIJALNO:
        'partneri-izvestaj-finansijko-komercijalno',
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
    },
    PARTNERI_FINANSIJSKO_I_KOMERCIJALNO: {
        READ: 'PartneriKomercijalnoFinansijskoPoGodinamaRead',
    },
}

export const PRINT_CLASSNAMES = {
    NO_PRINT: 'no-print',
}
