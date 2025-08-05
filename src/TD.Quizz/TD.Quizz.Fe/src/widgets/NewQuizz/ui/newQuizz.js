'use client'
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
import { AddCircle } from '@mui/icons-material'
import { useState } from 'react'
import { toast } from 'react-toastify'
import { handleResponse } from '@/helpers/responseHelpers'

export const NewQuizz = () => {
    const [isOpen, setIsOpen] = useState(false)
    const [name, setName] = useState(``)
    const handleCreate = () => {
        setIsOpen(false)
        if (name.trim() === ``) {
            toast.error('Naziv kviza ne moze biti prazan')
            return
        }
        fetch(`/api/admin-quiz`, {
            method: `POST`,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ name }),
        }).then(async (response) => {
            handleResponse(response, (data) => {
                toast.success(`Kviz ${data.name} je kreiran`)
                setName(``)
                setIsOpen(false)
            })
        })
    }
    return (
        <>
            <Dialog open={isOpen}>
                <DialogTitle>Novi Kviz</DialogTitle>
                <DialogContent>
                    <Stack py={2}>
                        <TextField
                            value={name}
                            onChange={(e) => {
                                setName(e.target.value)
                            }}
                            sx={{ width: 300 }}
                            label={`Naziv kviza`}
                        />
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={() => {
                            handleCreate()
                        }}
                        variant={`contained`}
                    >
                        Kreiraj
                    </Button>
                    <Button
                        onClick={() => {
                            setIsOpen(false)
                        }}
                    >
                        Otkazi
                    </Button>
                </DialogActions>
            </Dialog>
            <IconButton
                onClick={() => {
                    setIsOpen(true)
                }}
            >
                <AddCircle color={`primary`} />
            </IconButton>
        </>
    )
}
