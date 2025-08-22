'use client'
import { Button, Stack } from '@mui/material'
import { StartQuizz } from '@/widgets'
import { signOut, useSession } from 'next-auth/react'
import { useZSessionFetching } from '@/zStore'
import NextLink from 'next/link'

export const Home = () => {
    const { data: session } = useSession()
    const zSessionFetching = useZSessionFetching()
    if (!session) return null

    return (
        <Stack
            spacing={2}
            sx={{ padding: 2 }}
            alignItems={`center`}
            height={`100vh`}
            justifyContent={`center`}
        >
            <StartQuizz />
            <Button
                variant={`contained`}
                color={`warning`}
                disabled={zSessionFetching.isFetching}
                onClick={() => {
                    zSessionFetching.set(true)
                    signOut().then(() => {
                        zSessionFetching.set(false)
                    })
                }}
            >
                Izloguj se
            </Button>
            {session?.user?.isAdmin && (
                <Button href={`/admin`} LinkComponent={NextLink}>
                    Admin Panel
                </Button>
            )}
        </Stack>
    )
}
