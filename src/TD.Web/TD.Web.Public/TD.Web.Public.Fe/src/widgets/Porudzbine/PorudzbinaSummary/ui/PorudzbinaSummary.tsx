import { IPorudzbinaSummaryProps } from '../models/IPorudzbinaSummaryProps'
import { formatNumber } from '@/app/helpers/numberHelpers'
import { mainTheme } from '@/app/theme'
import { Alert, Grid, Stack, Typography, styled } from '@mui/material'
import { STOCK_TYPES, STOCK_TYPES_MESSAGES } from '../../constants'

export const PorudzbinaSummary = ({
    porudzbina,
    stockTypes,
    isDelivery,
}: IPorudzbinaSummaryProps): JSX.Element => {
    const BasicTStyled = styled(Typography)(
        ({ theme }) => `
            font-size: 1.5em;
            text-align: right;
        `
    )

    const hasTranzitItem = porudzbina?.items.some((item) => {
        const stockType = stockTypes.find((type) => type.id === item.stockType)
        return stockType?.name === STOCK_TYPES.TRANZIT
    })

    const hasVelikaStovaristaItem = porudzbina?.items.some((item) => {
        const stockType = stockTypes?.find((type) => type.id === item.stockType)
        return stockType?.name === STOCK_TYPES.VELIKA_STOVARISTA
    })

    return (
        <Grid container p={2} alignItems={`center`}>
            <Grid item xs={8}>
                <Stack direction={`column`} gap={2}>
                    {hasVelikaStovaristaItem && !isDelivery && (
                        <Alert
                            severity={`info`}
                            variant={`filled`}
                            sx={{ alignItems: `center` }}
                        >
                            {STOCK_TYPES_MESSAGES.VELIKA_STOVARISTA_MESSAGE}
                        </Alert>
                    )}
                    {hasTranzitItem && (
                        <Alert
                            severity={`warning`}
                            variant={`filled`}
                            sx={{ alignItems: `center` }}
                        >
                            {STOCK_TYPES_MESSAGES.TRANZIT_MESSAGE}
                        </Alert>
                    )}
                </Stack>
            </Grid>
            <Grid item xs={4}>
                <Grid container direction={`column`} alignItems={`flex-end`}>
                    <Grid item>
                        <BasicTStyled>
                            Osnovica:{' '}
                            {formatNumber(porudzbina.summary.valueWithoutVAT)}
                        </BasicTStyled>
                        <BasicTStyled>
                            PDV: {formatNumber(porudzbina.summary.vatValue)}
                        </BasicTStyled>
                        <BasicTStyled>
                            Za Uplatu:{' '}
                            {formatNumber(porudzbina.summary.valueWithVAT)}
                        </BasicTStyled>
                        <BasicTStyled
                            sx={{
                                fontWeight: `bold`,
                                color: mainTheme.palette.success.main,
                            }}
                        >
                            Ušteda:{' '}
                            {formatNumber(porudzbina.summary.discountValue)}
                        </BasicTStyled>
                    </Grid>
                </Grid>
            </Grid>
        </Grid>
    )
}
