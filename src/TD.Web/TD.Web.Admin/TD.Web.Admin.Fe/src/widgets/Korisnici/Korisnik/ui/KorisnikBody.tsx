import { Grid, Typography } from "@mui/material"

export const KorisnikBody = (props: any): JSX.Element => {
    return (
        <Grid container>
            <Grid item sm={4}>
                <Typography>
                    Datum kreiranja naloga: {props.user.createdAt}
                </Typography>
                {props.user.username}
            </Grid>
            <Grid item sm={8}>
                {props.user.username}
            </Grid>
        </Grid>
    )
}