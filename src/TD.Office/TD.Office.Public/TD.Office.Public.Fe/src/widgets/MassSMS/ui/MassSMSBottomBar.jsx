import {
    Button,
    Divider,
    LinearProgress,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import {
    forceReloadMassSMSQueueCountAsync,
    forceReloadZMassSMSQueueAsync,
    forceReloadZMassSMSStatus,
    useZMassSMSQueue,
    useZMassSMSStatus,
} from '../../../zStore'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { toast } from 'react-toastify'
import { ConfirmDialog } from '../../ConfirmDialog/ui/ConfirmDialog'

export const MassSMSBottomBar = ({
    sending,
    disabled,
    onStartSending,
    onFinishSending,
}) => {
    const zMassSMSQueue = useZMassSMSQueue()
    const status = useZMassSMSStatus()

    const [isTestSMSSent, setIsTestSMSSent] = useState(false)
    const [isSendMessageDialogOpen, setIsSendMessageDialogOpen] =
        useState(false)

    const sendHandler = async () => {
        onStartSending()
        await officeApi
            .post(ENDPOINTS_CONSTANTS.MASS_SMS.SEND)
            .then(() => {
                forceReloadZMassSMSQueueAsync()
                forceReloadMassSMSQueueCountAsync()
                forceReloadZMassSMSStatus()

                toast.success('Pokrenutno je slanje masovnih SMS poruka!')
            })
            .catch(handleApiError)
            .finally(onFinishSending)
    }

    const handleSendTestSMS = () => {
        onStartSending()
        toast.info('Jos uvek nije napravljeno, ali ipak mozes nastaviti')
        setIsTestSMSSent(true)
        onFinishSending()
    }

    const handleCloseSendMessageDialog = () => setIsSendMessageDialogOpen(false)
    const handleOpenSendMessageDialog = () => setIsSendMessageDialogOpen(true)

    useEffect(() => {
        setIsTestSMSSent(false)
    }, [zMassSMSQueue])

    if (status !== 'Initial') return

    return (
        <Paper sx={{ p: 2 }}>
            <Stack gap={2}>
                <ConfirmDialog
                    isOpen={isSendMessageDialogOpen}
                    onCancel={handleCloseSendMessageDialog}
                    onConfirm={() => {
                        sendHandler()
                        handleCloseSendMessageDialog()
                    }}
                />
                <Button
                    disabled={disabled}
                    variant={`contained`}
                    color={`info`}
                    onClick={handleSendTestSMS}
                >
                    Posalji test SMS
                </Button>
                <Divider />
                {!isTestSMSSent && (
                    <Typography textAlign={`center`} variant={`subtitle1`}>
                        Posalji test SMS da bi ti se polje ispod omogucilo
                    </Typography>
                )}
                <Button
                    disabled={disabled || !isTestSMSSent}
                    sx={{
                        py: 2,
                    }}
                    variant={`contained`}
                    color={`success`}
                    onClick={handleOpenSendMessageDialog}
                >
                    Posalji masovni SMS
                </Button>
                {sending && <LinearProgress />}
            </Stack>
        </Paper>
    )
}
