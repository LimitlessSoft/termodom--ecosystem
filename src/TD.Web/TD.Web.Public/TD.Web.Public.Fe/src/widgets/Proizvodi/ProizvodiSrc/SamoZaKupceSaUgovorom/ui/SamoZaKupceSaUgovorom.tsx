import { Alert, Button, Grid, Stack } from '@mui/material'

export const SamoZaKupceSaUgovorom = () => {
    return (
        <Grid py={2}>
            <Stack gap={2}>
                <Alert severity={`info`} variant={`filled`}>
                    Samo za kupce sa ugovorom
                </Alert>
            </Stack>
        </Grid>
    )
}
