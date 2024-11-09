import {
    Description,
    Home,
    Language,
    LocalAtm,
    LocalShipping,
    Logout,
    People,
    Person,
    RequestQuote,
} from '@mui/icons-material'
import { ILayoutLeftMenuProps } from '../interfaces/ILayoutLeftMenuProps'
import {
    COOKIES_CONSTANTS,
    NAV_BAR_CONSTANTS,
    PERMISSIONS_CONSTANTS,
    URL_CONSTANTS,
} from '@/constants'
import { LayoutLeftMenuButton } from './LayoutLeftMenuButton'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { Grid, styled, Typography } from '@mui/material'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'
import { useState } from 'react'

export const LayoutLeftMenu = ({ fixed, mobileHide }: ILayoutLeftMenuProps) => {
    const [isMenuExpanded, setIsMenuExpanded] = useState(false)

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.NAV_BAR
    )

    const router = useRouter()
    const [userToken, setUserToken] = useCookie(
        COOKIES_CONSTANTS.TOKEN.NAME,
        COOKIES_CONSTANTS.TOKEN.DEFAULT_VALUE
    )

    const LayoutLeftMenuStyled = styled(Grid)(
        ({ theme }) => `
            background-color: ${theme.palette.primary.main};
            color: ${theme.palette.primary.contrastText};
            height: 100vh;
            z-index: 950;
            position: fixed;

            @media screen and (max-width: ${theme.breakpoints.values.md}px) {
                ${fixed ? 'position: fixed; top: 0; left: 0;' : null}
                ${!fixed ? 'opacity: 0;' : null}
            }
            
            @media screen and (max-width: ${theme.breakpoints.values.md}px) {
                display: ${mobileHide || !fixed ? `none` : `block`};
            }
        `
    )

    const navLinks = [
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.KONTROLNA_TABLA,
            href: URL_CONSTANTS.URL_PREFIXES.KONTROLNA_TABLA,
            icon: <Home />,
            isLogout: false,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.PARTNERI,
            href: URL_CONSTANTS.URL_PREFIXES.PARTNERI,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PARTNERI.READ
            ),
            icon: <People />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.NALOG_ZA_PREVOZ,
            href: URL_CONSTANTS.URL_PREFIXES.NALOG_ZA_PREVOZ,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.NALOG_ZA_PREVOZ.READ
            ),
            icon: <LocalShipping />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.PRORACUN,
            href: URL_CONSTANTS.URL_PREFIXES.PRORACUN,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI.READ
            ),
            icon: <RequestQuote />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.SPECIFIKACIJA_NOVCA,
            href: URL_CONSTANTS.URL_PREFIXES.SPECIFIKACIJA_NOVCA,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.SPECIFIKACIJA_NOVCA.READ
            ),
            icon: <LocalAtm />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.WEB_PRODAVNICA,
            href: URL_CONSTANTS.URL_PREFIXES.WEB_PRODAVNICA,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.WEB_SHOP.READ
            ),
            icon: <Language />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.IZVESTAJI,
            href: URL_CONSTANTS.URL_PREFIXES.IZVESTAJI,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS
                    .IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA
                    .READ
            ),
            icon: <Description />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.KORISNICI,
            href: URL_CONSTANTS.URL_PREFIXES.KORISNICI,
            hasPermission: hasPermission(
                permissions,
                PERMISSIONS_CONSTANTS.USER_PERMISSIONS.KORISNICI.READ
            ),
            icon: <Person />,
        },
        {
            label: NAV_BAR_CONSTANTS.MODULE_LABELS.ODJAVI_SE,
            href: null,
            icon: <Logout />,
            isLogout: false,
        },
    ]

    return (
        <LayoutLeftMenuStyled
            onMouseEnter={() => setIsMenuExpanded(true)}
            onMouseLeave={() => setIsMenuExpanded(false)}
        >
            <Grid
                container
                direction={`column`}
                id={`asgbuoasg`}
                sx={{
                    paddingTop: {
                        xs: 5,
                        md: 0,
                    },
                }}
            >
                {navLinks.map((navLink, index) => {
                    if (
                        navLink.hasOwnProperty('hasPermission') &&
                        !navLink.hasPermission
                    )
                        return

                    const isLogout =
                        navLink.label ===
                        NAV_BAR_CONSTANTS.MODULE_LABELS.ODJAVI_SE

                    return (
                        <LayoutLeftMenuButton
                            key={index}
                            tooltip={navLink.label}
                            onClick={() => {
                                if (isLogout) {
                                    setUserToken('')
                                    router.reload()
                                } else if (navLink.href) {
                                    router.push(navLink.href)
                                }
                            }}
                        >
                            {navLink.icon}
                            {isMenuExpanded && (
                                <Typography>{navLink.label}</Typography>
                            )}
                        </LayoutLeftMenuButton>
                    )
                })}
            </Grid>
        </LayoutLeftMenuStyled>
    )
}
