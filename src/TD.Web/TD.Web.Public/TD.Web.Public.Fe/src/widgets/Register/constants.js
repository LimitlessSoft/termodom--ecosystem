const registerConstants = {
    VALIDATION_FIELDS: {
        NICKNAME: { LABEL: 'Ime i prezime', FIELD: 'nickname', ID: 'nickname' },
        COMPANY: { LABEL: 'Naziv firme', FIELD: 'company', ID: 'company' },
        PIB: { LABEL: 'PIB', FIELD: 'pib', ID: 'pib' },
        MB: { LABEL: 'Matični broj', FIELD: 'mb', ID: 'mb' },
        USERNAME: {
            LABEL: 'Korisničko ime',
            FIELD: 'username',
            ID: 'username',
        },
        PASSWORD: { LABEL: 'Lozinka', FIELD: 'password', ID: 'password' },
        CONFIRM_PASSWORD: {
            LABEL: 'Potvrda lozinke',
            FIELD: 'confirmPassword',
            ID: 'confirm-password',
        },
        DATE_OF_BIRTH: {
            LABEL: 'Datum rođenja',
            FIELD: 'dateOfBirth',
            ID: 'date-of-birth',
        },
        MOBILE: { LABEL: 'Mobilni telefon', FIELD: 'mobile', ID: 'mobile' },
        ADDRESS: {
            LABEL: 'Adresa stanovanja',
            FIELD: 'address',
            ID: 'address',
        },
        CITY: { LABEL: 'Mesto stanovanja', FIELD: 'cityId' },
        FAVORITE_STORE: {
            LABEL: 'Omiljena radnja',
            FIELD: 'favoriteStoreId',
            ID: 'favorite-store-id',
        },
        MAIL: { LABEL: 'Važeća email adresa', FIELD: 'mail', ID: 'mail' },
    },
}

export default registerConstants
