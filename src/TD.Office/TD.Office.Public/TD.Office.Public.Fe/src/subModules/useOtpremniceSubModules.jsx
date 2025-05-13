import { useSubModules } from '../hooks/useSubModulesHook'
import { useMemo } from 'react'
import { URL_CONSTANTS } from '../constants'

export const useOtpremniceSubModules = () => {
    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.OTPREMNICE.MP,
                label: 'Nalozi za izdvajanje MP',
                hasPermission: true,
            },
            {
                href: URL_CONSTANTS.OTPREMNICE.VP,
                label: 'Nalozi za izdvajanje VP',
                hasPermission: true,
            },
        ],
        []
    )
    return useSubModules(subModulesConfig)
}
