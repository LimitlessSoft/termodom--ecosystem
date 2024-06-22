import { Box, CircularProgress } from '@mui/material'
import { fetchMe, selectUser } from '@/features/userSlice/userSlice'
import tdLogo from '../../../../public/termodom-logo-white.svg'
import { useAppDispatch, useAppSelector } from '@/app/hooks'
import { HeaderWrapperStyled } from './HeaderWrapperStyled'
import { MobileHeaderNotch } from './MobileHeaderNotch'
import { XButtonStyled } from './XButtonStyled'
import { ApiBase, fetchApi } from '@/app/api'
import { HeaderLink } from './HeaderLink'
import useCookie from 'react-use-cookie'
import { toast } from 'react-toastify'
import { Divider } from './Divider'
import { useEffect } from 'react'

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
            response.json().then((response: any) => {
                response.map((alert: any) => {
                    toast.info(alert.text, {
                        autoClose: 1000 * 30,
                        theme: `colored`
                    })
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

    const linkVariant = `body1`

    const toggleMobileMenu = () => {
        var el = document.getElementById('header-wrapper')

        var currT = el?.style.getPropertyValue('transform')

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
                    <img src={tdLogo.src} style={{ width: '100%', minHeight: '30px', maxWidth: '3rem', padding: `4px` }} alt={`Termodom logo`} />
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
                
                {
                    user.isLoading || user.isLogged == false || user.data?.isAdmin == false ? null :
                        <HeaderLink
                            target="_blank"
                            href="https://admin.termodom.rs"
                            text="Admin panel" />
                }

                {
                    user.isLoading || user.isLogged == false || user.data?.isAdmin == false ? null :
                        <HeaderLink
                            target="_blank"
                            href="https://office.termodom.rs"
                            text="Office panel" />
                }
                
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