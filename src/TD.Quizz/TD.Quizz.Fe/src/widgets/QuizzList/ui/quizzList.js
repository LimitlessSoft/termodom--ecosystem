import { Divider, Paper, Stack, Typography } from '@mui/material'
import { QuizzListItem } from '@/widgets/QuizzList/ui/quizzListItem'
import { NewQuizz } from '@/widgets/NewQuizz/ui/newQuizz'

export const QuizzList = () => {
    return (
        <Paper sx={{ p: 2 }}>
            <Stack
                alignItems={`center`} 
                justifyContent={`center`}
                sx={{
                    borderRadius: 2,
                    overflow: `hidden`
                }}
            >
                <Stack direction={`row`} justifyContent={`space-between`} alignItems={`center`} width={`100%`}>
                    <Typography variant={`h6`}>Lista kvizova</Typography>
                    <NewQuizz />
                </Stack>
                <Divider sx={{ width: `100%`, mb: 2 }} />
                <QuizzListItem index={0} title={`Kviz 1`} nQuestions={10} />
                <QuizzListItem index={1} title={`Kviz 2`} nQuestions={10} />
                <QuizzListItem index={2} title={`Kviz 3`} nQuestions={10} />
            </Stack>
        </Paper>
    )
}