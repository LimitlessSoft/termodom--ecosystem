import { handleResponse } from '@/helpers/responseHelpers'
import { Edit } from '@mui/icons-material'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    IconButton,
    Stack,
    TextField,
    Tooltip,
} from '@mui/material'
import { useState } from 'react'
import { toast } from 'react-toastify'

export default function UserUpdateDialog({ data, onUpdate }) {
    const [isOpen, setIsOpen] = useState(false)
    const [username, setUsername] = useState(data.username)
    const [newPassword, setNewPassword] = useState('')

    const handleCloseDialog = () => setIsOpen(false)
    const handleOpenDialog = () => setIsOpen(true)

    const handleUpdateUser = () => {
        fetch(`/api/admin/users/${data.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password: newPassword }),
        }).then((response) =>
            handleResponse(response, () => {
                onUpdate(username)
                setNewPassword('')
                handleCloseDialog()
                toast.success('Uspešno ažuriran korisnik')
            })
        )
    }

    return (
        <>
            <Dialog open={isOpen} onClose={handleCloseDialog}>
                <DialogTitle>Izmena podataka korisnika</DialogTitle>
                <DialogContent>
                    <Stack spacing={2} py={2}>
                        <TextField
                            label={`Korisničko ime`}
                            value={username}
                            onChange={(e) => setUsername(e.target.value)}
                        />
                        <TextField
                            label={`Nova lozinka`}
                            type={`password`}
                            value={newPassword}
                            onChange={(e) => setNewPassword(e.target.value)}
                        />
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Button variant={`contained`} onClick={handleUpdateUser}>
                        Sačuvaj
                    </Button>
                    <Button variant={`outlined`} onClick={handleCloseDialog}>
                        Otkazi
                    </Button>
                </DialogActions>
            </Dialog>
            <Tooltip title={`Uredi korisnika`}>
                <IconButton color="primary" onClick={handleOpenDialog}>
                    <Edit />
                </IconButton>
            </Tooltip>
        </>
    )
}
