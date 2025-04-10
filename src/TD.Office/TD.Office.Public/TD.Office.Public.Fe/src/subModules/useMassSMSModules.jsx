import { useMemo } from 'react'
import { URL_CONSTANTS } from '../constants'
import { useSubModules } from '../hooks/useSubModulesHook'

export const useMassSMSModules = () => {
    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.MASS_SMS.INDEX,
                label: 'Masovni SMS',
                hasPermission: true,
            },
        ],
        []
    )
    return useSubModules(subModulesConfig)
}
