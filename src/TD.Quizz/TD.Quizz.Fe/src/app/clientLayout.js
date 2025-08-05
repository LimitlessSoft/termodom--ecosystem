'use client'
import { mainTheme } from '@/app/mainTheme'
import { Box, ThemeProvider } from '@mui/material'
import { ToastContainer } from 'react-toastify'

export const ClientLayout = ({ children }) => {
    return (
        <ThemeProvider theme={mainTheme}>
            <ToastContainer />
            <Box
                sx={{
                    backgroundColor: mainTheme.palette.background.default,
                    minHeight: '100vh',
                    minWidth: '100vw',
                    position: 'relative',
                    py: 4,
                }}
            >
                {children}
            </Box>
        </ThemeProvider>
    )
}
