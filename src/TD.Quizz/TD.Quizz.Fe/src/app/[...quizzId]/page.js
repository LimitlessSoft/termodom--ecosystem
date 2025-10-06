'use client'
import { useCallback, useEffect, useState } from 'react'
import { useParams } from 'next/navigation'
import { handleResponse } from '@/helpers/responseHelpers'
import {
    Box,
    CircularProgress,
    Grid,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import { QuizzQuestion, QuizzSummary } from '@/widgets'
import { useSession } from 'next-auth/react'
import QuizzSummaryLegend from '@/widgets/Quizz/ui/QuizzSummaryLegend'
import QuizzSummaryQuestionsList from '@/widgets/Quizz/ui/QuizzSummaryQuestionsList'

const QuizzPage = () => {
    const { quizzId } = useParams()
    const { data: session } = useSession()
    const [waitingNextQuestion, setWaitingNextQuestion] = useState(false)
    const [quizz, setQuizz] = useState(undefined)
    const [question, setQuestion] = useState(undefined)
    const fetchQuizz = useCallback(() => {
        setQuestion(undefined)
        setQuizz(undefined)
        fetch(`/api/quizz?sessionId=${quizzId}`).then((response) => {
            handleResponse(response, (data) => {
                if (data.id == quizzId) setQuizz(data)
                else setQuestion(data)
            }).finally(() => {
                setWaitingNextQuestion(false)
            })
        })
    }, [quizzId])

    useEffect(() => {
        if (!fetchQuizz) return
        fetchQuizz()
    }, [fetchQuizz])
    if (waitingNextQuestion) return <CircularProgress />
    if (!quizz && !question) return
    return (
        <Stack
            width={600}
            margin={`auto`}
            spacing={2}
            justifyContent={`center`}
            alignItems={`center`}
            minHeight={`100vh`}
        >
            {quizz && <QuizzSummary quizz={quizz} />}
            {quizz && session?.user?.isAdmin && (
                <>
                    <QuizzSummaryLegend />
                    <QuizzSummaryQuestionsList questions={quizz.questions} />
                </>
            )}
            {question && (
                <QuizzQuestion
                    question={question}
                    onSuccessSubmit={() => {
                        setWaitingNextQuestion(true)
                        fetchQuizz()
                    }}
                />
            )}
        </Stack>
    )
}
export default QuizzPage
