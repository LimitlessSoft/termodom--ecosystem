const orderConstants = {
    VALIDATION_FIELDS: {
        PICKUP_PLACE: { LABEL: 'Mesto preuzimanja', FIELD: 'storeId' },
        DELIVERY_ADDRESS: { LABEL: 'Adresa dostave', FIELD: 'deliveryAddress' },
        PAYMENT_TYPE: { LABEL: 'Način plaćanja', FIELD: 'paymentTypeId' },
        COMPANY: { LABEL: 'PIB / MB', FIELD: 'company' },
        FULL_NAME: { LABEL: 'Ime i prezime', FIELD: 'name' },
        MOBILE: { LABEL: 'Mobilni telefon', FIELD: 'mobile' },
        NOTE: { LABEL: 'Napomena', FIELD: 'note' },
    },
}

export default orderConstants
