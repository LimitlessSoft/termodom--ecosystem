import { useRouter } from 'next/router'
import { useEffect, useRef, useState } from 'react'
import queryHelpers from '@/helpers/queryHelpers'

const useQuery = (initialState, onMounted) => {
    if (!initialState)
        throw new Error('You must provide initialState to useQuery hook')
    const router = useRouter()
    const isMounted = useRef(false)
    const [state, setState] = useState(initialState)
    const [isReady, setIsReady] = useState(false)

    useEffect(() => {
        if (!isMounted.current) {
            isMounted.current = true
            return
        }

        router.replace({
            pathname: router.pathname,
            query: queryHelpers.serialize(state),
        })
    }, [state])

    useEffect(() => {
        if (!router.isReady) return

        if (Object.keys(router.query).length > 0) {
            const parsed = queryHelpers.parse(router.query)
            setState(parsed)
            onMounted?.(parsed)
        }
        setIsReady(true)
    }, [router.isReady])

    return [state, setState, isReady]
}

export default useQuery
