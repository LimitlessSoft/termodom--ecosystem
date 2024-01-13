import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from "@mui/material"

interface IAzuriranjeCenaPovezanRobaIdDialogProps {
    isOpen: boolean,
    handleClose: (nastaviAkciju: boolean) => void
}

export const AzuriranjeCenaPovezanRobaIdDialog = (props: IAzuriranjeCenaPovezanRobaIdDialogProps): JSX.Element => {
    return (
        <Dialog
            open={props.isOpen}
            onClose={() => props.handleClose(false)}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description">
                <DialogTitle id="alert-dialog-title">
                    Izmeni povezan RobaId
                </DialogTitle>
                <DialogContent>
                    <DialogContentText id="alert-dialog-description">
                        
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => {
                        props.handleClose(true)
                    }}>Ažuriraj RobaId!</Button>
                    <Button onClick={() => props.handleClose(false)}>Odustani!</Button>
                </DialogActions>
        </Dialog>
    )
}