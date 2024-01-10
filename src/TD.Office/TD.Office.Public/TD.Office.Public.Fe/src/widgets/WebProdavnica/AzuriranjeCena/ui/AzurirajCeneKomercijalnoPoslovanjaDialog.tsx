import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from "@mui/material"

interface IAzurirajCeneKomercijalnoPoslovanjaDialogProps {
    isOpen: boolean,
    handleClose: (nastaviAkciju: boolean) => void
}

export const AzurirajCeneKomercijalnoPoslovanjaDialog = (props: IAzurirajCeneKomercijalnoPoslovanjaDialogProps): JSX.Element => {
    return (
        <Dialog
            open={props.isOpen}
            onClose={() => props.handleClose(false)}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description">
                <DialogTitle id="alert-dialog-title">
                    Potvrdi
                </DialogTitle>
                <DialogContent>
                    <DialogContentText id="alert-dialog-description">
                        Cene komercijalnog poslovanja su poslednji put ažurirane 12.12.2021.
                        Da li ste sigurni da želite ponovo da ažurirate cene komercijalnog poslovanja?
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => {
                        props.handleClose(true)
                    }}>Da, ažuriraj cene!</Button>
                    <Button onClick={() => props.handleClose(false)}>Odustajem!</Button>
                </DialogActions>
        </Dialog>
    )
}