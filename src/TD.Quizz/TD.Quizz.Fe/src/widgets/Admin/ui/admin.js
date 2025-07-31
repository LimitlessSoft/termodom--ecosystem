import { Stack, Typography } from '@mui/material'
import { QuizzList } from '@/widgets/QuizzList/ui/quizzList'
import { QuizzResults } from '@/widgets/QuizzResults/ui/quizzResults'

export const Admin = () => {
    return (
        <Stack alignItems={`center`} justifyContent={`center`} sx={{ minHeight: `100vh` }} spacing={4}>
            <Typography variant={`h5`}>Admin panel</Typography>
            <QuizzList />
            <QuizzResults />
        </Stack>
    )
}