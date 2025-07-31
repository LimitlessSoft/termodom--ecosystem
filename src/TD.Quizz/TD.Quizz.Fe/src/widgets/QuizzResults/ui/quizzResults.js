import { Divider, Paper, Stack, Typography } from '@mui/material'
import { QuizzResultItem } from '@/widgets/QuizzResults/ui/quizzResultItem'
import { NewQuizz } from '@/widgets/NewQuizz/ui/newQuizz'

export const QuizzResults = () => {
    return (
        <Paper>
            <Stack
                alignItems={`center`} 
                justifyContent={`center`}
                sx={{
                    borderRadius: 2,
                    overflow: `hidden`,
                    p: 2
                }}
            >
                <Typography variant={`h6`}>Rezultati</Typography>
                <Divider sx={{ width: `100%`, mb: 2 }} />
                <QuizzResultItem index={0} title={`Kviz 1`} user={`Marko`} points={8} maxPoints={10} />
                <QuizzResultItem index={1} title={`Kviz 2`} user={`Jelena`} points={9} maxPoints={10} />
                <QuizzResultItem index={2} title={`Kviz 3`} user={`Petar`} points={7} maxPoints={10} />
                <QuizzResultItem index={3} title={`Kviz 4`} user={`Ana`} points={10} maxPoints={10} />
            </Stack>
        </Paper>
    )
}