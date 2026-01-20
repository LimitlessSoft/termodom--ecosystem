export const ENDPOINTS_CONSTANTS = {
    LOGIN: `/logovanje`,
    PREGLED_I_UPLATA_PAZARA: {
        GET_MULTIPLE: `/pregled-i-uplata-pazara`,
        GET_NEISPRAVO_UNETE_STAVKE_IZVODA:
            '/pregled-i-uplata-pazara/neispravne-stavke-izvoda',
    },
    STORES: {
        GET_MULTIPLE: `/stores`,
        GET: (id) => `/stores/${id}`,
        put: `/stores`,
    },
    IZVESTAJI: {
        GET_IZVESTAJ_IZLAZA_ROBE_PO_GODINAMA: `/izvestaj-izlaza-roba-po-godinama`,
        GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA_COUNT: `/izvestaj-neispravnih-cena-u-magacinima-count`,
        GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA:
            '/izvestaj-neispravnih-cena-u-magacinima',
        OSVEZI_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA:
            '/osvezi-izvestaj-neispravnih-cena-u-magacinima',
        GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA_STATUS:
            'izvjestaj-neispravnih-cena-u-magacinima-status',
        GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA_LAST_RUN: `izvestaj-neispravnih-cena-u-magacinima-last-run`,
    },
    NOTES: {
        GET: (id) => `/notes/${id}`,
        GET_INITIAL: `/notes`,
        DELETE: (id) => `/notes/${id}`,
        PUT: '/notes',
        PUT_NAME: (id) => `/notes/${id}/name`,
    },
    PARTNERS: {
        GET_MULTIPLE: `/partners`,
        GET: (id) => `/partners/${id}`,
        GET_MESTA: `/partners-mesta`,
        GET_KATEGORIJE: `/partners-kategorije`,
        POST: `/partners`,
        GET_ANALIZA: (id) => `/partneri-analiza/${id}`,
        RECENTLY_CREATED: '/partners-recently-created',
        GET_KOMERCIJALNO_I_FINANSIJSKO: `/partneri-po-godinama-komercijalno-finansijsko`,
        PUT_KOMERCIJALNO_I_FINANSIJSKO_DATA_KOMENTAR: (id) =>
            `/partneri-po-godinama-komercijalno-finansijsko-data/${id}/komentar`,
        PUT_KOMERCIJALNO_I_FINANSIJSKO_DATA_STATUS: (id) =>
            `/partneri-po-godinama-komercijalno-finansijsko-data/${id}/status`,
        GET_KOMERCIJALNO_I_FINANSIJSKO_DATA: `/partneri-po-godinama-komercijalno-finansijsko-data`,
    },
    PRORACUNI: {
        POST: `/proracuni`,
        GET_MULTIPLE: `/proracuni`,
        RECOMMENDED_VALUE: {
            PUT: (id) => `/proracuni/${id}/recommended-value`,
        },
        GET: (id) => `/proracuni/${id}`,
        STATE: (id) => `/proracuni/${id}/state`,
        PPID: (id) => `/proracuni/${id}/ppid`,
        NUID: (id) => `/proracuni/${id}/nuid`,
        EMAIL: {
            PUT: (id) => `/proracuni/${id}/email`,
        },
        POST_ITEM: (id) => `/proracuni/${id}/items`,
        DELETE_ITEM: (proracunId, itemId) =>
            `/proracuni/${proracunId}/items/${itemId}`,
        FORWARD_TO_KOMERCIJALNO: (id) =>
            `/proracuni/${id}/forward-to-komercijalno`,
    },
    USERS: {
        GET_MULTIPLE: (pageSize = 100) => `/users?pageSize=${pageSize}`,
        UPDATE_NICKNAME: (id) => `/users/${id}/nickname`,
        UPDATE_MAX_RABAT_MP_DOKUMENTI: (id) =>
            `/users/${id}/max-rabat-mp-dokumenti`,
        UPDATE_MAX_RABAT_VP_DOKUMENTI: (id) =>
            `/users/${id}/max-rabat-vp-dokumenti`,
        UPDATE_STORE_ID: (id) => `/users/${id}/store-id`,
        UPDATE_VP_STORE_ID: (id) => `/users/${id}/vp-magacin-id`,
        UPDATE_TIP_KORISNIKA_ID: (id) => `/users/${id}/tip-korisnika-id`,
    },
    MASS_SMS: {
        QUEUE_COUNT: `/mass-sms/queue-count`,
        STATUS: `/mass-sms/status`,
        QUEUE: `/mass-sms/queue`,
        CLEAR_QUEUE: `/mass-sms/clear-queue`,
        PREPARE_NUMBERS_FROM_PUBLIC_WEB:
            '/mass-sms/prepare-phone-numbers-from-public-web',
        PREPARE_NUMBERS_FROM_KOMERCIJALNO:
            '/mass-sms/prepare-phone-numbers-from-komercijalno',
        CLEAR_DUPLICATES: '/mass-sms/clear-duplicates',
        SET_TEXT: '/mass-sms/text',
        SEND: '/mass-sms/invoke-sending',
        CLEAR_BLACKLISTED: '/mass-sms/clear-blacklisted',
        IS_BLACKLISTED: (number) => `/mass-sms/${number}/is-blacklisted`,
        ADD_TO_BLACKLIST: (number) => `/mass-sms/${number}/blacklist`,
    },
    OTPREMNICE: {
        GET_MULTIPLE: `/interne-otpremnice`,
        POST: `/interne-otpremnice`,
        GET: (id) => `/interne-otpremnice/${id}`,
        SAVE_ITEM: (id) => `/interne-otpremnice/${id}/items`,
        DELETE_ITEM: (otpremnicaId, itemId) =>
            `/interne-otpremnice/${otpremnicaId}/items/${itemId}`,
        STATE: (id, state) => `/interne-otpremnice/${id}/state/${state}`,
        FORWARD_TO_KOMERCIJALNO: (id) =>
            `/interne-otpremnice/${id}/forward-to-komercijalno`,
    },
    SPECIFIKACIJA_NOVCA: {
        GET_DEFAULT: '/specifikacija-novca',
        GET: (id) => `/specifikacija-novca/${id}`,
        NEXT: (id, fixMagacin) =>
            `/specifikacija-novca-next?relativeToId=${id}&fixMagacin=${fixMagacin}`,
        PREVIOUS: (id, fixMagacin) =>
            `/specifikacija-novca-prev?relativeToId=${id}&fixMagacin=${fixMagacin}`,
        GET_BY_DATE: `/specifikacija-novca-date`,
        SAVE: (id) => `/specifikacija-novca/${id}`,
    },
    MODULES_HELPS: {
        GET: (moduleType) => `/module-helps?module=${moduleType}`,
        PUT: `/module-helps`,
    },
    POPISI: {
        POST_ITEM: (id) => `/popisi/${id}/items`,
        DELETE_ITEM: (popisId, itemId) => `/popisi/${popisId}/items/${itemId}`,
    },
    ODSUSTVO: {
        CALENDAR: (month, year, userId) => {
            let url = `/odsustvo/calendar?month=${month}&year=${year}`
            if (userId) url += `&userId=${userId}`
            return url
        },
        YEAR_LIST: (year, userId) => `/odsustvo/year-list?year=${year}&userId=${userId}`,
        PENDING: '/odsustvo/pending',
        GET: (id) => `/odsustvo/${id}`,
        SAVE: '/odsustvo',
        DELETE: (id) => `/odsustvo/${id}`,
        APPROVE: (id) => `/odsustvo/${id}/approve`,
        REALIZOVANO: (id) => `/odsustvo/${id}/realizovano`,
    },
    TIP_ODSUSTVA: {
        GET_MULTIPLE: '/tip-odsustva',
        SAVE: '/tip-odsustva',
        DELETE: (id) => `/tip-odsustva/${id}`,
    },
    TIP_KORISNIKA: {
        GET_MULTIPLE: '/tip-korisnika',
        SAVE: '/tip-korisnika',
        DELETE: (id) => `/tip-korisnika/${id}`,
    },
}
