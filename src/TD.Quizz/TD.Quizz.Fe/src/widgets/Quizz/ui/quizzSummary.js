import { Button, Paper, Stack, Typography } from '@mui/material'
import { KeyboardArrowLeft } from '@mui/icons-material'
import NextLink from 'next/link'

const getTimeNeededToFinish = (quizz) => {
    if (quizz.completed_at === null)
        throw new Error('Quizz is not completed yet')
    if (quizz.created_at === null)
        throw new Error('Quizz does not have creation date')
    if (quizz.completed_at < quizz.created_at)
        throw new Error('Quizz completion date is before creation date')
    return new Date(quizz.completed_at) - new Date(quizz.created_at)
}

const getMinutesToFinish = (timeInMs) => {
    if (!timeInMs) throw new Error('Time is required')
    if (timeInMs < 0) throw new Error('Time cannot be negative')
    return Math.floor(timeInMs / 60000).toLocaleString(undefined, {
        maximumFractionDigits: 0,
        maximumSignificantDigits: 3,
    })
}
export const QuizzSummary = ({ quizz }) => {
    const timeNeededToFinish = getTimeNeededToFinish(quizz)
    const minutesNeededToFinish = getMinutesToFinish(timeNeededToFinish)
    const probaColor = `#faaf7a`
    const ocenjivanjeColor = `rgba(0, 255, 8, 0.4)`
    return (
        <>
            <Stack direction={`row`} spacing={2}>
                <Button
                    startIcon={<KeyboardArrowLeft />}
                    variant={`contained`}
                    href={'/'}
                    LinkComponent={NextLink}
                >
                    Povratak
                </Button>
            </Stack>
            <Paper
                sx={{
                    p: 2,
                    borderRadius: 2,
                    backgroundColor:
                        quizz.type === `proba` ? probaColor : ocenjivanjeColor,
                }}
            >
                <Stack spacing={1}>
                    <Typography variant={`subtitle2`}>{quizz.id}</Typography>
                    <Typography variant={`h6`} fontWeight={`bold`}>
                        {quizz.quizzSchemaName}
                    </Typography>
                    <Typography>Tip: {quizz.type}</Typography>
                    <Typography>Korisnik: {quizz.user || `N/A`}</Typography>
                    <Typography>
                        Vreme: {minutesNeededToFinish} minuta
                    </Typography>
                    <Typography>
                        Broj pitanja: {quizz.questions.length}
                    </Typography>
                    <Typography>
                        Broj tacnih odgovora:{' '}
                        {
                            quizz.questions.filter((a) => a.answered_correctly)
                                .length
                        }
                    </Typography>
                </Stack>
            </Paper>
        </>
    )
}
