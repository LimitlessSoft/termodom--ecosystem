import { Box, IconButton, Stack, Typography } from '@mui/material'
import { FileOpen } from '@mui/icons-material'
import moment from 'moment'
import NextLink from 'next/link'

export const QuizzResultItem = ({
    index,
    id,
    title,
    user,
    points,
    maxPoints,
    completedAt,
}) => {
    const bgColor = index % 2 === 0 ? `#f0f0f0` : `#e0e0e0`
    return (
        <Stack
            width={300}
            sx={{ backgroundColor: bgColor, py: 1, px: 2 }}
            direction={`row`}
            justifyContent={`space-between`}
        >
            <Box>
                <Typography variant={`subtitle1`}>
                    {title} - {user}
                </Typography>
                <Typography color={`#888`}>
                    {points} / {maxPoints}
                </Typography>
                <Typography color={`#888`}>
                    {moment(completedAt).format('DD.MM.YYYY HH:mm:ss')}
                </Typography>
            </Box>
            <Stack direction={`row`} spacing={1} alignItems={`center`}>
                <IconButton
                    href={`/${id}`}
                    LinkComponent={NextLink}
                    target={`_blank`}
                >
                    <FileOpen />
                </IconButton>
            </Stack>
        </Stack>
    )
}
