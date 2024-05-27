import { mainTheme } from "@/app/theme";
import { Copyright } from "@mui/icons-material";
import { Button, Grid, Typography } from "@mui/material";

const Footer = (): JSX.Element => {
    return (
        <Grid container justifyContent={`center`} sx={{
            backgroundColor: mainTheme.palette.secondary.main,
        }}>
            <Grid container maxWidth={`1100px`} margin={`0 auto`}>
                <Grid item sm={12} textAlign={`right`}>
                    <Button color={`info`} variant={`text`} target="_blank" href={"https://limitlesssoft.com"}>
                        <Typography mx={1}>LimitlessSoft</Typography>
                        <Copyright fontSize={`small`} />
                        <Typography mx={1}>2021 - {new Date().getFullYear()}</Typography>
                    </Button>
                </Grid>
            </Grid>
        </Grid>
    );
}

export default Footer