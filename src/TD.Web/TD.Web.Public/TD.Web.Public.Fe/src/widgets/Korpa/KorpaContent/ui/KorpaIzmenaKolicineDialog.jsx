import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Grid,
    TextField,
} from '@mui/material'
import { useState } from 'react'

export const KorpaIzmenaKolicineDialog = (props) => {
    const [value, setValue] = useState(props.currentKolicina)

    return (
        <Dialog
            open={props.isOpen}
            onClose={() => {
                props.handleClose()
            }}
        >
            <DialogTitle>Izmena količine</DialogTitle>

            <DialogContent>
                <Grid container justifyContent={`center`}>
                    <TextField
                        required
                        type={`text`}
                        sx={{ m: 1 }}
                        label="Nova količina"
                        defaultValue={value}
                        onChange={(e) => {
                            setValue(parseInt(e.target.value))
                        }}
                        variant={`outlined`}
                    />
                </Grid>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        props.handleClose(value)
                    }}
                >
                    Ažuriraj količinu
                </Button>
                <Button onClick={() => props.handleClose()}>Odustani</Button>
            </DialogActions>
        </Dialog>
    )
}
