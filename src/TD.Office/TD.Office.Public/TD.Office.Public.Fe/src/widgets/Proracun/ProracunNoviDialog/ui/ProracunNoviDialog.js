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

export const ProracunNoviDialog = ({ open, onClose, onCancel, onSuccess }) => {
    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>Novi proračun</DialogTitle>
            <DialogContent>
                <Box p={1}>
                    <TextField
                        select
                        defaultValue={0}
                        label={'Tip proračuna'}
                        sx={{
                            width: 300,
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
                    onClick={() => {
                        toast('Kreirano')
                        onSuccess()
                    }}
                >
                    Kreiraj
                </Button>
                <Button
                    variant={'outlined'}
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
