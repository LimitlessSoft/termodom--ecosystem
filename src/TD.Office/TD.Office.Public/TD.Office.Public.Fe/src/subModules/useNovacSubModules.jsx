import { useMemo } from 'react'
import { URL_CONSTANTS } from '../constants'
import { useSubModules } from '../hooks/useSubModulesHook'

export const UseNovacSubModules = () => {
    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.NOVAC.INDEX,
                label: 'Specifikacija novca',
                hasPermission: true,
            },
            {
                href: URL_CONSTANTS.NOVAC.PREGLED_I_UPLATA_PAZARA,
                label: 'Pregled i uplata pazara',
                hasPermission: true,
            },
        ],
        []
    )
    return useSubModules(subModulesConfig)
}
