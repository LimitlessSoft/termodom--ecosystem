import colorConstants from '@/constants/colorConstants'
import { Box, Grid, Paper, Stack, Typography } from '@mui/material'
import React from 'react'

export default function QuizzSummaryQuestionsList({ questions }) {
    if (!questions || questions.length === 0)
        return <Typography>Nema pitanja za ovaj kviz.</Typography>

    return (
        <Stack spacing={2} direction="column" alignItems="stretch">
            {questions.map((q) => (
                <Paper key={q.id} sx={{ p: 2, minWidth: 400, borderRadius: 2 }}>
                    <Stack spacing={1}>
                        <Grid
                            container
                            justifyContent="space-between"
                            gap={4}
                            alignItems="center"
                        >
                            <Typography variant="h6" component="span">
                                {q.title}
                            </Typography>
                            <Typography>
                                {q.achieved_points} / {q.maximum_points} poena
                            </Typography>
                        </Grid>
                        {q.time_exceeded && (
                            <Typography>(Istekao tajmer)</Typography>
                        )}
                        {q.text && <Typography>{q.text}</Typography>}
                        {q.image && (
                            <Box
                                sx={{
                                    width: 600,
                                    height: 300,
                                    borderRadius: 2,
                                    backgroundImage: `url(${q.image})`,
                                    backgroundSize: `cover`,
                                    backgroundPosition: `center`,
                                }}
                            />
                        )}
                        <Stack spacing={1}>
                            {q.answers.map((a, i) => (
                                <Box
                                    key={i}
                                    sx={{
                                        p: 2,
                                        borderRadius: 2,
                                        border: `2px solid ${
                                            q.correct_answer_indexes.includes(i)
                                                ? colorConstants.CORRECT_ANSWER_BORDER_COLOR
                                                : colorConstants.INCORRECT_ANSWER_BORDER_COLOR
                                        }`,
                                        backgroundColor:
                                            q.answered_indexes.includes(i)
                                                ? colorConstants.USER_SELECTED_ANSWER_BACKGROUND_COLOR
                                                : colorConstants.NOT_SELECTED_ANSWER_BACKGROUND_COLOR,
                                    }}
                                >
                                    <Typography>{a}</Typography>
                                </Box>
                            ))}
                        </Stack>
                    </Stack>
                </Paper>
            ))}
        </Stack>
    )
}
