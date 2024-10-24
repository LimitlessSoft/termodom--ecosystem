import { hasPermission } from '../helpers/permissionsHelpers'
import { usePermissions } from '../hooks/usePermissionsHook'
import {
    PERMISSIONS_GROUPS,
    URL_PREFIXES,
    USER_PERMISSIONS,
} from '../constants'
import { useMemo } from 'react'
import { useSubModules } from '@/hooks/useSubmodules'

export const usePartneriSubModules = () => {
    const partneriFinansijskoIKomercijalnoPermissions = usePermissions(
        PERMISSIONS_GROUPS.PARTNERI_FINANSIJSKO_I_KOMERCIJALNO
    )

    const subModulesConfig = useMemo(
        () => [
            {
                href: `${URL_PREFIXES.PARTNERI}/komercijalno-i-finansijsko`,
                label: 'Komercijalno i Finansijsko',
                hasPermission: hasPermission(
                    partneriFinansijskoIKomercijalnoPermissions,
                    USER_PERMISSIONS.PARTNERI_FINANSIJSKO_I_KOMERCIJALNO.READ
                ),
            },
            {
                href: `${URL_PREFIXES.PARTNERI}/drugo`,
                label: 'Nesto drugo',
                hasPermission: false,
            },
        ],
        [partneriFinansijskoIKomercijalnoPermissions]
    )

    return useSubModules(subModulesConfig)
}
