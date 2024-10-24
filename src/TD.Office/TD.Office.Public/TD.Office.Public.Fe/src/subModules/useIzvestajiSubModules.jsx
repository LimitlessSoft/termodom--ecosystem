import { hasPermission } from '../helpers/permissionsHelpers'
import { usePermissions } from '../hooks/usePermissionsHook'
import {
    PERMISSIONS_GROUPS,
    URL_PREFIXES,
    USER_PERMISSIONS,
} from '../constants'
import { useMemo } from 'react'
import { useSubModules } from '@/hooks/useSubmodules'

export const useIzvestajiSubModules = () => {
    const izvestajUkupneKolicineRobeUDokumentimaPermissions = usePermissions(
        PERMISSIONS_GROUPS.IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA
    )

    const subModulesConfig = useMemo(
        () => [
            {
                href: `${URL_PREFIXES.IZVESTAJI}/izvestaj-ukupne-kolicine-robe-u-dokumentima`,
                label: 'Izvestaj ukupne kolicine robe U dokumentima',
                hasPermission: hasPermission(
                    izvestajUkupneKolicineRobeUDokumentimaPermissions,
                    USER_PERMISSIONS
                        .IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA
                        .READ
                ),
            },
            {
                href: `${URL_PREFIXES.IZVESTAJI}/nesto-drugo`,
                label: 'Nesto drugo',
                hasPermission: false,
            },
        ],
        [izvestajUkupneKolicineRobeUDokumentimaPermissions]
    )

    return useSubModules(subModulesConfig)
}
