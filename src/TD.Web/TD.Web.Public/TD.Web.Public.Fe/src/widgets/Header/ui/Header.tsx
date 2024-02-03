import { Box, CircularProgress, Link, Stack, Typography } from '@mui/material'
import NextLink from 'next/link'
import tdLogo from '../../../../public/termodom-logo-white.svg'
import { fetchMe, selectUser } from '@/features/userSlice/userSlice'
import { useAppDispatch, useAppSelector } from '@/app/hooks'
import useCookie from 'react-use-cookie'
import { useEffect } from 'react'
import { Bounce, Slide, toast } from 'react-toastify'
import { ApiBase, fetchApi } from '@/app/api'

export const Header = (): JSX.Element => {

    const dispatch = useAppDispatch()
    const user = useAppSelector(selectUser)
    const [userToken, setUserToken] = useCookie('token', undefined)

    useEffect(() => {
        dispatch(fetchMe())
    }, [dispatch])

    useEffect(() => {

        fetchApi(ApiBase.Main, `/global-alerts`)
        .then((response) => {
            response.map((alert: any) => {
                toast.info(alert.text, {
                    autoClose: 1000 * 30,
                    theme: `colored`
                })
            })
        })
    }, [])

    const profiColor = '#ff9800'

    const linkPaddingY = '20px'
    const linkPaddingX = '15px'

    const linkStyle = {
        textDecoration: 'none',
        color: 'var(--td-white)',
        paddingTop: linkPaddingY,
        paddingBottom: linkPaddingY,
        paddingLeft: linkPaddingX,
        paddingRight: linkPaddingX
    }

    const profiStyle = {
        ...linkStyle,
        backgroundColor: profiColor
    }

    const nameLabelStyle = {
        oneTime: {
            textDecoration: 'none',
            color: `yellow`
        },
        user: {
            textDecoration: 'none',
            color: `yellow`
        }
    }

    const linkVariant = `body1`

    return (
        <header style={{ backgroundColor: 'var(--td-red)' }}>
            <Stack
            sx={{ px: 2 }}
            direction={`row`}
            spacing={2}
            alignItems={`center`}>
                <Box>
                    <img src={tdLogo.src} style={{ width: '100%', maxWidth: '3rem', padding: `4px` }} alt={`Termodom logo`} />
                </Box>
                <Link
                    href="/"
                    component={NextLink}
                    variant={linkVariant}
                    style={linkStyle}>
                        <Typography>
                            Prodavnica
                        </Typography>
                </Link>
                <Link
                    href="/kontakt"
                    component={NextLink}
                    variant={linkVariant}
                    style={linkStyle}>
                        <Typography>
                            Kontakt
                        </Typography>
                </Link>
                <Typography
                    flexGrow={1}
                    style={
                        user.isLogged ?
                            nameLabelStyle.user :
                            nameLabelStyle.oneTime
                    }>
                    {
                        user.isLoading ?
                            <CircularProgress color={`primary`} /> :
                            user.isLogged ?
                                user.data?.nickname :
                                "jednokratna kupovina"
                    }
                </Typography>
                
                <Link
                    href="/korpa"
                    component={NextLink}
                    variant={linkVariant}
                    style={linkStyle}>
                        <Typography>
                            Korpa
                        </Typography>
                </Link>
                {
                    user.isLoading ?
                        <CircularProgress /> :
                        user.isLogged ?
                            <Link
                                href="#"
                                component={NextLink}
                                variant={linkVariant}
                                style={linkStyle}
                                onClick={(e) => {
                                    e.preventDefault()
                                    setUserToken('')
                                    dispatch(fetchMe())
                                }}>
                                    <Typography>
                                        Izloguj se
                                    </Typography>
                            </Link> :
                            <Link
                                href="/profi-kutak"
                                component={NextLink}
                                variant={linkVariant}
                                style={linkStyle}>
                                    <Typography>
                                        Profi Kutak
                                    </Typography>
                            </Link>
                }
                {
                    user.isLogged == false ? null :
                        <Link
                        href="/profi-kutak"
                        component={NextLink}
                        variant={linkVariant}
                        style={profiStyle}>
                            <Typography>
                                Moj kutak
                            </Typography>
                        </Link>
                }
            </Stack>
        </header>
    )
}