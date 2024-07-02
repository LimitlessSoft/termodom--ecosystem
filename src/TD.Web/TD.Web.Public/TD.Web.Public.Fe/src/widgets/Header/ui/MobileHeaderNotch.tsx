import { IClickable } from "@/interfaces/IClickable"
import { MobileHeaderNotchStyled } from "./MobileHeaderNotchStyled"
import { Grid, Typography } from "@mui/material"

export const MobileHeaderNotch = (props: IClickable): JSX.Element => {
    return (
        <Grid flexDirection={`row`} container alignItems={`center`}>
            <MobileHeaderNotchStyled
                onClick={() => {
                    props.onClick()
                }}>
                <span></span>
                <span></span>
                <span></span>
            </MobileHeaderNotchStyled>
            <Typography component={`h2`} variant="subtitle1" color={`black`} fontSize={32}>Meni</Typography>
        </Grid>
    )
}