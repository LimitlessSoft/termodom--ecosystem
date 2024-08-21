import { IPorudzbinaSummaryProps } from '../models/IPorudzbinaSummaryProps'
import { formatNumber } from '@/app/helpers/numberHelpers'
import { mainTheme } from '@/app/theme'
import { Grid, Typography, styled } from '@mui/material'

export const PorudzbinaSummary = ({
    porudzbina,
}: IPorudzbinaSummaryProps): JSX.Element => {
    const BasicTStyled = styled(Typography)(
        ({ theme }) => `
            font-size: 1.5em;
            text-align: right;
        `
    )

    return (
        <Grid p={2}>
            <BasicTStyled>
                Osnovica: {formatNumber(porudzbina.summary.valueWithoutVAT)}
            </BasicTStyled>
            <BasicTStyled>
                PDV: {formatNumber(porudzbina.summary.vatValue)}
            </BasicTStyled>
            <BasicTStyled>
                Za Uplatu: {formatNumber(porudzbina.summary.valueWithVAT)}
            </BasicTStyled>
            <BasicTStyled
                sx={{
                    fontWeight: `bold`,
                    color: mainTheme.palette.success.main,
                }}
            >
                Ušteda: {formatNumber(porudzbina.summary.discountValue)}
            </BasicTStyled>
        </Grid>
    )
}
