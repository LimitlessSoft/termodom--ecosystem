import { useUser } from '@/app/hooks'
import { Button, Card, Stack, Typography } from '@mui/material'
import NextLink from 'next/link'
import React from 'react'

export const ModKupovinePoruka = (): JSX.Element => {
    const user = useUser()

    return user == null || user.isLogged == true ? (
        <React.Fragment />
    ) : (
        <Card
            variant={`outlined`}
            sx={{
                py: { xs: 0.5, md: 2 },
                m: 2,
                backgroundColor: `rgb(220, 220, 220)`,
                display: 'flex',
                justifyContent: 'center',
            }}
        >
            <Stack
                sx={{
                    width: 'max-content',
                    mt: {
                        xs: 1,
                        md: 0,
                    },
                    gap: {
                        xs: 0,
                        md: 1,
                    },
                    flexDirection: {
                        xs: 'column',
                        md: 'row',
                        alignItems: 'center',
                    },
                }}
            >
                <Typography
                    fontWeight={`bold`}
                    textAlign={`center`}
                    lineHeight={1}
                >
                    Trenutna kupovina: Jednokratna
                </Typography>
                <Button
                    href="/logovanje"
                    component={NextLink}
                    sx={{
                        fontWeight: 'bold',
                        lineHeight: 1,
                    }}
                >
                    Prebaci se na profi!
                </Button>
            </Stack>
        </Card>
    )
}
