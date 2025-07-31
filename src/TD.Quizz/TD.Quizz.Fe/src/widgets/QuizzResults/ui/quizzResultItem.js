import { Box, IconButton, Stack, Typography } from '@mui/material'
import { FileOpen } from '@mui/icons-material'

export const QuizzResultItem = ({ index, title, user, points, maxPoints}) => {
    const bgColor = index % 2 === 0 ? `#f0f0f0` : `#e0e0e0`
    return (
        <Stack
            width={300}
            sx={{ backgroundColor: bgColor, py: 1, px: 2 }}
            direction={`row`}
            justifyContent={`space-between`}>
            <Box>
                <Typography variant={`subtitle1`}>{title} - {user}</Typography>
                <Typography color={`#888`}>{points} / {maxPoints}</Typography>
            </Box>
            <IconButton>
                <FileOpen />
            </IconButton>
        </Stack>
    )
}