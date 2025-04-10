import {
    MassSMSHeader,
    MassSMSPhoneNumbersPreparation,
    MassSMSQueue,
    MassSMSTextPreparation,
    MassSMSBottomBar,
} from '@/widgets'
import { Stack } from '@mui/material'
import Grid2 from '@mui/material/Unstable_Grid2'

const MasovniSMSPage = () => {
    return (
        <>
            <Grid2 container spacing={2}>
                <Grid2 xs={12} lg={5}>
                    <Stack gap={2}>
                        <MassSMSHeader />
                        <MassSMSPhoneNumbersPreparation />
                        <MassSMSTextPreparation />
                        <MassSMSBottomBar />
                    </Stack>
                </Grid2>
                <Grid2 xs={12} lg={7}>
                    <MassSMSQueue />
                </Grid2>
            </Grid2>
        </>
    )
}

export default MasovniSMSPage
