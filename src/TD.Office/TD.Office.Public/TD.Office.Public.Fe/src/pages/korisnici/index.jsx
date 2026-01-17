import { useEffect } from 'react'
import { useRouter } from 'next/router'
import { URL_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'

const KorisniciIndex = () => {
    const router = useRouter()

    const korisniciListPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.KORISNICI_LIST
    )
    const kalendarPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.KALENDAR_AKTIVNOSTI
    )
    const tipOdsustvaPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.TIP_ODSUSTVA
    )

    useEffect(() => {
        // Redirect to first available submodule based on permissions
        if (hasPermission(korisniciListPermissions, PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KORISNICI.LIST_READ)) {
            router.replace(URL_CONSTANTS.KORISNICI.LISTA)
        } else if (hasPermission(kalendarPermissions, PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KALENDAR_AKTIVNOSTI.READ)) {
            router.replace(URL_CONSTANTS.KORISNICI.KALENDAR_AKTIVNOSTI)
        } else if (hasPermission(tipOdsustvaPermissions, PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TIP_ODSUSTVA.READ)) {
            router.replace(URL_CONSTANTS.KORISNICI.TIPOVI_ODSUSTVA)
        }
    }, [router, korisniciListPermissions, kalendarPermissions, tipOdsustvaPermissions])

    return null
}

export default KorisniciIndex
