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

    const partneriAnalizaPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PARTNERI_ANALIZA
    )

    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.PARTNERI.LISTA,
                label: 'Partneri lista',
                hasPermission: hasPermission(
                    partneriPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PARTNERI.READ
                ),
            },
            {
                href: URL_CONSTANTS.PARTNERI.FINANSIJSKO_I_KOMERCIJALNO,
                label: 'Pregled komercijalno i finansijsko po godinama',
                hasPermission: hasPermission(
                    partneriFinansijskoIKomercijalnoPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                        .PARTNERI_FINANSIJSKO_I_KOMERCIJALNO.READ
                ),
            },
            {
                href: URL_CONSTANTS.PARTNERI.ANALIZA,
                label: 'Analiza partnera',
                hasPermission: hasPermission(
                    partneriAnalizaPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PARTNERI_ANALIZA.READ
                ),
            },
        ],
        [
            partneriFinansijskoIKomercijalnoPermissions,
            partneriPermissions,
            partneriAnalizaPermissions,
        ]
    )

    return useSubModules(subModulesConfig)
}
