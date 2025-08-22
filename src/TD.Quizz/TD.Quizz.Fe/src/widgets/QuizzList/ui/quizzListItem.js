import { Box, IconButton, Stack, Typography } from '@mui/material'
import { Edit, Visibility, VisibilityOff } from '@mui/icons-material'
import NextLink from 'next/link'
import { handleResponse } from '@/helpers/responseHelpers'
import { toast } from 'react-toastify'
import { useState } from 'react'

export const QuizzListItem = ({
    index,
    title,
    nQuestions,
    id,
    active,
    onActiveChanged,
}) => {
    const [isUpdating, setIsUpdating] = useState(false)
    const bgColor = index % 2 === 0 ? `#f0f0f0` : `#e0e0e0`
    const notActiveColor = `rgba(244, 67, 54, 0.44)`
    const activeColor = `rgba(26, 149, 29, 0.4)`
    const bgGradient = `linear-gradient(45deg, ${
        active ? activeColor : notActiveColor
    }, ${bgColor}, ${bgColor}, ${active ? activeColor : notActiveColor})`
    return (
        <Stack
            width={300}
            sx={{ background: bgGradient, py: 1, px: 2 }}
            direction={`row`}
            justifyContent={`space-between`}
            boxShadow={`0 1px 2px ${active ? activeColor : notActiveColor}`}
        >
            <Box>
                <Typography variant={`subtitle1`}>{title}</Typography>
                <Typography color={`#888`}>{nQuestions} pitanja</Typography>
            </Box>
            <Stack
                direction={`row`}
                justifyContent={`center`}
                alignItems={`center`}
            >
                {!active && (
                    <IconButton
                        disabled={isUpdating}
                        onClick={() => {
                            setIsUpdating(true)
                            fetch(`/api/admin-quiz/set-visible`, {
                                method: `PUT`,
                                headers: {
                                    'Content-Type': 'application/json',
                                },
                                body: JSON.stringify({ id, visible: true }),
                            })
                                .then(async (response) => {
                                    handleResponse(response, (data) => {
                                        onActiveChanged(true)
                                        toast.success(
                                            `Kviz ${title} je aktiviran`
                                        )
                                    })
                                })
                                .finally(() => {
                                    setIsUpdating(false)
                                })
                        }}
                    >
                        <Visibility />
                    </IconButton>
                )}
                {active && (
                    <IconButton
                        disabled={isUpdating}
                        onClick={() => {
                            setIsUpdating(true)
                            fetch(`/api/admin-quiz/set-visible`, {
                                method: `PUT`,
                                headers: {
                                    'Content-Type': 'application/json',
                                },
                                body: JSON.stringify({ id, visible: false }),
                            })
                                .then(async (response) => {
                                    handleResponse(response, (data) => {
                                        onActiveChanged(false)
                                        toast.success(
                                            `Kviz ${title} je deaktiviran`
                                        )
                                    })
                                })
                                .finally(() => {
                                    setIsUpdating(false)
                                })
                        }}
                    >
                        <VisibilityOff />
                    </IconButton>
                )}
                <IconButton
                    disabled={isUpdating}
                    href={`/admin/quizz-edit/${id}`}
                    LinkComponent={NextLink}
                >
                    <Edit />
                </IconButton>
            </Stack>
        </Stack>
    )
}
