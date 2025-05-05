import { Box, CircularProgress } from '@mui/material'
import { useState } from 'react'
import { useZOverlay } from '@/zStore/zOverlay'

export const OverlayContainer = () => {
    const zOverlay = useZOverlay()
    if (!zOverlay.displayed) return null
    return (
        <Box
            sx={{
                position: 'fixed',
                top: 0,
                left: 0,
                width: '100vw',
                height: '100vh',
                backgroundColor: 'rgba(0, 0, 0, 0.9)',
                zIndex: 999999,
            }}
        >
            <Box
                sx={{
                    px: 2,
                    py: 1,
                    position: 'absolute',
                    top: '50%',
                    left: '50%',
                    transform: 'translate(-50%, -50%)',
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                <CircularProgress />
            </Box>
        </Box>
    )
}
