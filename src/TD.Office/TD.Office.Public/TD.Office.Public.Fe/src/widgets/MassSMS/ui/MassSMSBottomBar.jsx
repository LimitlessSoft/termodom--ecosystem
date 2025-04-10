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
} from '../../../zStore'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { toast } from 'react-toastify'
import { ConfirmDialog } from '../../ConfirmDialog/ui/ConfirmDialog'

export const MassSMSBottomBar = () => {
    const zMassSMSQueue = useZMassSMSQueue()

    const [testSmsSent, setTestSmsSent] = useState(false)
    const [confirm, setConfirm] = useState(false)
    const [preparing, setPreparing] = useState(false)

    const sendHandler = async () => {
        setPreparing(true)
        await officeApi
            .post(ENDPOINTS_CONSTANTS.MASS_SMS.SEND)
            .then(() => {
                forceReloadZMassSMSQueueAsync()
                forceReloadMassSMSQueueCountAsync()
                forceReloadZMassSMSStatus()
                toast.success('Pokrenutno je slanje masovnih SMS poruka!')
            })
            .catch(handleApiError)
            .finally(() => {
                setPreparing(false)
            })
    }

    useEffect(() => {
        setTestSmsSent(false)
    }, [zMassSMSQueue])

    return (
        <Paper sx={{ p: 2 }}>
            <Stack gap={2}>
                <ConfirmDialog
                    isOpen={confirm}
                    onCancel={() => {
                        setConfirm(false)
                    }}
                    onConfirm={() => {
                        sendHandler()
                        setConfirm(false)
                    }}
                />
                <Button
                    disabled={preparing}
                    variant={`contained`}
                    color={`info`}
                    onClick={() => {
                        toast.info(
                            'Jos uvek nije napravljeno, ali ipak mozes nastaviti'
                        )
                        setTestSmsSent(true)
                    }}
                >
                    Posalji test SMS
                </Button>
                <Divider />
                {!testSmsSent && (
                    <Typography textAlign={`center`} variant={`subtitle1`}>
                        Posalji test SMS da bi ti se polje ispod omogucilo
                    </Typography>
                )}
                <Button
                    disabled={preparing || !testSmsSent}
                    sx={{
                        py: 2,
                    }}
                    variant={`contained`}
                    color={`success`}
                    onClick={() => {
                        setConfirm(true)
                    }}
                >
                    Posalji masovni SMS
                </Button>
                {preparing && <LinearProgress />}
            </Stack>
        </Paper>
    )
}
