import { useEffect, useRef, useState } from 'react'

export const useMountedState = ({ initialValue, onChange }) => {
    const isMounted = useRef(false)
    const [state, setState] = useState(initialValue)

    useEffect(() => {
        if (isMounted.current) {
            onChange(state)
        } else {
            isMounted.current = true
        }
    }, [state])

    return [state, setState]
}
