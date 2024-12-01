import { Home, Widgets, ListAlt, People, Settings } from '@mui/icons-material'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { URL_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'

export const NAV_BAR_CONSTANTS = {
    MODULE_LABELS: {
        KONTROLNA_TABLA: 'Kontrolna tabla',
        PROIZVODI: 'Proizvodi',
        PORUDZBINE: 'Porudzbine',
        KORISNICI: 'Korisnici',
        PODESAVANJA: 'Podesavanja',
    },
    MODULES: (permissions) => [
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.KONTROLNA_TABLA,
            href: '/',
            icon: <Home />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.PROIZVODI,
            href: URL_CONSTANTS.PROIZVODI.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PROIZVODI.READ
            ),
            icon: <Widgets />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.PORUDZBINE,
            href: URL_CONSTANTS.PORUDZBINE.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PORUDZBINE.READ
            ),
            icon: <ListAlt />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.KORISNICI,
            href: URL_CONSTANTS.KORISNICI.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KORISNICI.READ
            ),
            icon: <People />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.PODESAVANJA,
            href: URL_CONSTANTS.PODESAVANJA.INDEX,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PODESAVANJA.READ
            ),
            icon: <Settings />,
        },
    ],
}
