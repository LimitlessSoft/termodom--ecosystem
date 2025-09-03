import { handleResponse } from '@/helpers/responseHelpers'
import { Clear, LockOpen } from '@mui/icons-material'
import {
    Box,
    CircularProgress,
    Divider,
    Grid,
    IconButton,
    Paper,
    Stack,
    Tooltip,
    Typography,
} from '@mui/material'
import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import UserActiveQuizzesNew from './UserActiveQuizzesNew'

export default function UserActiveQuizzes({ userId }) {
    const [quizzes, setQuizzes] = useState()
    const [loading, setLoading] = useState(false)

    const fetchUserActiveQuizzes = () => {
        fetch(`/api/admin/users/${userId}/quizzes`).then((response) => {
            handleResponse(response, (data) => setQuizzes(data))
        })
    }

    useEffect(() => {
        fetchUserActiveQuizzes()
    }, [userId])

    const handleUnlockQuizzSessions = (quizzId, quizzName) => {
        setLoading(true)
        fetch(`/api/admin/users/${userId}/quizzes/${quizzId}/unlock`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
        })
            .then((response) => {
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
                    toast.success(
                        `Uspešno ste korisniku otključali sesiju za kviz '${quizzName}'`
                    )
                })
            })
            .finally(() => setLoading(false))
    }

    const handleRemoveQuizzFromUser = (quizzId, quizzName) => {
        setLoading(true)
        fetch(`/api/admin/users/${userId}/quizzes/${quizzId}`, {
            method: 'DELETE',
        })
            .then((response) => {
                handleResponse(response, () => {
                    setQuizzes((prev) =>
                        prev.filter((quizz) => quizz.id !== quizzId)
                    )
                    toast.success(
                        `Uspešno ste uklonili korisniku kviz '${quizzName}'`
                    )
                })
            })
            .finally(() => setLoading(false))
    }

    return (
        <Paper sx={{ p: 2 }}>
            <Stack
                direction={`row`}
                alignItems={`center`}
                justifyContent={`space-between`}
                spacing={2}
            >
                <Typography variant={`h6`}>
                    Lista aktivnih kvizova KORISNIKA
                </Typography>
                <UserActiveQuizzesNew onAddNew={fetchUserActiveQuizzes} />
            </Stack>
            <Divider sx={{ mb: 2 }} />
            <Stack spacing={1}>
                {!quizzes && <CircularProgress />}
                {quizzes && quizzes.length === 0 && (
                    <Typography>Korisnik nema aktivnih kvizova</Typography>
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
                            <Stack
                                direction={`row`}
                                justifyContent={`center`}
                                alignItems={`center`}
                            >
                                {quizz.hasAtLeastOneLockedSession && (
                                    <Tooltip title="Otključaj korisniku ocenjivanje sesiju za kviz">
                                        <IconButton
                                            disabled={loading}
                                            size="small"
                                            color="success"
                                            onClick={() =>
                                                handleUnlockQuizzSessions(
                                                    quizz.id,
                                                    quizz.name
                                                )
                                            }
                                        >
                                            <LockOpen />
                                        </IconButton>
                                    </Tooltip>
                                )}
                                <Tooltip title="Ukloni kviz korisniku">
                                    <IconButton
                                        disabled={loading}
                                        onClick={() =>
                                            handleRemoveQuizzFromUser(
                                                quizz.id,
                                                quizz.name
                                            )
                                        }
                                    >
                                        <Clear />
                                    </IconButton>
                                </Tooltip>
                            </Stack>
                        </Grid>
                    ))}
            </Stack>
        </Paper>
    )
}
