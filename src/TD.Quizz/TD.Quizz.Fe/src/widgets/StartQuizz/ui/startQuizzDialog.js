'use client'
import { useState } from 'react'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    MenuItem,
    Select,
    Stack,
    Typography,
} from '@mui/material'

export const StartQuizzDialog = ({ isOpen, onCancel, onStart }) => {
    const [quizzType, setQuizzType] = useState('proba')
    return (
        <Dialog
            open={isOpen}
            onClose={() => {
                onCancel()
            }}
            maxWidth={`md`}
        >
            <DialogTitle>Započni kviz</DialogTitle>
            <DialogContent>
                <Stack spacing={1}>
                    <Select
                        variant={`outlined`}
                        value={quizzType}
                        onChange={(e) => {
                            setQuizzType(e.target.value)
                        }}
                    >
                        <MenuItem value={`proba`}>Proba</MenuItem>
                        <MenuItem value={`ocenjivanje`}>Ocenjivanje</MenuItem>
                    </Select>
                    {quizzType === `proba` && (
                        <Typography>
                            Ovaj kviz je namenjen za vežbanje i mozete ga
                            ponoviti vise puta.
                        </Typography>
                    )}
                    {quizzType === `ocenjivanje` && (
                        <Typography color={`error`}>
                            Ovaj kviz mozete pokrenuti samo jednom.
                        </Typography>
                    )}
                </Stack>
            </DialogContent>
            <DialogActions>
                <Button
                    variant={`contained`}
                    onClick={() => {
                        onStart(quizzType)
                    }}
                >
                    Zapocni
                </Button>
                <Button
                    variant={`outlined`}
                    onClick={() => {
                        onCancel()
                    }}
                >
                    Odustani
                </Button>
            </DialogActions>
        </Dialog>
    )
}
