import { useEffect, useRef, useState } from 'react'

export const useSingleSelectState = (onChange) => {
    const isMounted = useRef(false)
    const [state, setState] = useState(undefined)
    useEffect(() => {
        if (isMounted.current && onChange) {
            onChange(state)
        } else {
            isMounted.current = true
        }
    }, [state])
    return [state, setState]
}
