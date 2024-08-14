import { formatNumber } from '@/app/helpers/numberHelpers'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { Grid, LinearProgress } from '@mui/material'
import { CenaNaUpitListProductCard } from '@/widgets/Proizvodi/ProizvodiSrc/CenaNaUpit/ui/CenaNaUpitListProductCard'

export const UserPrice = (props: any): JSX.Element => {
    return !props.prices ? (
        <LinearProgress />
    ) : props.prices.priceWithoutVAT === 0 ||
      props.prices.priceWithVAT === 0 ? (
        <CenaNaUpitListProductCard />
    ) : (
        <Grid sx={{ marginTop: `2px` }}>
            <ResponsiveTypography color={`rgb(203 148 92)`} variant={`caption`}>
                {props.isWholesale ? `Veleprodajna` : `Maloprodajna`} Cena /
                {props.unit}:
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
                    {formatNumber(props.prices.priceWithoutVAT)} RSD
                </ResponsiveTypography>
            </Grid>
            {!props.isWholesale && (
                <Grid color={`green`}>
                    <ResponsiveTypography variant={`caption`}>
                        MP Cena:
                    </ResponsiveTypography>
                    <ResponsiveTypography
                        sx={{ mx: 0.5 }}
                        component={'span'}
                        variant={`subtitle2`}
                    >
                        {formatNumber(props.prices.priceWithVAT)} RSD
                    </ResponsiveTypography>
                </Grid>
            )}
        </Grid>
    )
}
