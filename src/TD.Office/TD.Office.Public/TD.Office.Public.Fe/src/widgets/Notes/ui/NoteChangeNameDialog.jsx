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

export const NoteChangeNameDialog = ({ open, name, onCancel, onConfirm }) => {
    const [value, setValue] = useState(name)

    return (
        <Dialog open={open} onClose={onCancel} fullWidth maxWidth={`xs`}>
            <DialogTitle>Promeni ime beleške</DialogTitle>
            <DialogContent>
                <Box py={2}>
                    <TextField
                        value={value}
                        fullWidth
                        label={`Novo ime`}
                        onChange={(e) => {
                            setValue(e.target.value)
                        }}
                    />
                </Box>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        onConfirm(value)
                    }}
                    variant={`contained`}
                >
                    Postavi novo ime
                </Button>
                <Button onClick={onCancel}>Odustani</Button>
            </DialogActions>
        </Dialog>
    )
}
