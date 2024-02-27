import { Box, CircularProgress, Grid, Link, Stack, Typography, styled } from '@mui/material'
import { fetchMe, selectUser } from '@/features/userSlice/userSlice'
import tdLogo from '../../../../public/termodom-logo-white.svg'
import { useAppDispatch, useAppSelector } from '@/app/hooks'
import { HeaderWrapperStyled } from './HeaderWrapperStyled'
import useCookie from 'react-use-cookie'
import NextLink from 'next/link'
import { useEffect } from 'react'
import { XButtonStyled } from './XButtonStyled'
import { MobileHeaderNotchStyled } from './MobileHeaderNotchStyled'
import { MobileHeaderNotch } from './MobileHeaderNotch'
import { DividerStyled } from './DividerStyled'
import { Divider } from './Divider'
import { HeaderLinkStyled } from './HeaderLinkStyled'
import { HeaderLink } from './HeaderLink'

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

    const linkVariant = `body1`

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
            <MobileHeaderNotch onClick={() => {
                toggleMobileMenu()
            }} />

            <HeaderWrapperStyled
                user={user}
                id="header-wrapper">
                    <XButtonStyled onClick={() => {
                        toggleMobileMenu()
                    }}>
                        X
                    </XButtonStyled>
                <Box>
                    <img src={tdLogo.src} style={{ width: '100%', maxWidth: '3rem', padding: `4px` }} alt={`Termodom logo`} />
                </Box>
                <HeaderLink
                    onClick={() => {
                        toggleMobileMenu()
                    }}
                    href="/"
                    text="Prodavnica" />

                <HeaderLink
                    onClick={() => {
                        toggleMobileMenu()
                    }}
                    href="/kontakt"
                    text="Kontakt" />
                
                <Divider user={user} />
                
                <HeaderLink
                    onClick={() => {
                        toggleMobileMenu()
                    }}
                    href="/korpa"
                    text="Korpa" />

                {
                    user.isLoading ?
                        <CircularProgress /> :
                        user.isLogged ?
                            <HeaderLink
                                href="#"
                                text='Izloguj se'
                                onClick={(e) => {
                                    e.preventDefault()
                                    setUserToken('')
                                    dispatch(fetchMe())
                                    toggleMobileMenu()
                                }} /> :
                            <HeaderLink
                                onClick={() => {
                                    toggleMobileMenu()
                                }}
                                href="/profi-kutak"
                                text="Profi Kutak" />
                }

                {
                    user.isLogged == false ? null :
                        <HeaderLink
                            onClick={() => {
                                toggleMobileMenu()
                            }}
                            href="/profi-kutak"
                            text="Moj kutak" />
                }
            </HeaderWrapperStyled>
        </header>
    )
}