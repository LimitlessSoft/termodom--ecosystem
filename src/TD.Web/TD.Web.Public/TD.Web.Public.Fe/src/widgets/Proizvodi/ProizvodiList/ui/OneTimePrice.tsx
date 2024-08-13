import { formatNumber } from '@/app/helpers/numberHelpers'
import {
    Alert,
    Button,
    Grid,
    LinearProgress,
    Stack,
    Typography,
} from '@mui/material'
import { Phone } from '@mui/icons-material'
import { CenaNaUpitListProductCard } from '@/widgets/Proizvodi/ProizvodiSrc/CenaNaUpit/ui/CenaNaUpitListProductCard'

export const OneTimePrice = (props: any): JSX.Element => {
    const prices = props.prices

    return prices == null ? (
        <LinearProgress />
    ) : prices.minPrice === 0 || prices.maxPrice === 0 ? (
        <CenaNaUpitListProductCard />
    ) : (
        <Grid sx={{ marginTop: `2px` }}>
            <Typography color={`rgb(203 148 92)`} variant={`caption`}>
                {props.currentGroup?.type == 1 ? `VP` : `MP`}
                &nbsp;Cena /{props.unit}:
            </Typography>
            <Grid color={`green`}>
                <Typography variant={`caption`}>Od:</Typography>
                <Typography
                    sx={{ mx: 0.5 }}
                    component={'span'}
                    variant={`subtitle2`}
                >
                    {formatNumber(
                        prices.minPrice *
                            (props.currentGroup?.type == 1
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
                        prices.maxPrice *
                            (props.currentGroup?.type == 1
                                ? 1
                                : 1 + props.vat / 100)
                    )}{' '}
                    RSD
                </Typography>
            </Grid>
        </Grid>
    )
}
