import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { MagaciniDropdown } from '../../MagaciniDropdown/ui/MagaciniDropdown'
import { otpremniceHelpers } from '../../../helpers/otpremniceHelpers'

export const OtpremnicaNoviDialog = ({
    type,
    open,
    onClose,
    onCancel,
    onSuccess,
}) => {
    const defaultType = 0

    // const permissions = usePermissions(
    //     PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.OTPREMNICE
    // )

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
                    <MagaciniDropdown
                        excluteContainingStar
                        types={otpremniceHelpers.magaciniVrste(type)}
                        onChange={(e) => {
                            setNoviRequest((prev) => ({
                                ...prev,
                                polazniMagacinId: e,
                            }))
                        }}
                    />
                    <Typography variant={`h6`}>U magacin:</Typography>
                    <MagaciniDropdown
                        excluteContainingStar
                        types={otpremniceHelpers.magaciniVrste(type)}
                        onChange={(e) => {
                            setNoviRequest((prev) => ({
                                ...prev,
                                destinacioniMagacinId: e,
                            }))
                        }}
                    />
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
                                ENDPOINTS_CONSTANTS.OTPREMNICE.POST,
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
