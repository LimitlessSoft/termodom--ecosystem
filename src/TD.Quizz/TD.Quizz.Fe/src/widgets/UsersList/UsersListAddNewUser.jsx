'use client'
import { handleResponse } from '@/helpers/responseHelpers'
import { AddCircle } from '@mui/icons-material'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    IconButton,
    Stack,
    TextField,
} from '@mui/material'
import React, { useState } from 'react'
import { toast } from 'react-toastify'

export default function UsersListAddNewUser({ onCreate }) {
    const [isOpen, setIsOpen] = useState(false)
    const [data, setData] = useState({
        username: '',
        password: '',
    })

    function handleOpenDialog() {
        setIsOpen(true)
    }

    function handleCloseDialog() {
        setIsOpen(false)
    }

    function handleChangePassword(e) {
        setData((prev) => ({
            ...prev,
            password: e.target.value,
        }))
    }

    function handleChangeUsername(e) {
        setData((prev) => ({
            ...prev,
            username: e.target.value,
        }))
    }

    function handleCreateUser() {
        fetch(`/api/admin-users`, {
            method: `POST`,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        }).then(async (response) => {
            handleResponse(response, (responseData) => {
                toast.success(`Korisnik ${responseData.username} je kreiran.`)
                setData({
                    username: '',
                    password: '',
                })
                responseData.type = responseData.isAdmin ? 'admin' : 'user'
                onCreate(responseData)
            }).finally(() => {
                handleCloseDialog()
            })
        })
    }

    return (
        <>
            <Dialog open={isOpen} onClose={handleCloseDialog}>
                <DialogTitle>Novi korisnik</DialogTitle>
                <DialogContent>
                    <Stack py={2} gap={2}>
                        <TextField
                            value={data.username}
                            onChange={handleChangeUsername}
                            label={`Korisnicko ime`}
                            sx={{ width: 300 }}
                        />
                        <TextField
                            value={data.password}
                            onChange={handleChangePassword}
                            type={`password`}
                            label={`Lozinka`}
                            sx={{ width: 300 }}
                        />
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleCreateUser} variant={`contained`}>
                        Kreiraj
                    </Button>
                    <Button onClick={handleCloseDialog}>Otkazi</Button>
                </DialogActions>
            </Dialog>
            <IconButton onClick={handleOpenDialog}>
                <AddCircle color={`primary`} />
            </IconButton>
        </>
    )
}
