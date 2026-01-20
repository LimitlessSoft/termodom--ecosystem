import { Box, Typography } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { CircularProgress } from '@mui/material'

const NemaPristupa = () => {
    const user = useUser()

    if (!user?.isLogged) {
        return <CircularProgress />
    }

    return (
        <Box
            display="flex"
            justifyContent="center"
            alignItems="center"
            minHeight="50vh"
        >
            <Typography variant="h5" color="text.secondary">
                Nemate pravo pristupa nijednom modulu!
            </Typography>
        </Box>
    )
}

export default NemaPristupa
