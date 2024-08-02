import { Grid, Paper, Typography } from '@mui/material'
import { ISpecifikacijaNovcaBoxProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaBoxProps'

export const SpecifikacijaNovcaBox = (props: ISpecifikacijaNovcaBoxProps) => {
    return (
        <Grid item>
            <Paper
                elevation={6}
                sx={{
                    padding: 2,
                }}
            >
                {props.title && <Typography mb={2}>{props.title}</Typography>}
                {props.children}
            </Paper>
        </Grid>
    )
}
