import { TypedUseSelectorHook, useDispatch, useSelector } from 'react-redux'
import { RootState, AppDispatch } from "./store";
import { useEffect, useState } from 'react';
import { fetchMe, selectUser, User } from '@/features/slices/userSlice/userSlice';
import { useRouter } from 'next/router';

export const useAppDispatch: () => AppDispatch = useDispatch
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector

export const useUser = () => {

    const router = useRouter()
    const dispatch = useAppDispatch()
    const user = useAppSelector(selectUser)
    const [isLogged, setIsLogged] = useState<boolean | null>(null)

    useEffect(() => {
        dispatch(fetchMe())
        .then((response) => {
            setIsLogged(response.payload.isLogged)
        })
    }, [dispatch])

    useEffect(() => {
        if(isLogged == null)
            return

        if(!isLogged)
            router.push('/logovanje')
        
    }, [isLogged, router])

    return user
}