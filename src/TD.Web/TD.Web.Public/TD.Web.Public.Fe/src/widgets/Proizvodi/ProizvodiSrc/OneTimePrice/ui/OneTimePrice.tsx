import { Grid, Typography } from "@mui/material"
import { IOneTimePriceProps } from "../models/IOneTimePriceProps"
import { ResponsiveTypography } from "@/widgets/Responsive"

export const OneTimePrice = (props: IOneTimePriceProps): JSX.Element => {
    return (
        <Grid container textAlign={`center`} my={3}>
            <Grid item sm={12}>
                <Grid>
                    <ResponsiveTypography
                        scale={1.4}
                        variant={`h6`}
                        component={`h2`}>
                        <ResponsiveTypography component={`span`} sx={{ marginRight: `5px`, fontSize: `0.6em` }}>
                            MP Cena: {props.data.unit}
                        </ResponsiveTypography>
                        {(props.data.oneTimePrice.minPrice * (1 + (props.data.vat / 100))).toFixed(2)} - {(props.data.oneTimePrice.maxPrice * (1 + (props.data.vat / 100))).toFixed(2)}
                        <ResponsiveTypography component={`span`} sx={{ marginLeft: `5px`, fontSize: `0.6em` }}>
                            RSD/{props.data.unit}
                        </ResponsiveTypography>
                    </ResponsiveTypography>
                </Grid>
                <Grid my={1}>
                    <ResponsiveTypography component={`span`} sx={{ fontSize: `0.8em`, color: `rgb(203 148 92)` }}>
                        *mp cena zavisi od ukupne vrednosti vaše korpe. Tačnu cenu ćete videti u korpi
                    </ResponsiveTypography>
                </Grid>
            </Grid>
        </Grid>
    )
}