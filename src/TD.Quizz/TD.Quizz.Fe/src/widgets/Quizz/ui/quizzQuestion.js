'use client'
import { Box, Button, Paper, Stack, Typography } from '@mui/material'
import { useState } from 'react'
import { handleResponse } from '@/helpers/responseHelpers'

export const QuizzQuestion = ({ question, onSuccessSubmit }) => {
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [selectedAnswers, setSelectedAnswers] = useState([])
    if (!question) return
    return (
        <>
            <Paper
                sx={{
                    p: 2,
                    borderRadius: 2,
                }}
            >
                <Stack
                    direction={`row`}
                    alignItems={`center`}
                    spacing={4}
                    justifyContent={`space-between`}
                >
                    <Typography variant={`h6`} fontWeight={`bold`}>
                        {question.quizzSchemaName}
                    </Typography>
                    <Typography variant={`h6`}>
                        {question.answeredCount + 1}/{question.totalCount}
                    </Typography>
                </Stack>
            </Paper>
            <Paper
                sx={{
                    p: 2,
                    borderRadius: 2,
                }}
            >
                <Stack alignItems={`center`} gap={2}>
                    <Typography variant={`h5`} fontWeight={`bold`}>
                        {question.title}
                    </Typography>
                    {question.image && (
                        <Box
                            sx={{
                                width: 600,
                                height: 300,
                                borderRadius: 2,
                                backgroundImage: `url(${question.image})`,
                                backgroundSize: `cover`,
                                backgroundPosition: `center`,
                            }}
                        />
                    )}
                    {question.text && (
                        <Typography variant={`body1`}>
                            {question.text}
                        </Typography>
                    )}
                    <Stack direction={`column`} spacing={2} width={`600px`}>
                        {question.requiredAnswers > 1 && (
                            <Typography textAlign={`start`}>
                                Pitanje ima {question.requiredAnswers} odgovora
                            </Typography>
                        )}
                        {question.answers.map((answer, index) => (
                            <Paper
                                onClick={() => {
                                    if (selectedAnswers.includes(index)) {
                                        setSelectedAnswers((prev) =>
                                            prev.filter(
                                                (selectedAnswer) =>
                                                    selectedAnswer != index
                                            )
                                        )
                                    } else {
                                        setSelectedAnswers((prev) => [
                                            ...prev,
                                            index,
                                        ])
                                    }
                                }}
                                key={index}
                                sx={{
                                    cursor: isSubmitting
                                        ? `loading`
                                        : `pointer`,
                                    border: `1px solid ${
                                        selectedAnswers.includes(index)
                                            ? `#1976d2`
                                            : `#ccc`
                                    }`,
                                    borderRadius: 2,
                                    p: 2,
                                    backgroundColor: `${
                                        isSubmitting
                                            ? `lightgrey`
                                            : selectedAnswers.includes(index)
                                            ? `#e3f2fd`
                                            : `#fff`
                                    }`,
                                }}
                            >
                                <Typography variant={`body1`}>
                                    {answer.text}
                                </Typography>
                            </Paper>
                        ))}
                    </Stack>
                    {selectedAnswers.length === question.requiredAnswers && (
                        <Button
                            disabled={isSubmitting}
                            variant={`contained`}
                            onClick={() => {
                                setIsSubmitting(true)
                                fetch(`/api/quizz`, {
                                    method: `POST`,
                                    headers: {
                                        'Content-Type': 'application/json',
                                    },
                                    body: JSON.stringify({
                                        sessionId: question.sessionId,
                                        questionId: question.id,
                                        answerIndexes: selectedAnswers,
                                    }),
                                })
                                    .then((response) => {
                                        handleResponse(response, (data) => {
                                            onSuccessSubmit()
                                        })
                                    })
                                    .finally(() => {
                                        setIsSubmitting(false)
                                        setSelectedAnswers([])
                                    })
                            }}
                        >
                            Potvrdi odgovor
                        </Button>
                    )}
                </Stack>
            </Paper>
        </>
    )
}
