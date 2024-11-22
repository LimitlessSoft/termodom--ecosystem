export const PERMISSIONS_CONSTANTS = {
    PERMISSIONS_GROUPS: {
        NALOG_ZA_PREVOZ: 'nalog-za-prevoz',
        NAV_BAR: 'nav-bar',
        PARTNERI: 'partneri',
        PARTNERI_FINANSIJSKO_I_KOMERCIJALNO:
            'partneri-izvestaj-finansijko-komercijalno',
        IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA:
            'izvestaj-ukupne-kolicine-po-robi-u-filtriranim-dokumentima',
        PRORACUNI: 'proracuni',
        SPECIFIKACIJA_NOVCA: 'specifikacija-novca',
    },
    USER_PERMISSIONS: {
        NALOG_ZA_PREVOZ: {
            READ: 'NalogZaPrevozRead',
            NEW: 'NalogZaPrevozNovi',
            REPORT_PRINT: 'NalogZaPrevozStampaIzvestaja',
            INDIVIDUAL_ORDER_PRINT: 'NalogZaPrevozStampaPojedinacnogNaloga',
            ALL_WAREHOUSES: 'NalogZaPrevozRadSaSvimMagacinima',
            PREVIOUS_DATES: 'NalogZaPrevozPrethodniDatumi',
        },
        SPECIFIKACIJA_NOVCA: {
            READ: 'SpecifikacijaNovcaRead',
            ALL_WAREHOUSES: 'SpecifikacijaNovcaSviMagacini',
            PREVIOUS_WEEK: 'SpecifikacijaNovcaPrethodnih7Dana',
            ALL_DATES: 'SpecifikacijaNovcaSviDatumi',
            SEARCH_BY_NUMBER: 'SpecifikacijaNovcaPretragaPoBroju',
            SAVE: 'SpecifikacijaNovcaSave',
            PRINT: 'SpecifikacijaNovcaPrint',
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
            OLDER_THAN_SEVEN_DAYS: 'ProracuniReadStarijiOd7Dana',
            EDIT_RABAT: 'ProracuniRabat',
            CREATE_NALOG_ZA_UTOVAR: 'ProracuniNewNalogZaUtovar',
        },
        PARTNERI_FINANSIJSKO_I_KOMERCIJALNO: {
            READ: 'PartneriKomercijalnoFinansijskoPoGodinamaRead',
        },
    },
}
