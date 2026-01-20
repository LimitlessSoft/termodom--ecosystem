import { CircularProgress, Grid, Stack } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import {
    KomercijalnoNeispravneCene,
    Notes,
    PartneriSkoroKreirani,
} from '@/widgets'
import { DashboardAccordion } from '@/widgets/DashboardAccordion/ui/DashboardAccordion'
import {
    ENDPOINTS_CONSTANTS,
    NAV_BAR_CONSTANTS,
    PERMISSIONS_CONSTANTS,
    URL_CONSTANTS,
} from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { useRouter } from 'next/router'

const Home = () => {
    const user = useUser()
    const router = useRouter()
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.NAV_BAR
    )

    const [
        komercijalnoNeisparvneCeneCount,
        setKomercijalnoNeisparvneCeneCount,
    ] = useState(null)
    const [permissionsChecked, setPermissionsChecked] = useState(false)

    useEffect(() => {
        if (!permissions || permissions.length === 0) return

        const hasDashboardPermission = hasPermission(
            permissions,
            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.DASHBOARD.READ
        )

        if (!hasDashboardPermission) {
            const modules = NAV_BAR_CONSTANTS.MODULES(permissions)
            const firstAvailableModule = modules.find(
                (m) =>
                    m.href &&
                    m.href !== '/' &&
                    (!m.hasOwnProperty('hasPermission') || m.hasPermission)
            )

            if (firstAvailableModule) {
                router.replace(firstAvailableModule.href)
            } else {
                router.replace(URL_CONSTANTS.NEMA_PRISTUPA)
            }
            return
        }

        setPermissionsChecked(true)
    }, [permissions, router])

    useEffect(() => {
        if (!permissionsChecked) return

        officeApi
            .get(
                ENDPOINTS_CONSTANTS.IZVESTAJI
                    .GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA_COUNT
            )
            .then((res) => setKomercijalnoNeisparvneCeneCount(res.data))
            .catch(handleApiError)
    }, [permissionsChecked])

    if (!user?.isLogged || !permissionsChecked) {
        return <CircularProgress />
    }

    return (
        <Grid container spacing={2} p={2}>
            <Grid item xs={4}>
                <Stack gap={1}>
                    <DashboardAccordion
                        disabled={false}
                        badgeCount={null}
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
