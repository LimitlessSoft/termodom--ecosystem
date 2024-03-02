import { Box, Button, Grid, Stack, TextField, Typography } from "@mui/material"
import LogoLong from './assets/Logo_Long.png'
import Image from "next/image"
import { mainTheme } from "@/app/theme"
import NextLink from 'next/link'
import { useEffect, useState } from "react"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import useCookie from 'react-use-cookie'
import { useRouter } from "next/router"
import { fetchMe, selectUser } from "@/features/userSlice/userSlice"
import { useAppDispatch, useAppSelector } from "@/app/hooks"

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
    const user = useAppSelector(selectUser)
    const router = useRouter()
    const [isRefreshingData, setIsRefreshingData] = useState<boolean>(true)
    const dispatch = useAppDispatch()

    useEffect(() => {
        dispatch(fetchMe())
        .then((res) => {
            setIsRefreshingData(false)
        })
    }, [dispatch])

    useEffect(() => {
        if(isRefreshingData)
            return
        
        if(user.isLogged) {
            console.log('User is not logged in, redirecting to /logovanje')
            router.push('/profi-kutak')
        }
    }, [isRefreshingData, user, router])

    return (
        <Grid
            position={`relative`}
            top={0}
            zIndex={0}
            left={0}
            height={`calc(100vh - 64px)`}
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
                                router.push('/profi-kutak')
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