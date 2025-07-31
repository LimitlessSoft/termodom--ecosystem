import { Box, Grid, IconButton, Stack, Typography } from '@mui/material'
import { Edit } from '@mui/icons-material'

export const QuizzListItem = ({ index, title, nQuestions }) => {
    const bgColor = index % 2 === 0 ? `#f0f0f0` : `#e0e0e0`
    return (
        <Stack
            width={300}
            sx={{ backgroundColor: bgColor, py: 1, px: 2 }}
            direction={`row`}
            justifyContent={`space-between`}>
            <Box>
                <Typography variant={`subtitle1`}>{title}</Typography>
                <Typography color={`#888`}>{nQuestions} pitanja</Typography>
            </Box>
            <IconButton>
                <Edit />
            </IconButton>
        </Stack>
    )
}