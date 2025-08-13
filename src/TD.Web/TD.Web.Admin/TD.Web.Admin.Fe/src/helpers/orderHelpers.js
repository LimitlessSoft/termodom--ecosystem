import {
    CheckCircle, LocalOffer,
    NotInterested,
    PhoneCallback,
    PhoneDisabled,
    ThumbUpOffAlt, TransferWithinAStation,
} from '@mui/icons-material'
import { toast } from 'react-toastify'

export const getTrgovacActionIcon = (trgovacAction) => {
    switch (trgovacAction) {
        case 0:
            return <NotInterested />
        case 1:
            return <PhoneDisabled />
        case 2:
            return <PhoneCallback />
        case 3:
            return <ThumbUpOffAlt />
        case 4:
            return <CheckCircle color={`success`} />
        case 5:
            return <TransferWithinAStation />
        case 6:
            return <LocalOffer />
        default:
            toast.error(`Nepoznata akcija trgovca: ${trgovacAction}`)
            return 'nepoznata'
    }
}

export const getTrgovacActionText = (trgovacAction) => {
    switch (trgovacAction) {
        case 0:
            return 'Bez trgovac akcije'
        case 1:
            return 'Trgovac je pozvao kupca, ali se kupac nije javio'
        case 2:
            return 'Trgovac je pozvao kupca i ponovo ga treba pozvati'
        case 3:
            return 'Trgovac je sve dogovorio sa kupcem'
        case 4:
            return 'Trgovac je oznacio porudžbinu kao isporučenu'
        case 5:
            return 'Prosleđeno lokalnom trgovcu'
        case 6:
            return 'Profaktura poslata kupcu'
        default:
            toast.error(`Nepoznata akcija trgovca: ${trgovacAction}`)
            return 'nepoznata'
    }
}
