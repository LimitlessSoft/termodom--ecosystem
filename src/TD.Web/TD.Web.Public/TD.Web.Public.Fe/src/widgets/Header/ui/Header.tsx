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
                <Link
                    href="/logovanje"
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