import {
    Alert,
    Button,
    LinearProgress,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import { forceReloadZMassSMSQueue, useZMassSMSStatus } from '../../../zStore'
import { massSMSHelpers } from '../../../helpers/massSMSHelpers'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { ConfirmDialog } from '../../ConfirmDialog/ui/ConfirmDialog'
import { useState } from 'react'

export const MassSMSPhoneNumbersPreparation = () => {
    const [ukloniSveDialogOpened, setUkloniSveDialogOpened] = useState(false)
    const status = useZMassSMSStatus()

    const clearQueueHandler = async () => {
        await officeApi
            .delete(ENDPOINTS_CONSTANTS.MASS_SMS.CLEAR_QUEUE)
            .then(() => {
                forceReloadZMassSMSQueue()
            })
            .catch(handleApiError)
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
                    <Button variant={`contained`} color={`secondary`}>
                        Uvuci brojeve iz komercijalnog poslovanja
                    </Button>
                    <Button variant={`contained`} color={`secondary`}>
                        Uvuci brojeve sa sajta
                    </Button>
                    <Button variant={`contained`} color={`secondary`}>
                        Uvuci brojeve iz TDOffice-a
                    </Button>
                    <Button variant={`contained`} color={`warning`}>
                        Ukloni blokirane
                    </Button>
                    <>
                        <ConfirmDialog
                            isOpen={ukloniSveDialogOpened}
                            onCancel={() => {
                                setUkloniSveDialogOpened(false)
                            }}
                            onConfirm={async () => {
                                await clearQueueHandler()
                                setUkloniSveDialogOpened(false)
                            }}
                        />
                        <Button
                            variant={`contained`}
                            onClick={() => {
                                setUkloniSveDialogOpened(true)
                            }}
                        >
                            Ukloni sve pripremljene SMS poruke
                        </Button>
                    </>
                </Stack>
            </Stack>
        </Paper>
    )
}
