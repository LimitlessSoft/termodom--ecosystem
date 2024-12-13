import { hasPermission } from '../helpers/permissionsHelpers'
import { usePermissions } from '../hooks/usePermissionsHook'
import { URL_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { useMemo } from 'react'
import { useSubModules } from '@/hooks/useSubModulesHook'

export const useIzvestajiSubModules = () => {
    const izvestajUkupneKolicineRobeUDokumentimaPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS
            .IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA
    )

    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.IZVESTAJI
                    .IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA,
                label: 'Izvestaj ukupne kolicine robe u dokumentima',
                hasPermission: hasPermission(
                    izvestajUkupneKolicineRobeUDokumentimaPermissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                        .IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA
                        .READ
                ),
            },
            {
                href: URL_CONSTANTS.IZVESTAJI.IZVESTAJ_IZLAZA_ROBE_PO_GODINAMA,
                label: 'Izvestaj izlaza robe po godinama',
                hasPermission: () => true,
            },
        ],
        [izvestajUkupneKolicineRobeUDokumentimaPermissions]
    )

    return useSubModules(subModulesConfig)
}
