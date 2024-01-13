import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, Grid, TextField } from "@mui/material"
import { useState } from "react"

interface IAzuriranjeCenaPovezanRobaIdDialogProps {
    isOpen: boolean,
    handleClose: (value: number | null) => void,
    naziv: string,
    currentRobaId: number
}

export const AzuriranjeCenaPovezanRobaIdDialog = (props: IAzuriranjeCenaPovezanRobaIdDialogProps): JSX.Element => {

    const [value, setValue] = useState<number>(props.currentRobaId)

    return (
        <Dialog
            open={props.isOpen}
            onClose={() => props.handleClose(null)}
            aria-labelledby="alert-dialog-title">
                <DialogTitle id="alert-dialog-title">
                    Izmeni povezan RobaId proizvoda &quot;{props.naziv}&quot;
                </DialogTitle>
                <DialogContent>
                        <Grid container justifyContent={`center`}>
                            <TextField
                                required
                                type={`text`}
                                sx={{ m: 1 }}
                                label='RobaId'
                                defaultValue={props.currentRobaId}
                                onChange={(e) => {
                                    setValue(parseInt(e.target.value))
                                }}
                                variant={`outlined`} />
                        </Grid>
                </DialogContent>
                <DialogActions>
                    <Button onClick={() => {
                        props.handleClose(value)
                    }}>Ažuriraj RobaId!</Button>
                    <Button onClick={() => props.handleClose(null)}>Odustani!</Button>
                </DialogActions>
        </Dialog>
    )
}