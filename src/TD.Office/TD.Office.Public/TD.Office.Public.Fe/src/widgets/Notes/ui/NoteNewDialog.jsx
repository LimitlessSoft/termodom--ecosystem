import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
} from '@mui/material'
import { useState } from 'react'

export const NoteNewDialog = ({ open, onCancel, onConfirm, disabled }) => {
    const [value, setValue] = useState('')

    return (
        <Dialog open={open} onClose={onCancel} fullWidth maxWidth={`xs`}>
            <DialogTitle>Nova beleška</DialogTitle>
            <DialogContent>
                <Box py={2}>
                    <TextField
                        disabled={disabled}
                        value={value}
                        fullWidth
                        label={`Ime nove beleške`}
                        onChange={(e) => {
                            setValue(e.target.value)
                        }}
                    />
                </Box>
            </DialogContent>
            <DialogActions>
                <Button
                    disabled={disabled}
                    onClick={() => {
                        onConfirm(value)
                    }}
                    variant={`contained`}
                >
                    Kreiraj
                </Button>
                <Button onClick={onCancel} disabled={disabled}>
                    Odustani
                </Button>
            </DialogActions>
        </Dialog>
    )
}
