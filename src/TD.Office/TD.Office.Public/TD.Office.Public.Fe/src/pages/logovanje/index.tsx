import { Box, Button, Grid, Stack, TextField, Typography } from "@mui/material"
import LogoLong from './assets/Logo_Long.png'
import Image from "next/image"
import { useEffect, useState } from "react"
import { mainTheme } from "@/app/themes"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { useRouter } from "next/router"
import NextLink from 'next/link'
import useCookie from 'react-use-cookie'
import { useAppDispatch, useUser } from "@/app/hooks"
import { fetchMe } from "@/features/slices/userSlice/userSlice"

const textFieldVariant = 'filled'

interface LoginRequest {
    username: string,
    password: string
}

const Logovanje = (): JSX.Element => {
    
    const router = useRouter()

    const dispatch = useAppDispatch()
    const user = useUser(false)

    const [userToken, setUserToken] = useCookie('token', undefined)
    const [loginRequest, setLoginRequest] = useState<LoginRequest>({
        username: "",
        password: ""
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
            zIndex={-1}
            left={0}
            height={`100vh`}
            container
            direction={`row`}
            justifyContent={'center'}
            alignItems={`center`}>
            <Stack
                direction={`column`}>
                    <Stack
                        alignItems={`center`}>
                        <Image src={LogoLong.src} width={216} height={30} alt={`Termodom logo`} />
                        <Typography
                            sx={{ m: 1 }}
                            variant={`h6`}
                            component={`h2`}>
                                PROFI KUTAK
                        </Typography>
                    </Stack>
                    <Stack
                        direction={`column`}>
                        <TextField
                            required
                            sx={{ m: 1 }}
                            id='username'
                            label='Korisničko ime'
                            onChange={(e) => {
                                setLoginRequest((prev) => { return { ...prev, username: e.target.value }})
                            }}
                            variant={textFieldVariant} />
                        <TextField
                            required
                            type={`password`}
                            sx={{ m: 1 }}
                            id='password'
                            label='Lozinka'
                            onChange={(e) => {
                                setLoginRequest((prev) => { return { ...prev, password: e.target.value }})
                            }}
                            variant={textFieldVariant} />
                    </Stack>
                    <Button
                        variant={`contained`}
                        sx={{ m: 2, mx: 5, width: 'auto', color: mainTheme.palette.primary.contrastText }}
                        color={`secondary`}
                        onClick={() => {
                            fetchApi(ApiBase.Main, "/login", {
                                method: "POST",
                                contentType: ContentType.ApplicationJson,
                                body: loginRequest
                            }).then((response) => {
                                setUserToken(response)
                                dispatch(fetchMe())
                            })
                        }}>
                            Uloguj se
                    </Button>
                    <Button
                        href="/registrovanje"
                        component={NextLink}
                        variant={`contained`}
                        sx={{ m: 0.7, p: 0, backgroundColor: '#4caf50', '&:hover': { backgroundColor: '#3f9142' } }}>
                            Postani profi kupac
                    </Button>
            </Stack>
        </Grid>
    )
}

export default Logovanje