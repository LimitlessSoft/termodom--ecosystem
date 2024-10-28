export const ENDPOINTS_CONSTANTS = {
    LOGIN: `/logovanje`,
    STORES: {
        GET_MULTIPLE: `/stores`,
        GET: (id) => `/stores/${id}`,
        put: `/stores`,
    },
    PARTNERS: {
        GET_MULTIPLE: `/partners`,
        GET_MESTA: `/partners-mesta`,
        GET_KATEGORIJE: `/partners-kategorije`,
        POST: `/partners`,
        RECENTLY_CREATED: '/partners-recently-created',
        GET_KOMERCIJALNO_I_FINANSIJSKO: `/partneri-po-godinama-komercijalno-finansijsko`,
    },
    PRORACUNI: {
        POST: `/proracuni`,
        GET: `/proracuni`,
    },
}
