import { useRouter } from 'next/router'
import { useEffect, useRef, useState } from 'react'
import queryHelpers from '@/helpers/queryHelpers'

const useQuery = (initialState, onMounted) => {
    if (!initialState)
        throw new Error('You must provide initialState to useQuery hook')
    const router = useRouter()
    const isMounted = useRef(false)
    const [state, setState] = useState(initialState)

    useEffect(() => {
        if (isMounted.current) {
            router.replace({
                pathname: router.pathname,
                query: queryHelpers.serialize(state),
            })
            return
        }
        isMounted.current = true
    }, [state])

    useEffect(() => {
        if (router.isReady && router.query) {
            const parsed = queryHelpers.parseQuery(router.query, state)
            setState(parsed)
            onMounted?.(parsed)
        }
    }, [])

    return [state, setState]
}

export default useQuery
