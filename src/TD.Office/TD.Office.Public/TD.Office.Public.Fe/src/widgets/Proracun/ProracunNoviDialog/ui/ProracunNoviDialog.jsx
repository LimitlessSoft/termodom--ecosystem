import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    MenuItem,
    TextField,
} from '@mui/material'
import { useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'

export const ProracunNoviDialog = ({ open, onClose, onCancel, onSuccess }) => {
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
            <DialogTitle>Novi proračun</DialogTitle>
            <DialogContent>
                <Box p={1}>
                    <TextField
                        select
                        disabled={isCreating}
                        defaultValue={defaultType}
                        label={'Tip proračuna'}
                        sx={{
                            width: 300,
                        }}
                        onChange={(e) => {
                            setNoviRequest({
                                ...noviRequest,
                                type: e.target.value,
                            })
                        }}
                    >
                        {hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .CREATE_MP
                        ) && <MenuItem value={0}>Maloprodajni</MenuItem>}

                        {hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .CREATE_VP
                        ) && <MenuItem value={1}>Veleprodajni</MenuItem>}

                        {hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .CREATE_NALOG_ZA_UTOVAR
                        ) && <MenuItem value={2}>Nalog za utovar</MenuItem>}
                    </TextField>
                </Box>
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
