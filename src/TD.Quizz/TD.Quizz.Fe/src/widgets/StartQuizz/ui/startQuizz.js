import { Paper, Stack, Typography } from '@mui/material'
import { StartQuizzItem } from '@/widgets/StartQuizz/ui/startQuizzItem'

export const StartQuizz = () => {
    const bg1 = `#ddd`
    const bg2 = `#eee`
    return (
        <Paper sx={{
            p: 2,
            minWidth: 300
        }}>
            <Stack gap={1}>
                <Typography variant={`h5`}>
                    Započni kviz
                </Typography>
                <StartQuizzItem title={`Kviz 1`} backgroundColor={bg1} />
                <StartQuizzItem title={`Kviz 2`} backgroundColor={bg2} />
                <StartQuizzItem title={`Kviz 3`} backgroundColor={bg1} />
                <StartQuizzItem title={`Kviz 4`} backgroundColor={bg2} />
            </Stack>
        </Paper>
    )
}