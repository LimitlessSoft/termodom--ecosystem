'use client'
import { Box, Button, Grid, Paper, Stack, Typography } from '@mui/material'
import { useCallback, useEffect, useState } from 'react'
import { handleResponse } from '@/helpers/responseHelpers'

export const QuizzQuestion = ({ question, onSuccessSubmit }) => {
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [selectedAnswers, setSelectedAnswers] = useState([])
    const [correctAnswers, setCorrectAnswers] = useState([])
    const [timeRemaining, setTimeRemaining] = useState(question.duration)

    const handleGoToNextQuestion = useCallback(() => {
        onSuccessSubmit()
        setSelectedAnswers([])
    }, [onSuccessSubmit])

    const handleSubmitAnswers = useCallback(() => {
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
                    if (data?.correctAnswers) {
                        setCorrectAnswers(data.correctAnswers)
                        return
                    }
                    handleGoToNextQuestion()
                })
            })
            .finally(() => {
                setIsSubmitting(false)
            })
    }, [question, selectedAnswers, handleGoToNextQuestion])

    const removeSelectionFromSelectedAnswer = (index) => {
        setSelectedAnswers((prev) =>
            prev.filter((selectedAnswer) => selectedAnswer != index)
        )
    }
    const addSelectionToSelectedAnswer = (index) => {
        setSelectedAnswers((prev) => [...prev, index])
    }
    const toggleAnswerSelection = (index) => {
        if (selectedAnswers.includes(index)) {
            removeSelectionFromSelectedAnswer(index)
        } else {
            addSelectionToSelectedAnswer(index)
        }
    }

    const hasCorrectAnswers = correctAnswers.length > 0

    const getAnswerBorderColor = (index) => {
        const isCorrect = correctAnswers.includes(index)
        const isSelected = selectedAnswers.includes(index)

        let borderColor

        if (!hasCorrectAnswers && isSelected) {
            borderColor = '#1976d2'
        }

        if (!hasCorrectAnswers && !isSelected) {
            borderColor = '#ccc'
        }

        if (hasCorrectAnswers && isCorrect) {
            borderColor = 'green'
        }

        if (hasCorrectAnswers && !isCorrect) {
            borderColor = 'red'
        }

        return borderColor
    }

    const isCorrectNumberOfAnswersSelected =
        selectedAnswers.length !== question.requiredAnswers

    useEffect(() => {
        const interval = setInterval(() => {
            setTimeRemaining((prev) => {
                const next = prev - 1
                if (next <= 0) {
                    clearInterval(interval)
                    handleSubmitAnswers()
                    return 0
                }
                return next
            })
        }, 1000)

        return () => clearTimeout(interval)
    }, [handleSubmitAnswers])

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
                        {question.answeredCount}/{question.totalCount}
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
                        <Grid container justifyContent="space-between">
                            {question.requiredAnswers > 1 && (
                                <Typography textAlign={`start`}>
                                    Pitanje ima {question.requiredAnswers}{' '}
                                    odgovora
                                </Typography>
                            )}
                            <Typography ml="auto">{timeRemaining} s</Typography>
                        </Grid>
                        {question.answers.map((answer, index) => (
                            <Paper
                                onClick={() => {
                                    if (hasCorrectAnswers) return
                                    toggleAnswerSelection(index)
                                }}
                                key={index}
                                sx={{
                                    cursor: isSubmitting
                                        ? `loading`
                                        : hasCorrectAnswers
                                        ? 'not-allowed'
                                        : `pointer`,
                                    border: `2px solid ${getAnswerBorderColor(
                                        index
                                    )}`,
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
                    {isCorrectNumberOfAnswersSelected && !hasCorrectAnswers && (
                        <Typography color={`red`}>
                            Molimo odaberite tačno {question.requiredAnswers}{' '}
                            odgovora
                        </Typography>
                    )}
                    {(selectedAnswers.length > 0 || hasCorrectAnswers) && (
                        <Button
                            disabled={
                                isSubmitting ||
                                (isCorrectNumberOfAnswersSelected &&
                                    !hasCorrectAnswers)
                            }
                            variant={`contained`}
                            onClick={
                                hasCorrectAnswers
                                    ? handleGoToNextQuestion
                                    : handleSubmitAnswers
                            }
                        >
                            {hasCorrectAnswers ? 'Dalje' : 'Potvrdi odgovor'}
                        </Button>
                    )}
                </Stack>
            </Paper>
        </>
    )
}
