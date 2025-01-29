import { useSubModules } from '../hooks/useSubModulesHook'
import { useMemo } from 'react'
import { URL_CONSTANTS } from '../constants'

export const useOtpremniceSubModules = () => {
    const subModulesConfig = useMemo(
        () => [
            {
                href: URL_CONSTANTS.OTPREMNICE.MP,
                label: 'Interne otpremnice MP',
                hasPermission: true,
            },
            {
                href: URL_CONSTANTS.OTPREMNICE.VP,
                label: 'Interne otpremnice VP',
                hasPermission: true,
            },
        ],
        []
    )
    return useSubModules(subModulesConfig)
}
