export const ENDPOINTS_CONSTANTS = {
    LOGIN: `/logovanje`,
    STORES: {
        GET_MULTIPLE: `/stores`,
        GET: (id) => `/stores/${id}`,
        put: `/stores`,
    },
    IZVESTAJI: {
        GET_IZVESTAJ_IZLAZA_ROBE_PO_GODINAMA: `/izvestaj-izlaza-roba-po-godinama`,
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
        GET: (id) => `/proracuni/${id}`,
        STATE: (id) => `/proracuni/${id}/state`,
        PPID: (id) => `/proracuni/${id}/ppid`,
        NUID: (id) => `/proracuni/${id}/nuid`,
        POST_ITEM: (id) => `/proracuni/${id}/items`,
        DELETE_ITEM: (proracunId, itemId) =>
            `/proracuni/${proracunId}/items/${itemId}`,
        FORWARD_TO_KOMERCIJALNO: (id) =>
            `/proracuni/${id}/forward-to-komercijalno`,
    },
    USERS: {
        UPDATE_NICKNAME: (id) => `/users/${id}/nickname`,
        UPDATE_MAX_RABAT_MP_DOKUMENTI: (id) =>
            `/users/${id}/max-rabat-mp-dokumenti`,
        UPDATE_MAX_RABAT_VP_DOKUMENTI: (id) =>
            `/users/${id}/max-rabat-vp-dokumenti`,
    },
}
