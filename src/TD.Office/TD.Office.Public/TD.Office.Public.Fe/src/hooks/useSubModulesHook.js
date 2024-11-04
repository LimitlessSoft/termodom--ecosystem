import { useEffect, useState } from 'react'

export const useSubModules = (subModulesConfig) => {
    const [subModules, setSubModules] = useState(subModulesConfig)

    useEffect(() => {
        setSubModules(subModulesConfig)
    }, [subModulesConfig])

    return subModules
}
