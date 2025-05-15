import { useRouter } from 'next/router'
import { useEffect, useRef } from 'react'
import queryHelpers from '@/helpers/queryHelpers'

const useQuery = (itemsState, setItemsState) => {
    const router = useRouter()
    const isMounted = useRef(false)

    useEffect(() => {
        if (isMounted.current) {
            router.replace({
                pathname: router.pathname,
                query: queryHelpers.normalizeParams(itemsState),
            })
            return
        }

        isMounted.current = true
    }, [itemsState])

    useEffect(() => {
        if (router.isReady && router.query)
            setItemsState(queryHelpers.parseQuery(router.query, itemsState))
    }, [])
}

export default useQuery
