'use client'
import { Button, Paper, Stack, TextField } from '@mui/material'
import { useState } from 'react'
import { signIn } from 'next-auth/react'
import { useZSessionFetching } from '@/zStore'

export const Login = () => {
    const zSessionFetching = useZSessionFetching()
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')

    return (
        <Stack alignItems={`center`} justifyContent={`center`} height={`100vh`}>
            <Paper
                sx={{
                    padding: 2,
                }}
            >
                <Stack gap={2}>
                    <TextField
                        disabled={zSessionFetching.isFetching}
                        label={`Korisničko ime`}
                        variant={`outlined`}
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                    <TextField
                        disabled={zSessionFetching.isFetching}
                        label={`Lozinka`}
                        variant={`outlined`}
                        type={`password`}
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <Button
                        disabled={zSessionFetching.isFetching}
                        variant={`contained`}
                        onClick={() => {
                            zSessionFetching.set(true)
                            signIn(`credentials`, {
                                username,
                                password,
                                redirect: true,
                                callbackUrl: `/`,
                            }).then(() => {
                                zSessionFetching.set(false)
                            })
                        }}
                    >
                        Prijavi se
                    </Button>
                </Stack>
            </Paper>
        </Stack>
    )
}
