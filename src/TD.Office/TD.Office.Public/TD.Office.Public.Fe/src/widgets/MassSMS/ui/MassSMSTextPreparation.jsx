import {
    Alert,
    Button,
    LinearProgress,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { useZMassSMSStatus } from '../../../zStore'
import { massSMSHelpers } from '../../../helpers/massSMSHelpers'

export const MassSMSTextPreparation = () => {
    const status = useZMassSMSStatus()

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
                    sx={{
                        minWidth: 500,
                    }}
                    variant={`outlined`}
                    label={`Tekst poruke`}
                    error={false}
                    helperText={`Karaktera: 256/170`}
                />
                <Button variant={`contained`}>Postavi novi tekst poruke</Button>
            </Stack>
        </Paper>
    )
}
