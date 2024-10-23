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
import { toast } from 'react-toastify'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS } from '@/constants'

export const ProracunNoviDialog = ({ open, onClose, onCancel, onSuccess }) => {
    const defaultType = 0
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
                        <MenuItem value={0}>Maloprodajni</MenuItem>
                        <MenuItem value={1}>Veleprodajni</MenuItem>
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
                            .post(ENDPOINTS.PRORACUNI.POST, noviRequest)
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
