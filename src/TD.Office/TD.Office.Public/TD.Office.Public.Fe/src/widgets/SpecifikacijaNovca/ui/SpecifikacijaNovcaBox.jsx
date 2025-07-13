import { Grid, Paper, Typography } from '@mui/material'

export const SpecifikacijaNovcaBox = (props) => {
    return (
        <Grid item>
            <Paper
                elevation={6}
                sx={{
                    padding: 2,
                    backgroundColor: props.backgroundColor,
                }}
            >
                {props.title && <Typography mb={2}>{props.title}</Typography>}
                {props.children}
            </Paper>
        </Grid>
    )
}
