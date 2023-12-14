import { useAppDispatch, useAppSelector } from "@/app/hooks"
import { fetchMe, selectUser } from "@/features/userSlice/userSlice"
import { Grid } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"

const ProfiKutak = (): JSX.Element => {

    const dispatch = useAppDispatch()
    const user = useAppSelector(selectUser)
    const router = useRouter()
    const [isRefreshingData, setIsRefreshingData] = useState<boolean>(true)

    useEffect(() => {
        dispatch(fetchMe())
        .then((res) => {
            setIsRefreshingData(false)
        })
    }, [dispatch])

    useEffect(() => {
        if(isRefreshingData)
            return
        
        if(!user.isLogged) {
            console.log('User is not logged in, redirecting to /logovanje')
            router.push('/logovanje')
        }
    }, [isRefreshingData, user, router])

    return (
        <Grid>

        </Grid>
    )
}

export default ProfiKutak