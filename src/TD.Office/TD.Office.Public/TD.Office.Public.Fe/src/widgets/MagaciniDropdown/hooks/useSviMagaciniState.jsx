import { useEffect, useRef, useState } from 'react'

export const useSviMagaciniState = (onChange) => {
    const isMounted = useRef(false)
    const [state, setState] = useState(false)
    useEffect(() => {
        if (isMounted.current && onChange) {
            onChange(state)
        } else {
            isMounted.current = true
        }
    }, [state])
    return [state, setState]
}
