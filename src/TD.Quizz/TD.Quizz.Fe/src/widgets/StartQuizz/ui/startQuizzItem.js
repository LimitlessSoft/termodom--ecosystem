'use client'
import { IconButton, Stack, Typography } from '@mui/material'
import { PlayArrow } from '@mui/icons-material'
import { StartQuizzDialog } from '@/widgets/StartQuizz/ui/startQuizzDialog'
import { useState } from 'react'
import { handleResponse } from '@/helpers/responseHelpers'
import { useRouter } from 'next/navigation'

export const StartQuizzItem = ({ title, backgroundColor, id }) => {
    const router = useRouter()
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    return (
        <Stack
            direction={`row`}
            alignItems={`center`}
            justifyContent={`space-between`}
            sx={{
                backgroundColor: backgroundColor,
                borderRadius: 1,
                px: 2,
                py: 0.25,
            }}
        >
            <Typography>{title}</Typography>
            <StartQuizzDialog
                onStart={(type) => {
                    fetch(`/api/quizz/start?type=${type}&schemaId=${id}`).then(
                        (response) => {
                            handleResponse(response, (data) => {
                                router.push(`/${data}`)
                            })
                        }
                    )
                    setIsDialogOpen(false)
                }}
                onCancel={() => {
                    setIsDialogOpen(false)
                }}
                isOpen={isDialogOpen}
            />
            <IconButton
                onClick={() => {
                    setIsDialogOpen(true)
                }}
            >
                <PlayArrow />
            </IconButton>
        </Stack>
    )
}
