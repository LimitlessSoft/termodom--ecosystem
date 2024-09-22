import { Grid, Typography } from '@mui/material'
import { IUserPriceProps } from '../models/IUserPriceProps'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { UserPriceLabelStyled } from './UserPriceLabelStyled'
import { formatNumber } from '@/app/helpers/numberHelpers'

export const UserPrice = (props: IUserPriceProps): JSX.Element => {
    const fontScale = 1.4

    return (
        <Grid container textAlign={`center`} my={2}>
            <UserPriceLabelStyled item>
                <ResponsiveTypography
                    scale={fontScale}
                    variant={`h6`}
                    component={`h2`}
                    sx={{ color: `red`, borderRight: `1px solid gray` }}
                >
                    {formatNumber(props.data.userPrice.priceWithoutVAT)}
                    <Typography
                        component={`span`}
                        sx={{ marginLeft: `5px`, fontSize: `0.6em` }}
                    >
                        RSD/{props.data.unit}
                    </Typography>
                    <Typography>cena bez PDV-a</Typography>
                </ResponsiveTypography>
            </UserPriceLabelStyled>
            <UserPriceLabelStyled item>
                <ResponsiveTypography
                    scale={fontScale}
                    variant={`h6`}
                    component={`h2`}
                    sx={{ color: `green` }}
                >
                    {formatNumber(props.data.userPrice.priceWithVAT)}
                    <Typography
                        component={`span`}
                        sx={{ marginLeft: `5px`, fontSize: `0.6em` }}
                    >
                        RSD/{props.data.unit}
                    </Typography>
                    <Typography>cena sa PDV-a</Typography>
                </ResponsiveTypography>
            </UserPriceLabelStyled>
        </Grid>
    )
}
