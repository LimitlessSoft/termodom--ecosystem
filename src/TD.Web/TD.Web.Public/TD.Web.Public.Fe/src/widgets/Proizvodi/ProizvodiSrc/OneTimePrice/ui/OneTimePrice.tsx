import { Grid } from '@mui/material'
import { IOneTimePriceProps } from '../models/IOneTimePriceProps'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { formatNumber } from '@/app/helpers/numberHelpers'

export const OneTimePrice = (props: IOneTimePriceProps) => {
    const { minPrice, maxPrice } = props.data.oneTimePrice
    const vatMultiplier = props.data.isWholesale ? 1 : 1 + props.data.vat / 100
    const formattedMinPriceWithVAT = formatNumber(minPrice * vatMultiplier)
    const formattedMaxPriceWithVAT = formatNumber(maxPrice * vatMultiplier)

    return (
        <Grid container textAlign={`center`} my={3}>
            <Grid item sm={12}>
                <Grid>
                    <ResponsiveTypography
                        scale={1.4}
                        variant={`h6`}
                        component={`h2`}
                    >
                        <ResponsiveTypography
                            component={`span`}
                            sx={{ marginRight: `5px`, fontSize: `0.6em` }}
                        >
                            {props.data.isWholesale ? `VP` : `MP`}
                            &nbsp;Cena: &nbsp;
                        </ResponsiveTypography>
                        {minPrice == maxPrice ? (
                            formattedMaxPriceWithVAT
                        ) : (
                            <>
                                {formattedMinPriceWithVAT}
                                &nbsp;-&nbsp;
                                {formattedMaxPriceWithVAT}
                            </>
                        )}
                        <ResponsiveTypography
                            component={`span`}
                            sx={{ marginLeft: `5px`, fontSize: `0.6em` }}
                        >
                            RSD/{props.data.unit}
                        </ResponsiveTypography>
                    </ResponsiveTypography>
                </Grid>
                <Grid my={1}>
                    <ResponsiveTypography
                        component={`span`}
                        sx={{ fontSize: `0.8em`, color: `rgb(203 148 92)` }}
                    >
                        *{props.data.isWholesale ? `vp` : `mp`} cena zavisi od
                        ukupne vrednosti vaše korpe. Tačnu cenu ćete videti u
                        korpi
                    </ResponsiveTypography>
                </Grid>
            </Grid>
        </Grid>
    )
}
