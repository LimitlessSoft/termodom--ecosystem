import { formatNumber } from "@/app/helpers/numberHelpers"
import { Grid, LinearProgress, Typography } from "@mui/material"

export const OneTimePrice = (props: any): JSX.Element => {

    const prices = props.prices

    return (
        prices == null ? <LinearProgress /> :
        <Grid
            sx={{ marginTop: `2px` }}>
            <Typography
                color={`rgb(203 148 92)`}
                variant={`caption`}>
                {props.currentGroup?.type == 1 ? `VP` : `MP`}
                &nbsp;Cena /{props.unit}:
            </Typography>
            <Grid color={`green`}>
                <Typography
                    variant={`caption`}>
                        Od:
                    </Typography>
                <Typography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}>
                        { formatNumber(prices.minPrice * (props.currentGroup?.type == 1 ? 1 : (1 + (props.vat / 100)))) } RSD
                    </Typography>
            </Grid>
            <Grid color={`red`}>
                <Typography
                    variant={`caption`}>
                        Do:
                    </Typography>
                <Typography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}>
                        { formatNumber(prices.maxPrice * (props.currentGroup?.type == 1 ? 1 : (1 + (props.vat / 100)))) } RSD
                    </Typography>
            </Grid>
        </Grid>
    )
}