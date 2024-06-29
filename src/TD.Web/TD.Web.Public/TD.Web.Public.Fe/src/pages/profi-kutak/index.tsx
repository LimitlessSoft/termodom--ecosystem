import { useAppDispatch, useAppSelector, useUser } from "@/app/hooks"
import { ProfiKutakSkorasnjePorudzbinePanel } from "@/widgets/ProfiKutak/ProfiKutakSkorasnjePorudzbinePanel"
import { ProfiKutakPanelBaseStyled } from "@/widgets/ProfiKutak/ProfiKutakSkorasnjePorudzbinePanelBase/ui/ProfiKutakPanelBase"
import { ProfiKutakUserStatusPanel } from "@/widgets/ProfiKutak/ProfiKutakUserStatusPanel"
import { Button, CircularProgress, Grid, Typography } from "@mui/material"
import NextLink from "next/link"

const ProfiKutak = (): JSX.Element => {

    const user = useUser(true, true)

    return (
        user == null || user.isLoading || user.data === undefined || !user.isLogged ?
        <CircularProgress /> :
        <Grid
            container
            justifyContent={`center`}
            p={2}>
                <Typography my={5} variant={`h6`}>
                    Dobrodošao {user.data?.nickname}
                </Typography>
                <Grid item sm={12} marginBottom={5}>
                    <Grid
                        container
                        justifyContent={`center`}>
                        <Grid item>
                            <ProfiKutakPanelBaseStyled>
                                <Button
                                    variant={`contained`}
                                    component={NextLink}
                                    href={`/`}
                                    sx={{
                                        textAlign: `center`,
                                        fontFamily: `GothamProMedium`,
                                        py: 2,
                                        px: 6
                                    }}>
                                    Započni kupovinu
                                </Button>
                            </ProfiKutakPanelBaseStyled>
                        </Grid>
                    </Grid>
                </Grid>
                <ProfiKutakSkorasnjePorudzbinePanel />
                <ProfiKutakUserStatusPanel />
        </Grid>
    )
}

export default ProfiKutak
