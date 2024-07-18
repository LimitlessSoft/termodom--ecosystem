import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux'
import { fetchMe, selectUser } from '@/features/slices/userSlice/userSlice'
import { AppDispatch, RootState } from '@/store'
import { useRouter } from 'next/router'
import { useEffect } from 'react'

export const useAppDispatch: () => AppDispatch = useDispatch
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector

export const useUser = (
    redirectIfNotLogged: boolean = true,
    reload: boolean = false
) => {
    const router = useRouter()
    const dispatch = useAppDispatch()
    const user = useAppSelector(selectUser)

    useEffect(() => {
        if (reload) dispatch(fetchMe())
    }, [reload, dispatch])

    useEffect(() => {
        if (user.isLogged == null || user.isLoading) return

        if (
            !user.isLogged &&
            redirectIfNotLogged &&
            router.route !== '/logovanje'
        )
            router.push('/logovanje')
    }, [redirectIfNotLogged, user, user?.isLogged, router])

    return user
}
