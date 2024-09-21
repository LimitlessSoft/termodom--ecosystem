import { Alert, Button, Grid, Stack } from '@mui/material'
import { Phone } from '@mui/icons-material'
import NextLink from 'next/link'
import { SASA_PHONE } from '@/constants'

export const CenaNaUpitSingleProductDetails = () => {
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
                    064 108 39 32
                </Button>
            </Stack>
        </Grid>
    )
}
