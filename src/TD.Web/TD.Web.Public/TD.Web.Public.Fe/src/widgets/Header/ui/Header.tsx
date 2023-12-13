import { Box, CircularProgress, Link, Stack, Typography } from '@mui/material'
import NextLink from 'next/link'
import tdLogo from '../../../../public/termodom-logo-white.svg'
import { fetchMe, selectUser } from '@/features/userSlice/userSlice'
import { useAppDispatch, useAppSelector } from '@/app/hooks'
import { useEffect } from 'react'

export const Header = (): JSX.Element => {

    const dispatch = useAppDispatch()
    const user = useAppSelector(selectUser)

    useEffect(() => {
        dispatch(fetchMe())
    }, [dispatch])

    useEffect(() => {
        console.log(user)
    }, [user])

    const linkStyle = {
        textDecoration: 'none',
        color: 'var(--td-white)'
    }

    const nameLabelStyle = {
        oneTime: {
            textDecoration: 'none',
            color: '#ffee00'
        },
        user: {
            textDecoration: 'none',
            color: '#ffee00'
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
                    href="/profi-kutak"
                    component={NextLink}
                    variant={linkVariant}
                    style={linkStyle}>
                        <Typography>
                            Profi Kutak
                        </Typography>
                </Link>
            </Stack>
        </header>
    )
}