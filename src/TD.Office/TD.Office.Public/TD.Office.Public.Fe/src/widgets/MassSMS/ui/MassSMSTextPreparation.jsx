import {
    Alert,
    Button,
    CircularProgress,
    LinearProgress,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import {
    forceReloadMassSMSQueueCountAsync,
    forceReloadZMassSMSQueueAsync,
    useZMassSMSStatus,
} from '../../../zStore'
import { massSMSHelpers } from '../../../helpers/massSMSHelpers'
import { useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { toast } from 'react-toastify'

export const MassSMSTextPreparation = () => {
    const [text, setText] = useState('')
    const [preparing, setPreparing] = useState(false)
    const status = useZMassSMSStatus()

    const setTextHandler = async () => {
        setPreparing(true)
        await officeApi
            .put(ENDPOINTS_CONSTANTS.MASS_SMS.SET_TEXT, { text: text })
            .then(async () => {
                const p1 = forceReloadZMassSMSQueueAsync()
                const p2 = forceReloadMassSMSQueueCountAsync()
                await Promise.all([p1, p2])
                toast.success('Tekst je postavljen')
            })
            .catch(handleApiError)
            .finally(() => {
                setPreparing(false)
            })
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
            <Stack direction={`row`} gap={2}>
                <TextField
                    onChange={(e) => {
                        setText(e.target.value)
                    }}
                    value={text}
                    sx={{
                        width: 500,
                    }}
                    variant={`outlined`}
                    label={`Tekst poruke`}
                    error={text.length > 170}
                    helperText={`Karaktera: ${text.length}/170`}
                />
                <Button
                    disabled={preparing}
                    variant={`contained`}
                    startIcon={preparing ? <CircularProgress /> : null}
                    onClick={() => {
                        setTextHandler()
                    }}
                >
                    Postavi novi tekst poruke
                </Button>
            </Stack>
        </Paper>
    )
}
