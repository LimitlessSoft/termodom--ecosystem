import Grid2 from '@mui/material/Unstable_Grid2'
import { PregledIUplataPazaraTable } from './PregledIUplataPazaraTable'
import { PregledIUplataPazaraFilters } from './PregledIUplataPazaraFilters'
import { useState } from 'react'
import {
    Divider,
    LinearProgress,
    Paper,
    Stack,
    TextField,
    useTheme,
} from '@mui/material'
import { PregledIUplataPazaraNeispravneStavkeIzvoda } from './PregledIUplataPazaraNeispravneStavkeIzvoda'

export const PregledIUplataPazara = () => {
    const [loading, setLoading] = useState(false)
    const [data, setData] = useState()
    const theme = useTheme()
    return (
        <Grid2 container gap={2} marginBottom={4}>
            <Grid2>
                <PregledIUplataPazaraFilters
                    onLoadingChanged={(e) => {
                        setLoading(e)
                    }}
                    onData={(e) => {
                        setData(e)
                    }}
                />
            </Grid2>
            <Grid2 xs={12}>
                <PregledIUplataPazaraNeispravneStavkeIzvoda />
            </Grid2>
            {data && (
                <Grid2 xs={12}>
                    <Divider
                        sx={{
                            my: 2,
                        }}
                    />
                </Grid2>
            )}
            <Grid2 xs={12}>
                {loading && <LinearProgress />}
                {!loading && data && <PregledIUplataPazaraTable data={data} />}
            </Grid2>
            {data && (
                <Grid2 xs={12}>
                    <Grid2
                        container
                        gap={2}
                        alignItems={`center`}
                        justifyContent={`flex-end`}
                    >
                        <Grid2>
                            <Paper
                                sx={{
                                    p: 2,
                                }}
                            >
                                <TextField
                                    sx={{
                                        backgroundColor:
                                            theme.palette.warning.light,
                                        borderRadius: 1,
                                    }}
                                    variant={`filled`}
                                    label={`Ukupna razlika`}
                                    value={data.ukupnaRazlikaFormatted}
                                />
                            </Paper>
                        </Grid2>
                    </Grid2>
                </Grid2>
            )}
            {data && (
                <Grid2 xs={12}>
                    <Grid2 container justifyContent={`flex-end`}>
                        <Grid2>
                            <Paper
                                sx={{
                                    p: 2,
                                }}
                            >
                                <Stack gap={2}>
                                    <TextField
                                        variant={`filled`}
                                        label={`Ukupno Mp Racuna`}
                                        value={data.ukupnoMpRacunaFormatted}
                                    />
                                    <TextField
                                        variant={`filled`}
                                        label={`Ukupno Povratnica`}
                                        value={data.ukupnoPovratniceFormatted}
                                    />
                                    <TextField
                                        variant={`filled`}
                                        label={`Ukupno Potrazuje`}
                                        value={data.ukupnoPotrazujeFormatted}
                                    />
                                </Stack>
                            </Paper>
                        </Grid2>
                    </Grid2>
                </Grid2>
            )}
        </Grid2>
    )
}
