import { Button, Paper, Stack, Typography } from '@mui/material'
import { KeyboardArrowLeft } from '@mui/icons-material'
import NextLink from 'next/link'

export const QuizzSummary = ({ quizz }) => {
    const timeNeededToFinish =
        new Date(quizz.completed_at) - new Date(quizz.created_at)
    const minutesNeededToFinish = Math.floor(
        timeNeededToFinish / 60000
    ).toLocaleString(undefined, {
        maximumFractionDigits: 0,
        maximumSignificantDigits: 3,
    })
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
                        {quizz.questions.filter((a) => a.answered_correctly).length}
                    </Typography>
                </Stack>
            </Paper>
        </>
    )
}
