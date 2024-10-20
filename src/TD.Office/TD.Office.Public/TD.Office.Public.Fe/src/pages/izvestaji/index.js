import { IzvestajUkupneKolicineRobeUDokumentima } from '../../widgets'
import { Box, Typography } from '@mui/material'

const IzvestajiPage = () => {
    return (
        <Box>
            <Typography variant={`h4`}>Izveštaji</Typography>
            {/*TODO: Wrap this into sub-modules*/}
            <IzvestajUkupneKolicineRobeUDokumentima />
        </Box>
    )
}

export default IzvestajiPage
