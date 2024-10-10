import { Button, Grid, Paper, Stack, Typography } from '@mui/material'
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
                    <Stack
                        gap={3}
                        justifyContent={`center`}
                        maxWidth={200}
                        margin={`auto`}
                    >
                        <Typography
                            variant={`subtitle1`}
                            py={2}
                            textAlign={`center`}
                        >
                            Izaberi tip kalkulacije
                        </Typography>
                        <Button
                            variant={`contained`}
                            onClick={() => {
                                setKalkulatorShown(`suva-gradnja`)
                            }}
                        >
                            <Typography textAlign={`center`}>
                                Suva Gradnja
                            </Typography>
                        </Button>
                        <Button
                            variant={`contained`}
                            onClick={() => {
                                setKalkulatorShown(`fasada`)
                            }}
                        >
                            <Typography textAlign={`center`}>Fasada</Typography>
                        </Button>
                    </Stack>
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
