import { Box, Grid, Paper, Stack, Typography } from '@mui/material'
import React from 'react'

export default function QuizzSummaryQuestionsList({ questions }) {
    if (!questions || questions.length === 0)
        return <Typography>Nema pitanja za ovaj kviz.</Typography>

    return (
        <>
            {questions.map((q) => (
                <Paper key={q.id} sx={{ p: 2, width: 350, borderRadius: 2 }}>
                    <Stack spacing={1}>
                        <Grid container justifyContent="space-between">
                            <Typography>
                                {q.title}
                                {q.time_exceeded && ' (Istekao tajmer)'}
                            </Typography>
                            <Typography>
                                {q.achieved_points} / {q.maximum_points} poena
                            </Typography>
                        </Grid>
                        <Stack spacing={1}>
                            {q.answers.map((a, i) => (
                                <Box
                                    key={i}
                                    sx={{
                                        p: 2,
                                        borderRadius: 2,
                                        border: `2px solid ${
                                            q.correct_answer_indexes.includes(i)
                                                ? 'green'
                                                : 'red'
                                        }`,
                                        backgroundColor:
                                            q.answered_indexes.includes(i)
                                                ? '#e3f2fd'
                                                : '#e0e0e0',
                                    }}
                                >
                                    <Typography>{a}</Typography>
                                </Box>
                            ))}
                        </Stack>
                    </Stack>
                </Paper>
            ))}
        </>
    )
}
