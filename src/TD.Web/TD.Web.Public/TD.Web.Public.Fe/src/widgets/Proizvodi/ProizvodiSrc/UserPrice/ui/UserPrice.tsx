import { Grid, Typography } from "@mui/material"
import { IUserPriceProps } from "../models/IUserPriceProps"
import { ResponsiveTypography } from "@/widgets/Responsive"
import { UserPriceLabelStyled } from "./UserPriceLabelStyled"

export const UserPrice = (props: IUserPriceProps): JSX.Element => {

    const fontScale = 1.4

    return (
        <Grid container textAlign={`center`} my={2}>
            <UserPriceLabelStyled item>
                <ResponsiveTypography
                    scale={fontScale}
                    variant={`h6`}
                    component={`h2`}
                    sx={{ color: `red`, borderRight: `1px solid gray` }}>
                    {props.data.userPrice.priceWithoutVAT.toFixed(2)}
                    <Typography component={`span`} sx={{ marginLeft: `5px`, fontSize: `0.6em` }}>
                        RSD/{props.data.unit}
                    </Typography>
                    <Typography>
                        cena bez PDV-a
                    </Typography>
                </ResponsiveTypography>
            </UserPriceLabelStyled>
            <UserPriceLabelStyled item>
                <ResponsiveTypography
                    scale={fontScale}
                    variant={`h6`}
                    component={`h2`}
                    sx={{ color: `green` }}>
                    {props.data.userPrice.priceWithVAT.toFixed(2)}
                    <Typography component={`span`} sx={{ marginLeft: `5px`, fontSize: `0.6em` }}>
                        RSD/{props.data.unit}
                    </Typography>
                    <Typography>
                        cena sa PDV-a
                    </Typography>
                </ResponsiveTypography>
            </UserPriceLabelStyled>
        </Grid>
    )
}