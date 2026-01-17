import { hasPermission } from '../helpers/permissionsHelpers'
import { usePermissions } from '../hooks/usePermissionsHook'
import { PERMISSIONS_CONSTANTS, URL_CONSTANTS } from '@/constants'
import { useMemo } from 'react'
import { useSubModules } from '@/hooks/useSubModulesHook'

export const useKorisniciSubModules = () => {
    const korisniciPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.NAV_BAR
    )

    const kalendarAktivnostiPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.KALENDAR_AKTIVNOSTI
    )

    const tipOdsustvaPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.TIP_ODSUSTVA
    )

    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.KORISNICI.LISTA,
                label: 'Lista korisnika',
                hasPermission: hasPermission(
                    korisniciPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KORISNICI.READ
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
        ],
        [korisniciPermissions, kalendarAktivnostiPermissions, tipOdsustvaPermissions]
    )

    return useSubModules(subModulesConfig)
}
