import Grid2 from '@mui/material/Unstable_Grid2'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
    Typography,
} from '@mui/material'
import { useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { toast } from 'react-toastify'

export const KorisniciNovaLozinka = (props: any) => {
    const [updating, setUpdating] = useState<boolean>(false)
    const [password, setPassword] = useState<string>('')

    return (
        <Grid2>
            <Dialog
                open={props.isOpen}
                onClose={() => {
                    if (!updating) props.onClose()
                }}
            >
                <DialogTitle>
                    <Typography>Postavi novu lozinku</Typography>
                </DialogTitle>
                <DialogContent>
                    <TextField
                        disabled={updating}
                        label="Nova lozinka"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </DialogContent>
                <DialogActions>
                    <Button
                        disabled={updating}
                        onClick={() => {
                            setUpdating(true)
                            officeApi
                                .put(`/users/${props.id}/password`, {
                                    password,
                                })
                                .then(() => {
                                    toast.success('Lozinka uspešno postavljena')
                                    props.onClose()
                                })
                                .catch(handleApiError)
                                .finally(() => setUpdating(false))
                        }}
                    >
                        Postavi
                    </Button>
                    <Button disabled={updating} onClick={props.onClose}>
                        Odustani
                    </Button>
                </DialogActions>
            </Dialog>
        </Grid2>
    )
}
