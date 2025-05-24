import { CircularProgress, Grid, Stack } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import {
    KomercijalnoNeispravneCene,
    Notes,
    PartneriSkoroKreirani,
    Zoomable,
} from '@/widgets'
import { DashboardAccordion } from '@/widgets/DashboardAccordion/ui/DashboardAccordion'
import { ENDPOINTS_CONSTANTS } from '@/constants'

const Home = () => {
    const user = useUser()

    const [
        komercijalnoNeisparvneCeneCount,
        setKomercijalnoNeisparvneCeneCount,
    ] = useState(null)

    useEffect(() => {
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.IZVESTAJI
                    .GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA_COUNT
            )
            .then((res) => setKomercijalnoNeisparvneCeneCount(res.data))
            .catch(handleApiError)
    }, [])

    return user?.isLogged == null || user.isLogged == false ? (
        <CircularProgress />
    ) : (
        <Grid container spacing={2} p={2}>
            <Grid item xs={4}>
                <Stack gap={1}>
                    <DashboardAccordion
                        disabled={false}
                        badgeCount={null} // TODO: implement separate endpoint for this
                        caption={'Skoro kreirani partneri'}
                        component={<PartneriSkoroKreirani />}
                    />
                    <DashboardAccordion
                        disabled={
                            komercijalnoNeisparvneCeneCount === undefined ||
                            komercijalnoNeisparvneCeneCount === null
                        }
                        badgeCount={komercijalnoNeisparvneCeneCount}
                        caption={'Neispravne cene u magacinima'}
                        component={<KomercijalnoNeispravneCene />}
                    />
                </Stack>
            </Grid>
            <Grid item>
                <Notes />
            </Grid>
        </Grid>
    )
}

export default Home
