import { formatNumber } from "@/app/helpers/numberHelpers"
import { Grid, Typography, styled } from "@mui/material"

export const PorudzbinaSummary = (): JSX.Element => {

    const BasicTStyled = styled(Typography)(
        ({ theme }) => `
            font-size: 1.5em;
            text-align: right;
        `
    )

    return (
        <Grid
            container
            direction={`column`}
            alignItems={`flex-end`}
            sx={{
                px: 2
            }}>
                <Grid item>
                    <BasicTStyled>
                        Osnovica: {formatNumber(123123)}
                    </BasicTStyled>
                    <BasicTStyled>
                        PDV: {formatNumber(123123)}
                    </BasicTStyled>
                    <BasicTStyled>
                        Za Uplatu: {formatNumber(123123)}
                    </BasicTStyled>
                    <BasicTStyled
                        sx={{
                            fontWeight: `bold`
                        }}>
                        Ušteda: {formatNumber(123123)}
                    </BasicTStyled>
                </Grid>
        </Grid>
    )
}