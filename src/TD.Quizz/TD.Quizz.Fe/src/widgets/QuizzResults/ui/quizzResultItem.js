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
    createdAt,
}) => {
    const bgColor = index % 2 === 0 ? `#f0f0f0` : `#e0e0e0`
    const timeNeededToFinish = new Date(completedAt) - new Date(createdAt)
    const minutesNeededToFinish = Math.floor(
        timeNeededToFinish / 60000
    ).toLocaleString(undefined, {
        maximumFractionDigits: 0,
        maximumSignificantDigits: 3,
    })
    const blendColor = `#777`
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
                <Typography color={blendColor}>
                    {points} / {maxPoints}
                </Typography>
                <Typography color={blendColor}>
                    {moment(completedAt).format('DD.MM.YYYY HH:mm:ss')}
                </Typography>
                <Typography color={blendColor}>
                    Vreme: {minutesNeededToFinish} minuta
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
