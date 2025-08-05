'use client'
import { LinearProgress, Paper, Stack, Typography } from '@mui/material'
import { StartQuizzItem } from '@/widgets/StartQuizz/ui/startQuizzItem'
import { useEffect, useState } from 'react'
import { handleResponse } from '@/helpers/responseHelpers'

export const StartQuizz = () => {
    const bg1 = `#ddd`
    const bg2 = `#eee`

    const [quizzes, setQuizzes] = useState(undefined)

    useEffect(() => {
        fetch(`/api/quizz`).then((response) => {
            handleResponse(response, (data) => {
                setQuizzes(data)
            })
        })
    }, [])
    return (
        <Paper
            sx={{
                p: 2,
                minWidth: 300,
            }}
        >
            <Stack gap={1}>
                <Typography variant={`h5`}>Započni kviz</Typography>
                {!quizzes && <LinearProgress />}
                {quizzes && quizzes.length === 0 && (
                    <Typography variant={`body1`}>
                        Nema dostupnih kvizova
                    </Typography>
                )}
                {quizzes &&
                    quizzes.length > 0 &&
                    quizzes.map((quizz, index) => (
                        <StartQuizzItem
                            key={quizz.id}
                            id={quizz.id}
                            title={quizz.name}
                            backgroundColor={index % 2 === 0 ? bg1 : bg2}
                        />
                    ))}
            </Stack>
        </Paper>
    )
}
