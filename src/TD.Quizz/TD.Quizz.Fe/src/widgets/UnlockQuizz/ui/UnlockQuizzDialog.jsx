import { handleResponse } from '@/helpers/responseHelpers'
import { LockOpen } from '@mui/icons-material'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    IconButton,
    Tooltip,
    Typography,
} from '@mui/material'
import React, { useState } from 'react'
import { toast } from 'react-toastify'

export const UnlockQuizzDialog = ({
    quizzName,
    quizzId,
    hasAtLeastOneLockedSession,
    setLoading,
    disabled,
}) => {
    const [isOpen, setIsOpen] = useState(false)
    const [isUnlockable, setIsUnlockable] = useState(hasAtLeastOneLockedSession)

    const handleOpenDialog = () => setIsOpen(true)
    const handleCloseDialog = () => setIsOpen(false)

    const handleUnlockSessionForAllUser = () => {
        setLoading(true)
        fetch(`/api/admin-quiz/unlock-all`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                schemaId: quizzId,
            }),
        })
            .then((response) => {
                handleResponse(response, () => {
                    toast.success(
                        `Sesija za kviz "${quizzName}" je uspešno otključana svim korisnicima.`
                    )
                    setIsUnlockable(false)
                })
            })
            .finally(() => {
                handleCloseDialog()
                setLoading(false)
            })
    }

    return (
        <>
            <Dialog open={isOpen} onClose={handleCloseDialog} maxWidth={`md`}>
                <DialogTitle>
                    Otključavanje sesije svim korisnicima za "{quizzName}" kviz
                </DialogTitle>
                <DialogContent>
                    <Typography>
                        Da li ste sigurni da želite da otključate svim
                        korisnicima ovu sesiju?
                    </Typography>
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={handleUnlockSessionForAllUser}
                        variant={`contained`}
                    >
                        Da
                    </Button>
                    <Button onClick={handleCloseDialog} variant={`outlined`}>
                        Ne
                    </Button>
                </DialogActions>
            </Dialog>
            {isUnlockable && (
                <Tooltip title={`Otključaj sve ocenjivanje sesije`}>
                    <IconButton onClick={handleOpenDialog} disabled={disabled}>
                        <LockOpen />
                    </IconButton>
                </Tooltip>
            )}
        </>
    )
}
