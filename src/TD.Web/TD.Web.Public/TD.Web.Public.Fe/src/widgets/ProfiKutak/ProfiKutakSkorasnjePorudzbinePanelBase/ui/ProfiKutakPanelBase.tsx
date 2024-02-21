import { Grid, Paper, Typography, styled } from "@mui/material"
import { IProfiKutakPanelBaseProps } from "../interfaces/IProfiKutakPanelBaseProps"

const ProfiKutakPanelBaseStyled = styled(Paper)(
    ({ theme }) => `
        padding: 8px 15px;
        border-radius: 10px;
        margin: 10px;
    `
)

export const ProfiKutakPanelBase = (props: IProfiKutakPanelBaseProps): JSX.Element => {
    return (
        <ProfiKutakPanelBaseStyled
            variant={`outlined`}>
                <Typography
                    variant={`h6`}
                    sx={{
                        m: 1
                    }}>
                    { props.title }
                </Typography>
                <Grid>
                    {props.children}
                </Grid>
        </ProfiKutakPanelBaseStyled>
    )
}