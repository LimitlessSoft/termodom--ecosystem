import {
    Accordion,
    AccordionSummary,
    Box,
    CircularProgress,
    Grid,
    Stack,
} from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { useEffect, useState } from 'react'
import { officeApi } from '@/apis/officeApi'
import { Notes, PartneriSkoroKreirani } from '@/widgets'
import { ArrowDownward } from '@mui/icons-material'
import { DashboardAccordion } from '@/widgets/DashboardAccordion/ui/DashboardAccordion'

const Home = () => {
    const user = useUser()

    return user?.isLogged == null || user.isLogged == false ? (
        <CircularProgress />
    ) : (
        <Grid container gap={2} p={2}>
            <Grid item xs={4}>
                <Stack gap={2}>
                    <DashboardAccordion
                        badgeCount={null} // TODO: implement separate endpoint for this
                        caption={'Skoro kreirani partneri'}
                        component={<PartneriSkoroKreirani />}
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
