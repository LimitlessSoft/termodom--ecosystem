const registerConstants = {
    VALIDATION_FIELDS: {
        NICKNAME: { LABEL: 'Ime i prezime', FIELD: 'nickname' },
        COMPANY: { LABEL: 'Naziv firme', FIELD: 'company' },
        PIB: { LABEL: 'PIB', FIELD: 'pib' },
        MB: { LABEL: 'Matični broj', FIELD: 'mb' },
        USERNAME: { LABEL: 'Korisničko ime', FIELD: 'username' },
        PASSWORD: { LABEL: 'Lozinka', FIELD: 'password' },
        CONFIRM_PASSWORD: {
            LABEL: 'Potvrda lozinke',
            FIELD: 'confirmPassword',
        },
        DATE_OF_BIRTH: { LABEL: 'Datum rođenja', FIELD: 'dateOfBirth' },
        MOBILE: { LABEL: 'Mobilni telefon', FIELD: 'mobile' },
        ADDRESS: { LABEL: 'Adresa stanovanja', FIELD: 'address' },
        CITY: { LABEL: 'Mesto stanovanja', FIELD: 'cityId' },
        FAVORITE_STORE: { LABEL: 'Omiljena radnja', FIELD: 'favoriteStoreId' },
        MAIL: { LABEL: 'Važeća email adresa', FIELD: 'mail' },
    },
}

export default registerConstants
