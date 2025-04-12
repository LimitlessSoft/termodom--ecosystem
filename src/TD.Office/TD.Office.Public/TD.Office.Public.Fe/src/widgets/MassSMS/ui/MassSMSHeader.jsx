import { Alert, LinearProgress, Paper, Stack, Typography } from '@mui/material'
import { formatNumber } from '../../../helpers/numberHelpers'
import { useZMassSMSQueueCount, useZMassSMSStatus } from '../../../zStore'
import { massSMSHelpers } from '../../../helpers/massSMSHelpers'

export const MassSMSHeader = () => {
    const queueCount = useZMassSMSQueueCount()
    const status = useZMassSMSStatus()

    return (
        <Paper
            sx={{
                p: 2,
            }}
        >
            <Stack gap={1}>
                <Alert severity="info" variant={`filled`}>
                    Ovaj modul priprema SMS poruke i salje ih uredjaju za slanje
                    poruka. Ako ovde nema poruka u listi, to je znak da su
                    poruke poslate u uredjaju, ali ne znaci da je uredjaj
                    uspesno poslao te poruke! Da proverite tacan status poruke
                    na GSM modemu, pogledaj bazu na 4monitor.
                </Alert>
                {!status ? (
                    <LinearProgress />
                ) : (
                    <Typography>
                        Trenutni status masovnih SMS:{' '}
                        {massSMSHelpers.formatStatus(status)}
                    </Typography>
                )}
                {queueCount === undefined || !status ? (
                    <LinearProgress />
                ) : (
                    <Typography>
                        Poruka u {massSMSHelpers.formatStatus(status)}:{' '}
                        {formatNumber(queueCount, { decimalCount: 0 })}
                    </Typography>
                )}
            </Stack>
        </Paper>
    )
}
