import { hasPermission } from '../helpers/permissionsHelpers'
import { usePermissions } from '../hooks/usePermissionsHook'
import { PERMISSIONS_GROUPS, USER_PERMISSIONS } from '../constants'
import { useEffect, useState } from 'react'

export const usePartneriSubModules = () => {
    const [modules, setModules] = useState(undefined)
    const partneriPermissions = usePermissions(PERMISSIONS_GROUPS.PARTNERI)
    const partneriFinansijskoIKomercijalnoPermissions = usePermissions(
        PERMISSIONS_GROUPS.PARTNERI_FINANSIJSKO_I_KOMERCIJALNO
    )

    useEffect(() => {
        setModules([
            {
                href: '/finansijsko&komercijalno',
                label: 'Finansijsko',
                hasPermission: hasPermission(
                    partneriPermissions,
                    USER_PERMISSIONS.PARTNERI.READ
                ),
            },
            {
                href: '/nesto-drugo',
                label: 'Nesto drugo',
                hasPermission: hasPermission(
                    partneriFinansijskoIKomercijalnoPermissions,
                    USER_PERMISSIONS.PARTNERI_FINANSIJSKO_I_KOMERCIJALNO.READ
                ),
            },
        ])
    }, [partneriPermissions, partneriFinansijskoIKomercijalnoPermissions])

    return modules
}
