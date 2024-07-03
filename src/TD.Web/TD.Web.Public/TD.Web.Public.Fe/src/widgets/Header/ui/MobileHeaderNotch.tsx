import { IClickable } from "@/interfaces/IClickable"
import { MobileHeaderNotchStyled } from "./MobileHeaderNotchStyled"
import { Grid, Typography } from "@mui/material"

export const MobileHeaderNotch = (props: IClickable): JSX.Element => {
    return (
        <Grid container alignItems={`center`}>
            <MobileHeaderNotchStyled
                onClick={() => {
                    props.onClick()
                }}>
                <span></span>
                <span></span>
                <span></span>
            </MobileHeaderNotchStyled>
            <Typography component={`span`} variant={`subtitle1`} color={`black`} fontSize={{
                xs: 32
            }}>Meni</Typography>
        </Grid>
    )
}