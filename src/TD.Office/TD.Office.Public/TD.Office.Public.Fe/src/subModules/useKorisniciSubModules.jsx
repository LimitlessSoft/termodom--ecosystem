import { hasPermission } from '../helpers/permissionsHelpers'
import { usePermissions } from '../hooks/usePermissionsHook'
import { PERMISSIONS_CONSTANTS, URL_CONSTANTS } from '@/constants'
import { useMemo } from 'react'
import { useSubModules } from '@/hooks/useSubModulesHook'

export const useKorisniciSubModules = () => {
    const korisniciListPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.KORISNICI_LIST
    )

    const kalendarAktivnostiPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.KALENDAR_AKTIVNOSTI
    )

    const tipOdsustvaPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.TIP_ODSUSTVA
    )

    const tipKorisnikaPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.TIP_KORISNIKA
    )

    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.KORISNICI.LISTA,
                label: 'Lista korisnika',
                hasPermission: hasPermission(
                    korisniciListPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KORISNICI.LIST_READ
                ),
            },
            {
                href: URL_CONSTANTS.KORISNICI.KALENDAR_AKTIVNOSTI,
                label: 'Kalendar aktivnosti',
                hasPermission: hasPermission(
                    kalendarAktivnostiPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KALENDAR_AKTIVNOSTI.READ
                ),
            },
            {
                href: URL_CONSTANTS.KORISNICI.TIPOVI_ODSUSTVA,
                label: 'Tipovi odsustva',
                hasPermission: hasPermission(
                    tipOdsustvaPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TIP_ODSUSTVA.READ
                ),
            },
            {
                href: URL_CONSTANTS.KORISNICI.TIPOVI_KORISNIKA,
                label: 'Tipovi korisnika',
                hasPermission: hasPermission(
                    tipKorisnikaPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TIP_KORISNIKA.READ
                ),
            },
        ],
        [korisniciListPermissions, kalendarAktivnostiPermissions, tipOdsustvaPermissions, tipKorisnikaPermissions]
    )

    return useSubModules(subModulesConfig)
}
