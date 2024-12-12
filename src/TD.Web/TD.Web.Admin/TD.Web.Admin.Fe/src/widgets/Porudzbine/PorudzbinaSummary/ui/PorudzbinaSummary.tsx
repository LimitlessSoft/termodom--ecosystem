import { IPorudzbinaSummaryProps } from '../models/IPorudzbinaSummaryProps'
import { formatNumber } from '@/helpers/numberHelpers'
import { mainTheme } from '@/theme'
import { Grid, Stack, TextField, Typography, styled } from '@mui/material'

export const PorudzbinaSummary = (
    props: IPorudzbinaSummaryProps
): JSX.Element => {
    const BasicTStyled = styled(Typography)(
        ({ theme }) => `
            font-size: 1.5em;
            text-align: right;
        `
    )

    return (
        <Grid item xs={12} md={4}>
            <Stack>
                <BasicTStyled>
                    Osnovica:{' '}
                    {formatNumber(props.porudzbina.summary.valueWithoutVAT)}
                </BasicTStyled>
                <BasicTStyled>
                    PDV: {formatNumber(props.porudzbina.summary.vatValue)}
                </BasicTStyled>
                <BasicTStyled>
                    Za Uplatu:{' '}
                    {formatNumber(props.porudzbina.summary.valueWithVAT)}
                </BasicTStyled>
                <BasicTStyled
                    sx={{
                        fontWeight: `bold`,
                        color: mainTheme.palette.success.main,
                    }}
                >
                    Ušteda:{' '}
                    {formatNumber(props.porudzbina.summary.discountValue)}
                </BasicTStyled>
            </Stack>
        </Grid>
    )
}
