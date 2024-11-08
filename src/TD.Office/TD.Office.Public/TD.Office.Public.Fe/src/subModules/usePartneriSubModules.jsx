import { hasPermission } from '../helpers/permissionsHelpers'
import { usePermissions } from '../hooks/usePermissionsHook'
import { PERMISSIONS_CONSTANTS, URL_CONSTANTS } from '@/constants'
import { useMemo } from 'react'
import { useSubModules } from '@/hooks/useSubModulesHook'

export const usePartneriSubModules = () => {
    const partneriPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PARTNERI
    )

    const partneriFinansijskoIKomercijalnoPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS
            .PARTNERI_FINANSIJSKO_I_KOMERCIJALNO
    )

    const subModulesConfig = useMemo(
        () => [
            {
                href: `${URL_CONSTANTS.URL_PREFIXES.PARTNERI}/lista`,
                label: 'Partneri lista',
                hasPermission: hasPermission(
                    partneriPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PARTNERI.READ
                ),
            },
            {
                href: `${URL_CONSTANTS.URL_PREFIXES.PARTNERI}/komercijalno-i-finansijsko`,
                label: 'Pregled komercijalno i finansijsko po godinama',
                hasPermission: hasPermission(
                    partneriFinansijskoIKomercijalnoPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                        .PARTNERI_FINANSIJSKO_I_KOMERCIJALNO.READ
                ),
            },
        ],
        [partneriFinansijskoIKomercijalnoPermissions, partneriPermissions]
    )

    return useSubModules(subModulesConfig)
}
