export const FORMAT_NUMBER_DECIMAL_COUNT = 2

// ===============================
// ENDPOINTS
// ===============================
export const ENDPOINTS = {
    LOGIN: `/logovanje`
}


// ===============================
// PERMISSIONS
// ===============================
export const PERMISSIONS_GROUPS = {
    NALOG_ZA_PREVOZ: 'nalog-za-prevoz',
    NAV_BAR: 'nav-bar'
}

export const USER_PERMISSIONS = {
    NALOG_ZA_PREVOZ: {
        READ: 'NalogZaPrevozRead',
        NEW: 'NalogZaPrevozNovi',
        REPORT_PRINT: 'NalogZaPrevozStampaIzvestaja',
        INDIVIDUAL_ORDER_PRINT: 'NalogZaPrevozStampaPojedinacnogNaloga',
        ALL_WAREHOUSES: 'NalogZaPrevozRadSaSvimMagacinima',
        PREVIOUS_DATES: 'NalogZaPrevozPrethodniDatumi'
    }
}

