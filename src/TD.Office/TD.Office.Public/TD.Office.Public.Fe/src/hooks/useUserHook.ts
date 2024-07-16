import {fetchMe, selectUser} from "@/features/slices/userSlice/userSlice"
import {TypedUseSelectorHook, useDispatch, useSelector} from "react-redux"
import {AppDispatch, RootState} from "@/app/store"
import {ENDPOINTS} from "@/constants"
import {useRouter} from "next/router"
import {useEffect} from "react"

export const useAppDispatch: () => AppDispatch = useDispatch
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector

export const useUser = (redirectIfNotLogged: boolean = true, reload: boolean = false) => {

    const user = useAppSelector(selectUser)
    const dispatch = useAppDispatch()
    const router = useRouter()

    useEffect(() => {
        if(reload)
            dispatch(fetchMe())
    }, [reload, dispatch])

    useEffect(() => {
        if(user.isLogged == null)
            return

        if(!user.isLogged && redirectIfNotLogged && router.route !== ENDPOINTS.LOGIN)
            router.push(ENDPOINTS.LOGIN)
    }, [redirectIfNotLogged, user, user?.isLogged, router])

    return user
}