import { formatNumber } from '@/app/helpers/numberHelpers'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { Alert, Grid, LinearProgress, Typography } from '@mui/material'
import { CenaNaUpit } from '@/widgets/Proizvodi/ProizvodiSrc/CenaNaUpit/ui/CenaNaUpit'

export const UserPrice = (props: any): JSX.Element => {
    const prices = props.prices

    return prices == null ? (
        <LinearProgress />
    ) : prices.priceWithoutVAT === 0 || prices.priceWithVAT === 0 ? (
        <CenaNaUpit />
    ) : (
        <Grid sx={{ marginTop: `2px` }}>
            <ResponsiveTypography color={`rgb(203 148 92)`} variant={`caption`}>
                {props.currentGroup?.name}
                Cena /{props.unit}:
            </ResponsiveTypography>
            <Grid color={`red`}>
                <ResponsiveTypography variant={`caption`}>
                    VP Cena:
                </ResponsiveTypography>
                <ResponsiveTypography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}
                >
                    {formatNumber(prices.priceWithoutVAT)} RSD
                </ResponsiveTypography>
            </Grid>
            <Grid color={`green`}>
                <ResponsiveTypography variant={`caption`}>
                    MP Cena:
                </ResponsiveTypography>
                <ResponsiveTypography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}
                >
                    {formatNumber(prices.priceWithVAT)} RSD
                </ResponsiveTypography>
            </Grid>
        </Grid>
    )
}
