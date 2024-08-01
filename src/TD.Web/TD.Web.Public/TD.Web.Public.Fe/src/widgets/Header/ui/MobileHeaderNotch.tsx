import { IClickable } from '@/interfaces/IClickable'
import { MobileHeaderNotchStyled } from './MobileHeaderNotchStyled'
import { Grid, Typography } from '@mui/material'

export const MobileHeaderNotch = (props: IClickable): JSX.Element => {
    return (
        <MobileHeaderNotchStyled
            display={{
                xs: `block`,
                md: `none`,
            }}
            onClick={() => {
                props.onClick()
            }}
        >
            <Grid container spacing={1} alignItems={`center`}>
                <Grid item>
                    <span></span>
                    <span></span>
                    <span></span>
                </Grid>
                <Grid item>
                    <Typography
                        fontSize={{
                            xs: `32px`,
                        }}
                        component={`p`}
                        variant={`subtitle1`}
                    >
                        Meni
                    </Typography>
                </Grid>
            </Grid>
        </MobileHeaderNotchStyled>
    )
}
