import { Box, Link, Stack, Typography } from '@mui/material'
import NextLink from 'next/link'
import tdLogo from '../../../../public/termodom-logo-white.svg'

export const Header = (): JSX.Element => {
    const linkStyle = {
        textDecoration: 'none',
        color: 'var(--td-white)'
    }

    const linkVariant = `body1`

    return (
        <header style={{ backgroundColor: 'var(--td-red)' }}>
            <Stack
            direction={`row`}
            spacing={2}
            alignItems={`center`}>
                <Box>
                    <img src={tdLogo.src} style={{ width: '100%', maxWidth: '3rem', padding: `4px` }} />
                </Box>
                <Link
                href="/kontrolna-tabla"
                component={NextLink}
                variant={linkVariant}
                style={linkStyle}>
                    <Typography>
                        Kontrolna Tabla
                    </Typography>
                </Link>
                <Link
                href="/proizvodi"
                component={NextLink}
                variant={linkVariant}
                style={linkStyle}>
                    <Typography>
                        Proizvodi
                    </Typography>
                </Link>
                <Link
                href="/porudžbine"
                component={NextLink}
                variant={linkVariant}
                style={linkStyle}>
                    <Typography>
                        Porudžbine
                    </Typography>
                </Link>
                <Link
                href="/korisnici"
                component={NextLink}
                variant={linkVariant}
                style={linkStyle}>
                    <Typography>
                        Korisnici
                    </Typography>
                </Link>
                <Link
                href="/podešavanja"
                component={NextLink}
                variant={linkVariant}
                style={linkStyle}>
                    <Typography>
                        Podešavanja
                    </Typography>
                </Link>
            </Stack>
        </header>
    )
}