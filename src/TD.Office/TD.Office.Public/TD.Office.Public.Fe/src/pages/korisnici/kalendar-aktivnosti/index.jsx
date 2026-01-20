import { KalendarAktivnosti, Notes } from '@/widgets'
import { Grid } from '@mui/material'

const KalendarAktivnostiPage = () => {
    return (
        <Grid container spacing={2}>
            <Grid item xs={12} lg={9}>
                <KalendarAktivnosti />
            </Grid>
            <Grid item xs={12} lg={3}>
                <Notes />
            </Grid>
        </Grid>
    )
}

export default KalendarAktivnostiPage
