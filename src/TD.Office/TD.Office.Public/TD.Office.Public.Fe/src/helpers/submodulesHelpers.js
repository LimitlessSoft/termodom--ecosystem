import { useEffect, useState } from 'react'

export const createSubModules = (subModulesConfig) => {
    const [subModules, setSubModules] = useState(subModulesConfig)

    useEffect(() => {
        setSubModules(subModulesConfig)
    }, [subModulesConfig])

    return subModules
}
