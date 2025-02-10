import { URL_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import {
    Description,
    Home,
    Language,
    LocalAtm,
    LocalShipping,
    Logout,
    People,
    Person,
    RequestQuote,
    SwapHorizontalCircle,
} from '@mui/icons-material'
import { hasPermission } from '@/helpers/permissionsHelpers'

export const NAV_BAR_CONSTANTS = {
    MODULE_LABELS: {
        PARTNERI: 'Partneri',
        NALOG_ZA_PREVOZ: 'Nalog za prevoz',
        OTPREMNICE: 'Otpremnice',
        PRORACUN: 'Proračun',
        SPECIFIKACIJA_NOVCA: 'Specifikacija novca',
        WEB_PRODAVNICA: 'Web prodavnica',
        IZVESTAJI: 'Izveštaji',
        KORISNICI: 'Korisnici',
        KONTROLNA_TABLA: 'Kontrolna tabla',
        ODJAVI_SE: 'Odjavi se',
    },
    MODULES: (permissions) => [
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.KONTROLNA_TABLA,
            href: '/',
            icon: <Home />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.PARTNERI,
            href: URL_CONSTANTS.PARTNERI.LISTA,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PARTNERI.READ
            ),
            icon: <People />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.NALOG_ZA_PREVOZ,
            href: URL_CONSTANTS.NALOG_ZA_PREVOZ.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.NALOG_ZA_PREVOZ.READ
            ),
            icon: <LocalShipping />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.OTPREMNICE,
            href: URL_CONSTANTS.OTPREMNICE.MP,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.OTPREMNICE.READ
            ),
            icon: <SwapHorizontalCircle />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.PRORACUN,
            href: URL_CONSTANTS.PRORACUN.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI.READ
            ),
            icon: <RequestQuote />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.SPECIFIKACIJA_NOVCA,
            href: URL_CONSTANTS.SPECIFIKACIJA_NOVCA.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.SPECIFIKACIJA_NOVCA.READ
            ),
            icon: <LocalAtm />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.WEB_PRODAVNICA,
            href: URL_CONSTANTS.WEB_PRODAVNICA.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.WEB_SHOP.READ
            ),
            icon: <Language />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.IZVESTAJI,
            href: URL_CONSTANTS.IZVESTAJI
                .IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                    .IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA
                    .READ
            ),
            icon: <Description />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.KORISNICI,
            href: URL_CONSTANTS.KORISNICI.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KORISNICI.READ
            ),
            icon: <Person />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.ODJAVI_SE,
            href: null,
            icon: <Logout />,
        },
    ],
}
