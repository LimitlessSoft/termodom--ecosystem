import { Box, CircularProgress, Grid, Link, Stack, Typography, styled } from '@mui/material'
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

        // fetchApi(ApiBase.Main, `/global-alerts`)
        // .then((response) => {
        //     response.map((alert: any) => {
        //         toast.info(alert.text, {
        //             autoClose: 1000 * 30,
        //             theme: `colored`
        //         })
        //     })
        // })
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

    const HeaderWrapperStyled = styled(Stack)(
        ({ theme }) => `
            flex-direction: row;
            align-items: center;
            padding-left: 10px;
            padding-right: 10px;
            transition-duration: 0.5s;

            @media only screen and (max-width: 600px) {
                transform: translateX(-100%);
                flex-direction: column;
                min-height: 100vh;
                z-index: 1000;
                width: 100vw;
                top: 0;
                left: 0;
                position: fixed;
                padding-top: 20px;
                padding-bottom: 20px;
                background-color: var(--td-red);
            }
        `)

        const DividerStyled = styled(Typography)(
            ({ theme }) => `
                flex-grow: 1;

                @media only screen and (max-width: 600px) {
                    flex-grow: initial;
                }
            `
        )

    const XButtonStyled = styled(Box)(
        ({ theme }) => `
            display: none;

            @media only screen and (max-width: 600px) {
                display: block;
                position: absolute;
                right: 0px;
                top: 20px;
                transform: translateX(-100%);
                background-color: rgba(0, 0, 0, 0.4);
                border-radius: 10px;
                padding: 10px 15px;
                color: white;
                font-size: 1.5rem;
            }
        `
    )

    const MobileHeaderNotchStyled = styled(Grid)(
        ({ theme }) => `
            display: none;
            background-color: var(--td-red);
            top: 0;
            left: 0;
            width: 100vw;
            padding: 8px;
            z-index: 100;

            span {
                display: block;
                width: 40px;
                height: 10px;
                margin: 5px;
                background-color: black;
            }

            @media only screen and (max-width: 600px) {
                display: block;
            }
        `
    )

    const toggleMobileMenu = () => {
        var el = document.getElementById('header-wrapper')

        var currT = el?.style.getPropertyValue('transform')

        console.log(currT)
        if(currT == 'translateX(0px)') {
            el?.style.setProperty('transform', 'translateX(-100%)')
            return
        }

        el?.style.setProperty('transform', 'translateX(0)')
    }
    return (
        <header style={{ backgroundColor: 'var(--td-red)' }}>
            <MobileHeaderNotchStyled
                onClick={() => {
                    toggleMobileMenu()
                }}>
                <span></span>
                <span></span>
                <span></span>
            </MobileHeaderNotchStyled>
            <HeaderWrapperStyled
                id="header-wrapper">
                    <XButtonStyled onClick={() => {
                        toggleMobileMenu()
                    }}>
                        X
                    </XButtonStyled>
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
                <DividerStyled
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
                                "Jednokratna kupovina"
                    }
                </DividerStyled>
                
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
            </HeaderWrapperStyled>
        </header>
    )
}