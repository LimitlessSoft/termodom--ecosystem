import { formatNumber } from '@/app/helpers/numberHelpers'
import { Grid, LinearProgress, Typography } from '@mui/material'
import { CenaNaUpitListProductCard } from '@/widgets/Proizvodi/ProizvodiSrc/CenaNaUpit/ui/CenaNaUpitListProductCard'

export const OneTimePrice = (props: any) => {
    const { minPrice, maxPrice } = props.prices

    return !props.prices ? (
        <LinearProgress />
    ) : minPrice === 0 || maxPrice === 0 ? (
        <CenaNaUpitListProductCard />
    ) : (
        <Grid sx={{ marginTop: `2px` }}>
            <Typography color={`rgb(203 148 92)`} variant={`caption`}>
                {props.isWholesale ? `VP` : `MP`}
                &nbsp;Cena /{props.unit}:
            </Typography>
            {minPrice == maxPrice ? (
                <Typography>{formatNumber(maxPrice)} RSD</Typography>
            ) : (
                <>
                    <Grid color={`green`}>
                        <Typography variant={`caption`}>Od:</Typography>
                        <Typography
                            sx={{ mx: 0.5 }}
                            component={'span'}
                            variant={`subtitle2`}
                        >
                            {formatNumber(
                                minPrice *
                                    (props.isWholesale
                                        ? 1
                                        : 1 + props.vat / 100)
                            )}{' '}
                            RSD
                        </Typography>
                    </Grid>
                    <Grid color={`red`}>
                        <Typography variant={`caption`}>Do:</Typography>
                        <Typography
                            sx={{ mx: 0.5 }}
                            component={'span'}
                            variant={`subtitle2`}
                        >
                            {formatNumber(
                                maxPrice *
                                    (props.isWholesale
                                        ? 1
                                        : 1 + props.vat / 100)
                            )}{' '}
                            RSD
                        </Typography>
                    </Grid>
                </>
            )}
        </Grid>
    )
}
