import { handleResponse } from '@/helpers/responseHelpers'
import { LockOpen } from '@mui/icons-material'
import {
    Box,
    CircularProgress,
    Divider,
    Grid,
    IconButton,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'

export default function UserActiveQuizzes({ userId }) {
    const [quizzes, setQuizzes] = useState()

    useEffect(() => {
        fetch(`/api/admin/users/${userId}/quizzes`).then((response) => {
            handleResponse(response, (data) => setQuizzes(data))
        })
    }, [userId])

    const handleUnlockQuizzSessions = (quizzId) => {
        fetch(`/api/admin/users/${userId}/quizzes/${quizzId}/unlock`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
        }).then((response) => {
            handleResponse(response, () => {
                setQuizzes((prev) =>
                    prev.map((quizz) =>
                        quizz.id === quizzId
                            ? {
                                  ...quizz,
                                  hasAtLeastOneLockedSession: false,
                              }
                            : quizz
                    )
                )
                toast.success('Uspešno ste otključali sesiju')
            })
        })
    }

    return (
        <Paper sx={{ p: 2 }}>
            <Typography variant={`h6`}>Lista aktivnih kvizova</Typography>
            <Divider sx={{ mb: 2 }} />
            <Stack spacing={1}>
                {!quizzes && <CircularProgress />}
                {quizzes && quizzes.length === 0 && (
                    <Typography>Nema aktivnih kvizova</Typography>
                )}
                {quizzes &&
                    quizzes.length > 0 &&
                    quizzes.map((quizz) => (
                        <Grid
                            key={quizz.id}
                            container
                            justifyContent={`space-between`}
                            alignItems={`center`}
                            gap={2}
                            sx={{
                                bgcolor: quizz.hasAtLeastOneLockedSession
                                    ? `#e0e0e0`
                                    : `#f0f0f0`,
                                borderRadius: 2,
                                p: 2,
                                borderColor: quizz.hasAtLeastOneLockedSession
                                    ? 'orange'
                                    : 'green',
                                borderWidth: 2,
                                borderStyle: 'solid',
                            }}
                        >
                            <Typography>{quizz.name}</Typography>
                            <Box>
                                {quizz.hasAtLeastOneLockedSession && (
                                    <IconButton
                                        size="small"
                                        color="success"
                                        onClick={() =>
                                            handleUnlockQuizzSessions(quizz.id)
                                        }
                                    >
                                        <LockOpen />
                                    </IconButton>
                                )}
                            </Box>
                        </Grid>
                    ))}
            </Stack>
        </Paper>
    )
}
