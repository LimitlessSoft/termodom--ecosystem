import { Box, CircularProgress, Grid } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { useEffect, useState } from 'react'
import { officeApi } from '@/apis/officeApi'
import { Notes, PartneriSkoroKreirani } from '@/widgets'

const Home = () => {
    const user = useUser()

    return user?.isLogged == null || user.isLogged == false ? (
        <CircularProgress />
    ) : (
        <Grid container gap={2} p={2}>
            <Grid item>
                <PartneriSkoroKreirani />
            </Grid>
            <Grid item>
                <Notes />
            </Grid>
        </Grid>
    )
}

export default Home
