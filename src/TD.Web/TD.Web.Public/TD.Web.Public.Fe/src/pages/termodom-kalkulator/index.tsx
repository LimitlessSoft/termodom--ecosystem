import { Button, Grid, Paper, Typography } from '@mui/material'
import { useState } from 'react'
import { ArrowBack } from '@mui/icons-material'
import {
    TermodomKalkulatorFasada,
    TermodomKalkulatorSuvaGradnja,
} from '@/widgets'

const Page = () => {
    const [kalkulatorShown, setKalkulatorShown] = useState<string | undefined>(
        undefined
    )

    return (
        <Grid container p={2} justifyContent={`center`} gap={2}>
            <Grid item sm={12}>
                <Typography
                    textAlign={`center`}
                    component={`h1`}
                    variant={`h4`}
                    py={5}
                >
                    Termodom Kalkulator
                </Typography>
            </Grid>
            <Grid item sm={12}>
                {!kalkulatorShown && (
                    <Grid
                        container
                        justifyContent={`center`}
                        gap={2}
                        textAlign={`center`}
                    >
                        <Grid item sm={12}>
                            <Typography variant={`subtitle1`}>
                                Izaberi tip kalkulacije
                            </Typography>
                        </Grid>
                        <Grid item sm={4}>
                            <Paper
                                sx={{
                                    p: 2,
                                    py: 5,
                                }}
                            >
                                <Button
                                    sx={{
                                        p: 4,
                                    }}
                                    onClick={() => {
                                        setKalkulatorShown(`suva-gradnja`)
                                    }}
                                >
                                    <Typography textAlign={`center`}>
                                        Suva Gradnja
                                    </Typography>
                                </Button>
                            </Paper>
                        </Grid>
                        <Grid item sm={4}>
                            <Paper
                                sx={{
                                    p: 2,
                                    py: 5,
                                }}
                            >
                                <Button
                                    sx={{
                                        p: 4,
                                    }}
                                    onClick={() => {
                                        setKalkulatorShown(`fasada`)
                                    }}
                                >
                                    <Typography textAlign={`center`}>
                                        Fasada
                                    </Typography>
                                </Button>
                            </Paper>
                        </Grid>
                    </Grid>
                )}
                {kalkulatorShown && (
                    <Grid
                        container
                        justifyContent={`center`}
                        gap={2}
                        maxWidth={`600px`}
                        margin={`auto`}
                    >
                        <Grid item sm={12}>
                            <Button
                                startIcon={<ArrowBack />}
                                onClick={() => {
                                    setKalkulatorShown(undefined)
                                }}
                            >
                                <Typography textAlign={`center`}>
                                    Povratak
                                </Typography>
                            </Button>
                        </Grid>
                        <Grid item sm={12}>
                            {kalkulatorShown === `suva-gradnja` && (
                                <TermodomKalkulatorSuvaGradnja />
                            )}
                            {kalkulatorShown === `fasada` && (
                                <TermodomKalkulatorFasada />
                            )}
                        </Grid>
                    </Grid>
                )}
            </Grid>
        </Grid>
    )
}

export default Page
