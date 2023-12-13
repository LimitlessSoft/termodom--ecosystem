import { useAppDispatch, useAppSelector } from "@/app/hooks"
import { fetchMe, selectUser } from "@/features/userSlice/userSlice"
import { Grid } from "@mui/material"
import { useEffect } from "react"

const ProfiKutak = (): JSX.Element => {

    const dispatch = useAppDispatch()
    const user = useAppSelector(selectUser)

    useEffect(() => {
        dispatch(fetchMe())
        .then((res) => {
            if(res.type == 'user/fetchMe/fulfilled') {
                console.log(user)
            }
        })
    }, [dispatch])

    return (
        <Grid>

        </Grid>
    )
}

export default ProfiKutak