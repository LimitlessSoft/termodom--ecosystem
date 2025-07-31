'use client'
import {
    CircularProgress,
    Divider,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import { QuizzListItem } from '@/widgets/QuizzList/ui/quizzListItem'
import { NewQuizz } from '@/widgets/NewQuizz/ui/newQuizz'
import { useEffect, useState } from 'react'
import { handleResponse } from '@/helpers/responseHelpers'

export const QuizzList = () => {
    const [quizzes, setQuizzes] = useState([])

    useEffect(() => {
        fetch('/api/admin-quiz').then(async (response) => {
            handleResponse(response, (data) => {
                setQuizzes(data)
            })
        })
    }, [])

    return (
        <Paper sx={{ p: 2 }}>
            <Stack
                alignItems={`center`}
                justifyContent={`center`}
                spacing={1}
                sx={{
                    borderRadius: 2,
                    overflow: `hidden`,
                }}
            >
                <Stack
                    direction={`row`}
                    justifyContent={`space-between`}
                    alignItems={`center`}
                    width={`100%`}
                >
                    <Typography variant={`h6`}>Lista kvizova</Typography>
                    <NewQuizz />
                </Stack>
                <Divider sx={{ width: `100%`, mb: 2 }} />
                {quizzes.map((quiz, index) => (
                    <QuizzListItem
                        key={quiz.id}
                        id={quiz.id}
                        index={index}
                        title={quiz.name}
                        nQuestions={quiz.quizz_question?.length || 0}
                        active={quiz.is_active}
                        onActiveChanged={(active) => {
                            setQuizzes((prev) =>
                                prev.map((q) =>
                                    q.id === quiz.id
                                        ? { ...q, is_active: active }
                                        : q
                                )
                            )
                        }}
                    />
                ))}
                {(!quizzes || quizzes.length === 0) && <CircularProgress />}
            </Stack>
        </Paper>
    )
}
