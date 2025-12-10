import { useMemo } from 'react'
import { PERMISSIONS_CONSTANTS, URL_CONSTANTS } from '../constants'
import { useSubModules } from '../hooks/useSubModulesHook'
import { hasPermission } from '../helpers/permissionsHelpers'
import { usePermissions } from '../hooks/usePermissionsHook'

export const useRobaSubModules = () => {
    const robaPopisPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.ROBA_POPIS
    )

    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.ROBA.POPIS,
                label: 'Popis Robe',
                hasPermission: hasPermission(
                    robaPopisPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.ROBA_POPIS.READ
                ),
            },
        ],
        [robaPopisPermissions]
    )
    return useSubModules(subModulesConfig)
}
