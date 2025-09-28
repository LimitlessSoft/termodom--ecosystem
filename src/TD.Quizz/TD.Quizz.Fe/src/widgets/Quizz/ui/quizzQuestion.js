'use client'
import {
    Box,
    Button,
    CircularProgress,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import { useCallback, useEffect, useRef, useState } from 'react'
import { handleResponse } from '@/helpers/responseHelpers'
import { toast } from 'react-toastify'

export const QuizzQuestion = ({ question, onSuccessSubmit }) => {
    const [isSubmitting, setIsSubmitting] = useState(false)
    const [selectedAnswers, setSelectedAnswers] = useState([])
    const [correctAnswers, setCorrectAnswers] = useState([])
    const [remainingTime, setRemainingTime] = useState(0)
    const timerIntervalRef = useRef(null)

    const getTimeLeft = useCallback(() => {
        const startTime = new Date(question.startCountTime)
        const duration = question.duration
        const now = new Date()
        const elapsedSeconds = (now - startTime) / 1000
        return Math.max(0, duration - Math.floor(elapsedSeconds))
    }, [question.duration, question.startCountTime])
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

    const handleSubmitAnswers = (isTimeout) => {
        setIsSubmitting(true)
        if (
            isTimeout &&
            isTimeout === true &&
            selectedAnswers.length === question.requiredAnswers
        ) {
            toast.info(
                `Vreme je isteklo, potvrdicemo odgovore koje ste selektovali.`
            )
        }
        fetch(`/api/quizz`, {
            method: `POST`,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                sessionId: question.sessionId,
                questionId: question.id,
                answerIndexes:
                    selectedAnswers.length === question.requiredAnswers
                        ? selectedAnswers
                        : [-1],
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
    }

    const handleGoToNextQuestion = useCallback(() => {
        if (timerIntervalRef.current) clearInterval(timerIntervalRef.current)
        onSuccessSubmit()
        setSelectedAnswers([])
    }, [onSuccessSubmit])

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
        if (!getTimeLeft) return
        if (timerIntervalRef.current) clearInterval(timerIntervalRef.current)

        timerIntervalRef.current = setInterval(() => {
            const remainingTime = getTimeLeft()
            if (remainingTime <= 0) {
                handleSubmitAnswers(true)
            }
            setRemainingTime(remainingTime)
        }, 1000)
    }, [getTimeLeft, handleSubmitAnswers])

    if (!question || !remainingTime || remainingTime < 0)
        return <CircularProgress />
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
                        <Stack
                            direction={`row`}
                            justifyContent={`space-between`}
                        >
                            {question.requiredAnswers > 1 && (
                                <Typography textAlign={`start`}>
                                    Pitanje ima {question.requiredAnswers}{' '}
                                    odgovora
                                </Typography>
                            )}
                            <Typography ml="auto">
                                Preostalo vreme: {remainingTime}
                            </Typography>
                        </Stack>
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
                    {selectedAnswers.length > 0 && (
                        <Button
                            disabled={
                                isSubmitting || isCorrectNumberOfAnswersSelected
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
