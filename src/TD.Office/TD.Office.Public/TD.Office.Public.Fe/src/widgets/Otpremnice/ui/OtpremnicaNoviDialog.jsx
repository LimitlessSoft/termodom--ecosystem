import {
    Alert,
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    MenuItem,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { MagaciniDropdown } from '../../MagaciniDropdown/ui/MagaciniDropdown'

export const OtpremnicaNoviDialog = ({
    type,
    open,
    onClose,
    onCancel,
    onSuccess,
}) => {
    const defaultType = 0

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PRORACUNI
    )

    const [noviRequest, setNoviRequest] = useState({ type: defaultType })
    const [isCreating, setIsCreating] = useState(false)

    return (
        <Dialog
            open={open}
            onClose={() => {
                if (isCreating) return
                onClose()
            }}
        >
            <DialogTitle>{type} otpremnice | Nova</DialogTitle>
            <DialogContent>
                <Stack p={1} gap={2}>
                    <Typography variant={`h6`}>Iz magacina:</Typography>
                    {/*TODO: only MP/VP magacini should be visible in dropdown depending on type*/}
                    <MagaciniDropdown />
                    <Typography variant={`h6`}>U magacin:</Typography>
                    {/*TODO: only MP/VP magacini should be visible in dropdown depending on type*/}
                    <MagaciniDropdown />
                </Stack>
            </DialogContent>
            <DialogActions>
                <Button
                    variant={'contained'}
                    disabled={isCreating}
                    onClick={() => {
                        setIsCreating(true)
                        officeApi
                            .post(
                                ENDPOINTS_CONSTANTS.PRORACUNI.POST,
                                noviRequest
                            )
                            .then(() => {
                                onSuccess()
                            })
                            .catch(handleApiError)
                            .finally(() => {
                                setIsCreating(false)
                            })
                    }}
                >
                    Kreiraj
                </Button>
                <Button
                    variant={'outlined'}
                    disabled={isCreating}
                    onClick={() => {
                        onCancel()
                    }}
                >
                    Odustani
                </Button>
            </DialogActions>
        </Dialog>
    )
}
