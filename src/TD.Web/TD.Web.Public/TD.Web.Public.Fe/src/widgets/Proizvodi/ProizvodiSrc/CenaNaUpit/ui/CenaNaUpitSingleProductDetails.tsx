import { Alert, Button, Grid, Stack } from '@mui/material'
import { Phone } from '@mui/icons-material'
import NextLink from 'next/link'
import { PHONE_CONSTANTS } from '@/constants'

export const CenaNaUpitSingleProductDetails = () => {
    const { SASA_PHONE } = PHONE_CONSTANTS

    return (
        <Grid py={2}>
            <Stack gap={2}>
                <Alert severity={`info`} variant={`filled`}>
                    Pozovi za cenu
                </Alert>
                <Button
                    color={`secondary`}
                    startIcon={<Phone />}
                    variant={`contained`}
                    LinkComponent={NextLink}
                    href={`tel:${SASA_PHONE}`}
                >
                    {SASA_PHONE}
                </Button>
            </Stack>
        </Grid>
    )
}
