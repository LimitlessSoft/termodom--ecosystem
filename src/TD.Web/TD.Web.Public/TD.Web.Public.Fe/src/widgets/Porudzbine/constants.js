import { AlertProps } from '@mui/material'

export const STOCK_TYPES = {
    VELIKA_STOVARISTA: 'Samo velika stovarišta',
    STANDARD: 'Standard',
    TRANZIT: 'Tranzit',
}

export const STOCK_TYPES_MESSAGES = {
    VELIKA_STOVARISTA_MESSAGE:
        'Proveri stanje: U porudžbini se nalaze proizvodi koje možda trenutno nemamo na stanju. Trgovac će Vas kontaktirati u trenutku obrade Vaše porudžbine.',
    TRANZIT_MESSAGE:
        'Iz centralnog magacina: U porudžbini se nalaze proizvodi koji se isporučuju iz centralnog magacina. Kontaktiraćemo Vas uskoro.',
}

export const STORE_NAMES = {
    DOSTAVA: 'Dostava',
}

export const TIP_LAGERA = {
    ALERT_VARIANT: 'filled',
    ALERT_ALIGNMENT: { alignItems: 'center' },
}
