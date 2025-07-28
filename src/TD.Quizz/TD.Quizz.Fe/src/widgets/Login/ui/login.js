import { Button, Paper, Stack, TextField, Typography } from '@mui/material'
import { useZSetUser, useZUser } from '@/zStore'
import { useState } from 'react'
import { loginAction } from '@/app/actions/login'
import { toast } from 'react-toastify'
import { handleResponse } from '@/helpers/responseHelpers'

export const Login = () => {
    const zUser = useZUser()
    const zSetUser = useZSetUser()
    
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    
    if (zUser)
        return (
            <Typography>
                Please wait for redirect...
            </Typography>
        )
    
    return (
        <Stack alignItems={`center`} justifyContent={`center`} height={`100vh`}>
            <Paper sx={{
                padding: 2,
            }}>
                <Stack gap={2}>
                    <TextField
                        label={`Korisničko ime`}
                        variant={`outlined`}
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        />
                    <TextField
                        label={`Lozinka`}
                        variant={`outlined`}
                        type={`password`}
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        />
                    <Button variant={`contained`} onClick={async () => {
                        const resp = await loginAction(username, password)
                        handleResponse(resp, (data) => {
                            zSetUser(data)
                        })
                    }}>
                        Prijavi se
                    </Button>
                </Stack>
            </Paper>
        </Stack>
    )
}