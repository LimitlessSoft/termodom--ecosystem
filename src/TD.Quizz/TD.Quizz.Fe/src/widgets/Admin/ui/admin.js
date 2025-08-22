import { Button, Grid, Stack, Typography } from '@mui/material'
import { QuizzList } from '@/widgets/QuizzList/ui/quizzList'
import { QuizzResults } from '@/widgets/QuizzResults/ui/quizzResults'
import { KeyboardArrowLeft } from '@mui/icons-material'
import NextLink from 'next/link'
import UsersList from '@/widgets/UsersList/UsersList'

export const Admin = () => {
    return (
        <Grid
            container
            alignItems={`center`}
            justifyContent={`center`}
            sx={{ minHeight: `100vh` }}
            gap={2}
        >
            <Grid item size={12}>
                <Stack
                    justifyContent={`center`}
                    alignItems={`center`}
                    spacing={2}
                >
                    <Button
                        startIcon={<KeyboardArrowLeft />}
                        variant={`contained`}
                        href={'/'}
                        LinkComponent={NextLink}
                    >
                        Povratak
                    </Button>
                    <Typography variant={`h5`}>Admin panel</Typography>
                </Stack>
            </Grid>
            <Grid item>
                <QuizzList />
            </Grid>
            <Grid item>
                <QuizzResults />
            </Grid>
            <Grid item>
                <UsersList />
            </Grid>
        </Grid>
    )
}
