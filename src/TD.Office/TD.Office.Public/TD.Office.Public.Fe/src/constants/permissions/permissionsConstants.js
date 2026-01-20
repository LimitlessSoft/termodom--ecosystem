export const PERMISSIONS_CONSTANTS = {
    PERMISSIONS_GROUPS: {
        DASHBOARD: 'dashboard',
        NALOG_ZA_PREVOZ: 'nalog-za-prevoz',
        NAV_BAR: 'nav-bar',
        PARTNERI: 'partneri',
        PARTNERI_FINANSIJSKO_I_KOMERCIJALNO:
            'partneri-izvestaj-finansijko-komercijalno',
        IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA:
            'izvestaj-ukupne-kolicine-po-robi-u-filtriranim-dokumentima',
        PRORACUNI: 'proracuni',
        SPECIFIKACIJA_NOVCA: 'specifikacija-novca',
        IZVESTAJ_IZLAZA_ROBE_PO_GODINAMA: 'izvestaj-izlaza-robe-po-godinama',
        PARTNERI_ANALIZA: 'partneri-analiza',
        OTPREMNICE: 'otpremnice',
        ROBA_POPIS: 'roba-popis',
        MASOVNI_SMS: 'masovni-sms',
        KALENDAR_AKTIVNOSTI: 'kalendar-aktivnosti',
        TIP_ODSUSTVA: 'tip-odsustva',
        KORISNICI_LIST: 'korisnici-lista',
        TIP_KORISNIKA: 'tip-korisnika',
        MODULE_HELP: 'module-help',
    },
    USER_PERMISSIONS: {
        DASHBOARD: {
            READ: 'DashboardRead',
        },
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
            LIST_READ: 'KorisniciListRead',
        },
        PARTNERI: {
            READ: 'PartneriRead',
            VIDI_MOBILNI: 'PartneriVidiMobilni',
            SKORO_KREIRANI: 'PartneriSkoroKreirani',
        },
        PARTNERI_ANALIZA: {
            READ: 'PartnerAnalizaRead',
        },
        IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA: {
            READ: 'IzvestajUkupneKolicinePoRobiUFiltriranimDokumentimaRead',
        },
        IZVESTAJ_IZLAZA_ROBE_PO_GODINAMA: {
            READ: 'IzvestajIzlazaRobePoGodinamaRead',
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
        OTPREMNICE: {
            READ: 'OtpremniceRead',
            LOCK: 'OtpremniceLock',
            UNLOCK: 'OtpremniceUnlock',
        },
        ROBA_POPIS: {
            READ: 'RobaPopisRead',
            FILTER_7_DANA_UNAZAD: 'RobaPopisFilter7DanaUnazad',
            FILTER_SVI_DATUMI: 'RobaPopisFilterSviDatumi',
            LOCK: 'RobaPopisFilterLock',
            UNLOCK: 'RobaPopisFilterUnlock',
            STORNIRAJ: 'RobaPopisFilterStorniraj',
        },
        MASOVNI_SMS: {
            READ: 'MasovniSMSRead',
        },
        KALENDAR_AKTIVNOSTI: {
            READ: 'KalendarAktivnostiRead',
            WRITE: 'KalendarAktivnostiWrite',
            EDIT_ALL: 'KalendarAktivnostiEditAll',
            APPROVE: 'KalendarAktivnostiApprove',
            DELETE: 'KalendarAktivnostiDelete',
        },
        TIP_ODSUSTVA: {
            READ: 'TipOdsustvaRead',
            WRITE: 'TipOdsustvaWrite',
        },
        TIP_KORISNIKA: {
            READ: 'TipKorisnikaRead',
            WRITE: 'TipKorisnikaWrite',
        },
        MODULE_HELP: {
            SYSTEM_WRITE: 'ModuleHelpSystemWrite',
        },
    },
}
