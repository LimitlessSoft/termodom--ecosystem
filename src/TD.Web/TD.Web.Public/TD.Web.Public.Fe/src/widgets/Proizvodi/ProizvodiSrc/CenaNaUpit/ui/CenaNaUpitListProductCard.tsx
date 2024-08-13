import { Alert, Grid, Typography } from '@mui/material'

export const CenaNaUpit = () => {
    return (
        <Grid py={1}>
            <Alert severity={`info`} variant={`filled`}>
                <Typography fontSize={`small`}>Klikni za cenu</Typography>
            </Alert>
        </Grid>
    )
}
