import { useMemo } from 'react'
import { URL_CONSTANTS } from '../constants'
import { useSubModules } from '../hooks/useSubModulesHook'

export const useRobaSubModules = () => {
    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.ROBA.POPIS,
                label: 'Popis Robe',
                hasPermission: true,
            },
        ],
        []
    )
    return useSubModules(subModulesConfig)
}
