import { useMemo } from 'react'
import { USER_PERMISSIONS, PERMISSIONS_GROUPS } from '@/constants'

export const useModules = () => {
    return useMemo(
        () => [
            {
                label: 'Finansijsko i Komercijalno',
                href: '/finansijsko&komercijalno',
                permission_name:
                    USER_PERMISSIONS.PARTNERI_FINANSIJSKO_I_KOMERCIJALNO.READ,
                permission_group:
                    PERMISSIONS_GROUPS.PARTNERI_FINANSIJSKO_I_KOMERCIJALNO,
            },
            {
                label: 'Nesto drugo',
                href: '/nesto-drugo',
            },
        ],
        []
    )
}
