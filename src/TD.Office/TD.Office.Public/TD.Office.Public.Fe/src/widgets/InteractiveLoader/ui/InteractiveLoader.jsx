import { Box, CircularProgress, Paper, Typography } from '@mui/material'
import { useEffect, useState } from 'react'
import Grid2 from '@mui/material/Unstable_Grid2'
import { InteractiveLoaderItemWrapper } from '../styled/InteractiveLoaderItemWrapper'

export const InteractiveLoader = ({ messages, dots = 5 }) => {
    const [message, setMessage] = useState(messages[0] ?? `Učitavanje`)
    const [nDots, setNDots] = useState(0)

    useEffect(() => {
        if (messages.length === 0) return
        let i = 0
        const interval = setInterval(() => {
            i = i === messages.length - 1 ? 0 : i + 1
            setMessage(messages[i])
        }, dots * 1000)
        return () => clearInterval(interval)
    }, [messages])

    useEffect(() => {
        const interval = setInterval(() => {
            setNDots((prev) => (prev === dots ? 1 : prev + 1))
        }, 1000)
        return () => clearInterval(interval)
    }, [dots])

    return (
        <Grid2 container>
            <Grid2>
                <InteractiveLoaderItemWrapper component={Box}>
                    <CircularProgress
                        color={`inherit`}
                        size={`1.2rem`}
                        sx={{
                            marginRight: `0.5rem`,
                        }}
                    />
                    <Typography component={`span`} fontSize={`1.2rem`}>
                        {message}
                        {`.`.repeat(nDots)}
                    </Typography>
                </InteractiveLoaderItemWrapper>
            </Grid2>
        </Grid2>
    )
}
