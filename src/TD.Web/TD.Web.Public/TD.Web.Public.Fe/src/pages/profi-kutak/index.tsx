import { useAppDispatch, useAppSelector, useUser } from "@/app/hooks"
import { ProfiKutakSkorasnjePorudzbinePanel } from "@/widgets/ProfiKutak/ProfiKutakSkorasnjePorudzbinePanel"
import { ProfiKutakUserStatusPanel } from "@/widgets/ProfiKutak/ProfiKutakUserStatusPanel"
import { CircularProgress, Grid } from "@mui/material"

const ProfiKutak = (): JSX.Element => {

    const user = useUser(true, true)

    return (
        user == null || user.isLoading ?
        <CircularProgress /> :
        <Grid
            container
            justifyContent={`center`}
            p={2}>
                <ProfiKutakUserStatusPanel />
                <ProfiKutakSkorasnjePorudzbinePanel />
        </Grid>
    )
}

export default ProfiKutak