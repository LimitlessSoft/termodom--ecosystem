import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
} from '@mui/material'

export const NoteDeleteDialog = ({ open, onConfirm, onCancel }) => {
    return (
        <Dialog
            open={open}
            onClose={() => {
                onCancel()
            }}
        >
            <DialogTitle>Potvrdi brisanje</DialogTitle>
            <DialogContent>
                Da li ste sigurni da zelite da obrisete ovu belešku?
            </DialogContent>
            <DialogActions>
                <Button onClick={onConfirm} variant={`contained`}>
                    Da, obrisi
                </Button>
                <Button onClick={onCancel}>Ne, odustani</Button>
            </DialogActions>
        </Dialog>
    )
}
