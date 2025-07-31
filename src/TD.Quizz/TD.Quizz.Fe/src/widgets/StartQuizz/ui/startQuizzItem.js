"use client"
import { IconButton, Stack, Typography } from '@mui/material'
import { PlayArrow } from '@mui/icons-material'
import { StartQuizzDialog } from '@/widgets/StartQuizz/ui/startQuizzDialog'
import { toast } from 'react-toastify'
import { useState } from 'react'

export const StartQuizzItem = ({ title, backgroundColor }) => {
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
                py: 0.25
            }}
        >
            <Typography>{title}</Typography>
            <StartQuizzDialog
                onStart={(type) => {
                    toast(type)
                    setIsDialogOpen(false)
                }}
                onCancel={() => { 
                    setIsDialogOpen(false)
                }}
                isOpen={isDialogOpen} />
            <IconButton onClick={() => {
                setIsDialogOpen(true)
            }}>
                <PlayArrow />
            </IconButton>
        </Stack>
    )
}