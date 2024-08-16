import { Button, Grid, Stack, TextField, Typography } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import LogoLong from './assets/Logo_Long.png'
import { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'
import { COOKIES } from '@/constants'
import { mainTheme } from '@/theme'
import Image from 'next/image'

const textFieldVariant = 'filled'

interface LoginRequest {
    username: string
    password: string
}

const Logovanje = (): JSX.Element => {
    const router = useRouter()

    const user = useUser(false)
    const [token, setToken] = useCookie(COOKIES.TOKEN.NAME)

    const [loginRequest, setLoginRequest] = useState<LoginRequest>({
        username: '',
        password: '',
    })

    useEffect(() => {
        if (user != null && user.isLogged) {
            router.push('/')
        }
    }, [router, user, user.isLogged])

    return (
        <Grid
            position={`fixed`}
            top={0}
            zIndex={1}
            left={0}
            height={`100vh`}
            container
            direction={`row`}
            justifyContent={'center'}
            alignItems={`center`}
        >
            <Stack direction={`column`}>
                <Stack alignItems={`center`}>
                    <Image
                        src={LogoLong.src}
                        width={216}
                        height={30}
                        alt={`Termodom logo`}
                    />
                    <Typography sx={{ m: 1 }} variant={`h6`} component={`h2`}>
                        PROFI KUTAK
                    </Typography>
                </Stack>
                <Stack direction={`column`}>
                    <TextField
                        required
                        sx={{ m: 1 }}
                        id="username"
                        label="Korisničko ime"
                        onChange={(e) => {
                            setLoginRequest((prev) => {
                                return { ...prev, username: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                    <TextField
                        required
                        type={`password`}
                        sx={{ m: 1 }}
                        id="password"
                        label="Lozinka"
                        onChange={(e) => {
                            setLoginRequest((prev) => {
                                return { ...prev, password: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                </Stack>
                <Button
                    variant={`contained`}
                    sx={{
                        m: 2,
                        mx: 5,
                        width: 'auto',
                        color: mainTheme.palette.primary.contrastText,
                    }}
                    color={`secondary`}
                    onClick={() => {
                        adminApi
                            .post('/login', loginRequest)
                            .then((response) => {
                                setToken(response.data)
                                router.reload()
                            })
                            .catch((err) => handleApiError(err))
                    }}
                >
                    Uloguj se
                </Button>
            </Stack>
        </Grid>
    )
}

export default Logovanje
