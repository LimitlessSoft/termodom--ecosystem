import { Grid } from '@mui/material'
import { PartneriList } from '@/widgets'

const Partneri = () => {
    return (
        <Grid container p={2} gap={2}>
            <Grid item xs={12}>
                <PartneriList />
            </Grid>
        </Grid>
    )
}

export default Partneri
