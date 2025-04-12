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
                    component={`h2`}
                    sx={{ color: `red`, borderRight: `1px solid gray` }}
                >
                    <Typography component={`strong`} variant={`h6`}>
                        {formatNumber(props.data.userPrice.priceWithoutVAT)}
                    </Typography>
                    <Typography
                        component={`span`}
                        sx={{ marginLeft: `5px`, fontSize: `0.8rem` }}
                    >
                        RSD/{props.data.unit}
                    </Typography>
                    <Typography
                        component={`caption`}
                        sx={{ display: `block`, fontSize: `1rem` }}
                    >
                        cena bez PDV-a
                    </Typography>
                </ResponsiveTypography>
            </UserPriceLabelStyled>
            <UserPriceLabelStyled item>
                <ResponsiveTypography
                    scale={fontScale}
                    component={`h2`}
                    sx={{ color: `green` }}
                >
                    <Typography component={`strong`} variant={`h6`}>
                        {formatNumber(props.data.userPrice.priceWithVAT)}
                    </Typography>
                    <Typography
                        component={`span`}
                        sx={{ marginLeft: `5px`, fontSize: `0.8rem` }}
                    >
                        RSD/{props.data.unit}
                    </Typography>
                    <Typography
                        component={`caption`}
                        sx={{ display: `block`, fontSize: `1rem` }}
                    >
                        cena sa PDV-om
                    </Typography>
                </ResponsiveTypography>
            </UserPriceLabelStyled>
        </Grid>
    )
}
