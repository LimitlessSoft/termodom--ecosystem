import { Box, Button, Grid, Stack, TextField, Typography } from "@mui/material"
import LogoLong from './assets/Logo_Long.png'
import Image from "next/image"
import { mainTheme } from "@/app/theme"
import NextLink from 'next/link'
import { useState } from "react"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import useCookie from 'react-use-cookie'

const textFieldVariant = 'filled'

interface LoginRequest {
    username: string,
    password: string
}

const Logovanje = (): JSX.Element => {

    const [loginRequest, setLoginRequest] = useState<LoginRequest>({
        username: "",
        password: ""
    })

    const [userToken, setUserToken] = useCookie('token', undefined)

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
                            })
                        }}>
                            Uloguj se
                    </Button>
                    <Button
                        variant={`contained`}
                        sx={{ m: 0.7, p: 0, px: 1 }}>
                            prebaci se na jednokratnu kupovinu
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