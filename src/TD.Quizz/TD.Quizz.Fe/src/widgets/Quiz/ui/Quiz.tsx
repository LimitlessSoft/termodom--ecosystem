import { Box, Button, Card, Container, Grid, Typography } from "@mui/material"
import { IQuizProps } from "../types/IQuizProps"
import { useState } from "react"
import { QuestionTitle } from "./QuestionTitle"
import { QuestionAnswer } from "./QuestionAnswer"
import { useAppSelector, useAppDispatch } from "@/app/hooks"

export const Quiz = (props: IQuizProps): JSX.Element => {
    
    const [questions, setQuestions] = useState(props.questions)
    const correctAnswers = useAppSelector((state) => state.userAnswers.correctAnswers)
    const incorrectAnswers = useAppSelector((state) => state.userAnswers.incorrectAnswers)
    const dispatch = useAppDispatch()

    return (
        <Container maxWidth={'sm'} sx={{ p: 2 }}>
            <Box>
                <Typography>Correct answers: { correctAnswers }</Typography>
                <Typography>Incorrect answers: { incorrectAnswers }</Typography>
                <Button onClick={() => {
                    dispatch({ type: 'usersAnswers/increaseCorrectAnswers', payload: 1 })
                }}>
                    hello
                </Button>
            </Box>
            {questions.map((question, index) => {
                return (
                    <Card
                        key={`question-${index}`}
                        sx={{ p: 2, m: 2 }}
                        variant={'outlined'}>
                        <Grid container sx={{ width: '100%', justifyContent: 'center' }} >
                            <QuestionTitle text={question.title} />
                            {question.answers.map((answer, index) => {
                                return (
                                    <QuestionAnswer
                                        key={`answer-${index}`}
                                        text={answer.text} />
                                )
                            })}
                        </Grid>
                    </Card>
                )
            })}
        </Container>
    )
}