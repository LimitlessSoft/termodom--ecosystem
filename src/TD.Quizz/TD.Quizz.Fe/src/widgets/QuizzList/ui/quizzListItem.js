import { Box, IconButton, Stack, Typography } from '@mui/material'
import { Edit } from '@mui/icons-material'
import NextLink from 'next/link'
import { useState } from 'react'
import { UnlockQuizzDialog } from '@/widgets/UnlockQuizz/ui/UnlockQuizzDialog'
import QuizzListItemAdditionalActionsMenu from './QuizzListItemAdditionalActionsMenu'

export const QuizzListItem = ({ index, data }) => {
    const [loading, setLoading] = useState(false)
    const bgColor = index % 2 === 0 ? `#f0f0f0` : `#e0e0e0`
    const activeColor = `rgba(26, 149, 29, 0.4)`
    const bgGradient = `linear-gradient(45deg, ${activeColor}, ${bgColor}, ${bgColor}, ${activeColor})`
    return (
        <Stack
            width={300}
            sx={{ background: bgGradient, py: 1, px: 2 }}
            direction={`row`}
            justifyContent={`space-between`}
            boxShadow={`0 1px 2px ${activeColor}`}
        >
            <Box>
                <Typography variant={`subtitle1`}>{data.name}</Typography>
                <Typography color={`#888`}>
                    {data.quizz_questions_count} pitanja
                </Typography>
            </Box>
            <Stack
                direction={`row`}
                justifyContent={`center`}
                alignItems={`center`}
            >
                <IconButton
                    disabled={loading}
                    href={`/admin/quizz-edit/${data.id}`}
                    LinkComponent={NextLink}
                >
                    <Edit />
                </IconButton>
                <UnlockQuizzDialog
                    hasAtLeastOneLockedSession={data.hasAtLeastOneLockedSession}
                    lockedSessionsUsernames={data.lockedSessionsUsernames}
                    quizzName={data.name}
                    quizzId={data.id}
                    disabled={loading}
                    setLoading={setLoading}
                />
                <QuizzListItemAdditionalActionsMenu
                    quizzId={data.id}
                    quizzName={data.name}
                />
            </Stack>
        </Stack>
    )
}
