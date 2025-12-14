import { Button, Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material'

// Generic confirmation dialog used across the project (MUI)
// Extended to support optional custom title and message while keeping backward compatibility
export const ConfirmDialog = ({
    isOpen,
    onCancel,
    onConfirm,
    title = 'Da li ste sigurni?',
    message,
}) => {
    return (
        <Dialog open={isOpen} onClose={onCancel}>
            <DialogTitle>{title}</DialogTitle>
            {message && <DialogContent>{message}</DialogContent>}
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
