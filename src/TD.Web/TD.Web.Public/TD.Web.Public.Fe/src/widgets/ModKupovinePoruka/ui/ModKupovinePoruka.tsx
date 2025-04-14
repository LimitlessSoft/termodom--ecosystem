import { useUser } from '@/app/hooks'
import {
    Button,
    Card,
    Stack,
    Typography,
    useMediaQuery,
    useTheme,
} from '@mui/material'
import NextLink from 'next/link'
import React from 'react'

export const ModKupovinePoruka = (): JSX.Element => {
    const user = useUser()
    const theme = useTheme()

    const isMobile = useMediaQuery(theme.breakpoints.down('sm'))

    return user == null || user.isLogged == true ? (
        <React.Fragment />
    ) : (
        <Card
            variant={`outlined`}
            sx={{
                py: { xs: 1, md: 2 },
                m: 2,
                backgroundColor: `rgb(220, 220, 220)`,
                display: 'flex',
                justifyContent: 'center',
            }}
        >
            <Stack
                direction={`row`}
                gap={1}
                sx={{
                    width: `max-content`,
                    alignItems: `center`,
                }}
            >
                <Typography
                    fontWeight={`bold`}
                    textAlign={`center`}
                    lineHeight={1}
                >
                    {isMobile
                        ? `Aktiv: Jednokratna`
                        : `Trenutna kupovina: Jednokratna`}
                </Typography>
                <Button
                    href="/logovanje"
                    component={NextLink}
                    sx={{
                        fontWeight: 'bold',
                        lineHeight: 1,
                    }}
                >
                    {isMobile ? `Idi na: profi` : `Prebaci se na profi!`}
                </Button>
            </Stack>
        </Card>
    )
}
