import {
    MassSMSHeader,
    MassSMSPhoneNumbersPreparation,
    MassSMSQueue,
    MassSMSTextPreparation,
    MassSMSBottomBar,
} from '@/widgets'
import { Stack } from '@mui/material'
import Grid2 from '@mui/material/Unstable_Grid2'
import { useState } from 'react'

const MasovniSMSPage = () => {
    const [isPreparingPhoneNumbers, setIsPreparingPhoneNumbers] =
        useState(false)
    const [isPreparingMessageText, setIsPreparingMessageText] = useState(false)
    const [isSendingSMS, setIsSendingSMS] = useState(false)

    return (
        <Grid2 container spacing={2}>
            <Grid2 xs={12} lg={5}>
                <Stack gap={2}>
                    <MassSMSHeader />
                    <MassSMSPhoneNumbersPreparation
                        preparing={isPreparingPhoneNumbers}
                        disabled={
                            isPreparingPhoneNumbers ||
                            isPreparingMessageText ||
                            isSendingSMS
                        }
                        onStartPreparing={() =>
                            setIsPreparingPhoneNumbers(true)
                        }
                        onFinishPreparing={() =>
                            setIsPreparingPhoneNumbers(false)
                        }
                    />
                    <MassSMSTextPreparation
                        preparing={isPreparingMessageText}
                        disabled={
                            isPreparingPhoneNumbers ||
                            isPreparingMessageText ||
                            isSendingSMS
                        }
                        onStartPreparing={() => setIsPreparingMessageText(true)}
                        onFinishPreparing={() =>
                            setIsPreparingMessageText(false)
                        }
                    />
                    <MassSMSBottomBar
                        sending={isSendingSMS}
                        disabled={
                            isPreparingPhoneNumbers ||
                            isPreparingMessageText ||
                            isSendingSMS
                        }
                        onStartSending={() => setIsSendingSMS(true)}
                        onFinishSending={() => setIsSendingSMS(false)}
                    />
                </Stack>
            </Grid2>
            <Grid2 xs={12} lg={7}>
                <MassSMSQueue />
            </Grid2>
        </Grid2>
    )
}

export default MasovniSMSPage
