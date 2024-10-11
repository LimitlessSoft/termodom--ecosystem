import { Button, Grid, Stack, Typography } from '@mui/material'
import { Copyright } from '@mui/icons-material'
import { mainTheme } from '@/app/theme'
import Link from 'next/link'

const Footer = (): JSX.Element => {
    const linkColor = `#5cff00`

    return (
        <Grid
            container
            justifyContent={`center`}
            sx={{
                paddingY: 2,
                backgroundColor: mainTheme.palette.secondary.main,
            }}
        >
            <Grid
                container
                maxWidth={`1100px`}
                margin={`0 auto`}
                p={2}
                color={mainTheme.palette.secondary.contrastText}
            >
                <Grid item py={2}>
                    <Typography component={`h3`} variant={`h5`}>
                        Termodom - centar građevinskog materijala
                    </Typography>
                    <Typography component={`h4`}>
                        Centrala - Zrenjaninski put 84, 11213 Krnjača, Beograd
                    </Typography>
                    <Typography component={`h4`}>
                        Web podrška - 064 108 39 32
                    </Typography>
                    <Typography component={`h4`}>
                        Email -{' '}
                        <a
                            href={`mailto:vukasin@termodom.rs`}
                            style={{
                                color: linkColor,
                            }}
                        >
                            vukasin@termodom.rs
                        </a>
                    </Typography>
                </Grid>
                <Grid item xs={12} sm={2} py={2}>
                    <Stack
                        spacing={1}
                        textAlign={`center`}
                        px={{
                            xs: 0,
                            md: 2,
                        }}
                        width={`100%`}
                    >
                        <Typography component={`h4`} fontWeight={`bolder`}>
                            Brzi linkovi
                        </Typography>
                        <Typography component={`h4`}>
                            <Button
                                variant={`outlined`}
                                color={`info`}
                                LinkComponent={Link}
                                href={`/profi-kutak`}
                            >
                                Profi Kutak
                            </Button>
                        </Typography>
                        <Typography component={`h4`}>
                            <Button
                                variant={`outlined`}
                                color={`info`}
                                LinkComponent={Link}
                                href={`/korpa`}
                            >
                                Korpa
                            </Button>
                        </Typography>
                        <Typography component={`h4`}>
                            <Button
                                variant={`outlined`}
                                color={`info`}
                                LinkComponent={Link}
                                href={`/kontakt`}
                            >
                                Kontakt
                            </Button>
                        </Typography>
                    </Stack>
                </Grid>
                <Grid item flexGrow={1} py={2}></Grid>
                <Grid item textAlign={`right`} py={2}>
                    <Typography>
                        Aplikacija je u vlasništvu TERMOSHOP D.O.O.
                    </Typography>
                    <Button
                        color={`info`}
                        variant={`text`}
                        target="_blank"
                        href={'https://limitlesssoft.com'}
                    >
                        <Typography mx={1}>LimitlessSoft</Typography>
                        <Copyright fontSize={`small`} />
                        <Typography mx={1}>
                            2021 - {new Date().getFullYear()}
                        </Typography>
                    </Button>
                </Grid>
            </Grid>
        </Grid>
    )
}

export default Footer
