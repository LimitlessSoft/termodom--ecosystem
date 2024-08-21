import {
    Button,
    Dialog,
    DialogContent,
    DialogTitle,
    Grid,
    TextField,
    Typography,
} from '@mui/material'
import { IZaboravljenaLozinkaDialogProps } from '../models/IZaboravljenaLozinkaDialogProps'
import { useState } from 'react'
import { toast } from 'react-toastify'
import { handleApiError, webApi } from '@/api/webApi'

export const ZaboravljenaLozinkaDialog = (
    props: IZaboravljenaLozinkaDialogProps
): JSX.Element => {
    const [username, setUsername] = useState<string>(``)
    const [phoneNumber, setPhoneNumber] = useState<string>(``)
    const [isReset, setIsReset] = useState<boolean>(false)

    return (
        <Dialog
            open={props.isOpen}
            onClose={() => {
                props.handleClose()
            }}
        >
            <DialogTitle>Zaboravljena lozinka</DialogTitle>

            <DialogContent>
                <Grid container justifyContent={`center`}>
                    <Typography>
                        Ukoliko ste zaboravili lozinku, unesite korisničko ime i
                        broj mobilnog telefona sa kojim je nalog povezan. Nakon
                        klika na dugme &quot;Resetuj lozinku&quot;, dobićete SMS
                        sa novom lozinkom.
                    </Typography>
                    <Grid my={2} container justifyContent={`center`}>
                        <TextField
                            required
                            type={`text`}
                            sx={{ m: 1 }}
                            label="Korisničko ime"
                            onChange={(e) => {
                                setUsername(e.target.value)
                            }}
                            variant={`outlined`}
                        />
                        <TextField
                            required
                            type={`text`}
                            sx={{ m: 1 }}
                            label="Broj mobilnog telefona"
                            onChange={(e) => {
                                setPhoneNumber(e.target.value)
                            }}
                            variant={`outlined`}
                        />
                    </Grid>
                    <Button
                        disabled={isReset}
                        variant={`contained`}
                        sx={{ my: 3 }}
                        onClick={() => {
                            setIsReset(true)
                            webApi
                                .post('/reset-password', {
                                    username: username,
                                    mobile: phoneNumber,
                                })
                                .then(() => {
                                    toast.success(
                                        `Ukoliko korisnik postoji i povezan je sa unetim brojem telefona, kroz par minuta ćete dobiti SMS sa novom lozinkom.`
                                    )
                                })
                                .catch((err) => handleApiError(err))
                        }}
                    >
                        Resetuj lozinku
                    </Button>
                </Grid>
            </DialogContent>
        </Dialog>
    )
}
