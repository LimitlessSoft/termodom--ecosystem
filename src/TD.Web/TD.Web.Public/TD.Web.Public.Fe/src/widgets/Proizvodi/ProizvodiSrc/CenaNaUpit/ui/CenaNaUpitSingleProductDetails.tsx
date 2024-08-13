import { Phone } from '@mui/icons-material'
import { Alert, Button, Grid, Stack } from '@mui/material'
import NextLink from 'next/link'
import { ICenaNaUpitSingleProductDetailsProps } from '../interfaces/ICenaNaUpitSingleProductDetailsProps'

export const CenaNaUpitSingleProductDetails = ({
    isWholesale,
}: ICenaNaUpitSingleProductDetailsProps) => {
    return (
        <Grid py={2}>
            <Stack gap={2}>
                <Alert severity={`info`} variant={`filled`}>
                    {isWholesale
                        ? 'Samo za kupce sa ugovorom'
                        : 'Pozovi za cenu'}
                </Alert>
                <Button
                    color={`secondary`}
                    startIcon={<Phone />}
                    variant={`contained`}
                    LinkComponent={NextLink}
                    href={`tel:0641083932`}
                >
                    064 108 39 32
                </Button>
            </Stack>
        </Grid>
    )
}
