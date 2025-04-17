import {
    Alert,
    Button,
    LinearProgress,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import {
    forceReloadMassSMSQueueCountAsync,
    forceReloadZMassSMSQueueAsync,
    forceReloadZMassSMSStatus,
    useZMassSMSStatus,
} from '../../../zStore'
import { massSMSHelpers } from '../../../helpers/massSMSHelpers'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { ConfirmDialog } from '../../ConfirmDialog/ui/ConfirmDialog'
import { useState } from 'react'
import { toast } from 'react-toastify'

export const MassSMSPhoneNumbersPreparation = ({
    preparing,
    disabled,
    onStartPreparing,
    onFinishPreparing,
}) => {
    const [ukloniSveDialogOpened, setUkloniSveDialogOpened] = useState(false)
    const status = useZMassSMSStatus()

    const clearQueueHandler = async () => {
        onStartPreparing()
        await officeApi
            .delete(ENDPOINTS_CONSTANTS.MASS_SMS.CLEAR_QUEUE)
            .then(() => {
                forceReloadZMassSMSQueueAsync()
                forceReloadMassSMSQueueCountAsync()
            })
            .catch(handleApiError)
            .finally(onFinishPreparing)
    }

    const prepareNumbersFromPublicWebHandler = async () => {
        onStartPreparing()
        await officeApi
            .post(ENDPOINTS_CONSTANTS.MASS_SMS.PREPARE_NUMBERS_FROM_PUBLIC_WEB)
            .then(() => {
                forceReloadZMassSMSQueueAsync()
                forceReloadMassSMSQueueCountAsync()
            })
            .catch(handleApiError)
            .finally(onFinishPreparing)
    }

    const prepareNumbersFromKomercijalnoHandler = async () => {
        onStartPreparing()
        await officeApi
            .post(
                ENDPOINTS_CONSTANTS.MASS_SMS.PREPARE_NUMBERS_FROM_KOMERCIJALNO
            )
            .then(() => {
                forceReloadZMassSMSQueueAsync()
                forceReloadMassSMSQueueCountAsync()
            })
            .catch(handleApiError)
            .finally(onFinishPreparing)
    }

    const clearDuplicatesHandler = async () => {
        onStartPreparing()
        await officeApi
            .delete(ENDPOINTS_CONSTANTS.MASS_SMS.CLEAR_DUPLICATES)
            .then(() => {
                forceReloadZMassSMSQueueAsync()
                forceReloadMassSMSQueueCountAsync()
            })
            .catch(handleApiError)
            .finally(onFinishPreparing)
    }

    if (!status) return <LinearProgress />

    if (status !== 'Initial')
        return (
            <Alert severity={`info`} variant={`filled`}>
                <Typography fontSize={`small`}>
                    Status mora biti{' '}
                    <b>{massSMSHelpers.formatStatus('Initial')}</b> da bi videli
                    komande
                </Typography>
            </Alert>
        )

    return (
        <Paper sx={{ p: 2 }}>
            <Stack gap={1}>
                <Stack direction={`row`} gap={2}>
                    <Button
                        disabled={disabled}
                        variant={`contained`}
                        color={`secondary`}
                        onClick={() => {
                            prepareNumbersFromKomercijalnoHandler()
                        }}
                    >
                        Uvuci brojeve iz komercijalnog poslovanja
                    </Button>
                    <Button
                        disabled={disabled}
                        variant={`contained`}
                        color={`secondary`}
                        onClick={() => {
                            prepareNumbersFromPublicWebHandler()
                        }}
                    >
                        Uvuci brojeve sa sajta
                    </Button>
                    <Button
                        disabled={disabled || true} // Brojevi u TDOffice nisu jos uvek implementirani
                        variant={`contained`}
                        color={`secondary`}
                    >
                        Uvuci brojeve iz TDOffice-a
                    </Button>
                    <Button
                        disabled={disabled}
                        variant={`contained`}
                        color={`warning`}
                        onClick={() => {
                            clearDuplicatesHandler()
                        }}
                    >
                        Ukloni duplikate
                    </Button>
                    <Button
                        disabled={disabled}
                        variant={`contained`}
                        color={`warning`}
                        onClick={() => {
                            toast.error('Nije jos uvek implementirano')
                        }}
                    >
                        Ukloni blokirane
                    </Button>

                    <ConfirmDialog
                        isOpen={ukloniSveDialogOpened}
                        onCancel={() => {
                            setUkloniSveDialogOpened(false)
                        }}
                        onConfirm={async () => {
                            setUkloniSveDialogOpened(false)
                            clearQueueHandler()
                        }}
                    />
                    <Button
                        disabled={disabled}
                        variant={`contained`}
                        onClick={() => {
                            setUkloniSveDialogOpened(true)
                        }}
                    >
                        Ukloni sve pripremljene SMS poruke
                    </Button>
                </Stack>
                {preparing && <LinearProgress />}
            </Stack>
        </Paper>
    )
}
