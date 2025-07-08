import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Paper,
    Stack,
    Switch,
    TextField,
    Typography,
} from '@mui/material'
import { useState } from 'react'

export const NoteNewDialog = ({ open, onCancel, onConfirm, disabled }) => {
    const [value, setValue] = useState('')
    const [isNoteList, setIsNoteList] = useState(false)

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
                <Paper
                    sx={{
                        p: 2,
                        backgroundColor: 'orange',
                    }}
                >
                    <Stack
                        direction="row"
                        spacing={1}
                        justifyContent="center"
                        sx={{ alignItems: 'center' }}
                    >
                        <Typography>Text</Typography>
                        <Switch
                            checked={isNoteList}
                            onChange={(e) => {
                                setIsNoteList(e.target.checked)
                            }}
                            name="noteList"
                        />
                        <Typography>Lista Taskova</Typography>
                    </Stack>
                </Paper>
            </DialogContent>
            <DialogActions>
                <Button
                    disabled={disabled}
                    onClick={() => {
                        onConfirm(value, isNoteList)
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
