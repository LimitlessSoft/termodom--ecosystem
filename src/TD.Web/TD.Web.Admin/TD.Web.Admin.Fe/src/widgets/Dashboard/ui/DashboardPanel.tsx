import { Grid, Typography } from "@mui/material"
import { DashboardPanelStyled } from "../styled/DashboardPanelStyled"
import { IDashboardPanelProps } from "../models/IDashboardPanelProps"

export const DashboardPanel = (props: IDashboardPanelProps): JSX.Element => {
    return (
        <Grid item sm={props.sm ?? 2}>
            <DashboardPanelStyled>
                {
                    props.title &&
                    <Typography variant={`h6`} fontWeight={`bold`}>
                        {props.title}
                    </Typography>
                }
                {props.children}
            </DashboardPanelStyled>
        </Grid>
    )
}