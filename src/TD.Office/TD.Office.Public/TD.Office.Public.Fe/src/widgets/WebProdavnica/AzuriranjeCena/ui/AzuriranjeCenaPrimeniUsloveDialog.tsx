import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography } from "@mui/material"

interface IAzuriranjeCenaPrimeniUsloveDialogProps {
    isOpen: boolean,
    handleClose: (nastaviAkciju: boolean) => void
}

export const AzuriranjeCenaPrimeniUsloveDialog = (props: IAzuriranjeCenaPrimeniUsloveDialogProps): JSX.Element => {
    return (
        <Dialog
            open={props.isOpen}
            onClose={() => props.handleClose(false)}>
            <DialogTitle>
                Primeni uslove formiranja Min Web Osnove
            </DialogTitle>
            <DialogContent>
                <Typography>
                    Da li ste sigurni da želite da primenite uslove formiranja Min Web Osnove?
                </Typography>
            </DialogContent>
            <DialogActions>
                <Button onClick={() => {
                    props.handleClose(true)
                }}>Primeni uslove!</Button>
                <Button onClick={() => props.handleClose(false)}>Odustani!</Button>
            </DialogActions>
        </Dialog>
    )
}