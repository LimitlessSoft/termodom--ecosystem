import { Button, Dialog, DialogActions, DialogTitle } from '@mui/material'

export const ConfirmDialog = ({ isOpen, onCancel, onConfirm }) => {
    return (
        <Dialog open={isOpen} onClose={onCancel}>
            <DialogTitle>Da li ste sigurni?</DialogTitle>
            <DialogActions>
                <Button
                    onClick={() => {
                        onCancel()
                    }}
                >
                    Ipak odustani...
                </Button>
                <Button
                    variant={`contained`}
                    onClick={() => {
                        onConfirm()
                    }}
                >
                    Jesam, nastavi!
                </Button>
            </DialogActions>
        </Dialog>
    )
}
