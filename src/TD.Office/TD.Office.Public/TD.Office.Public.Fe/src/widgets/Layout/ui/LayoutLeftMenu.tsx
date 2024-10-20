import {
    Description,
    Home,
    Language,
    LocalAtm,
    LocalShipping,
    Logout,
    People,
    Person,
    Person2,
    Report,
    ReportGmailerrorred,
    RequestQuote,
} from '@mui/icons-material'
import { ILayoutLeftMenuProps } from '../interfaces/ILayoutLeftMenuProps'
import { COOKIES, PERMISSIONS_GROUPS, USER_PERMISSIONS } from '@/constants'
import { LayoutLeftMenuButton } from './LayoutLeftMenuButton'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { Grid, styled, Tooltip } from '@mui/material'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'

export const LayoutLeftMenu = ({ fixed, mobileHide }: ILayoutLeftMenuProps) => {
    const permissions = usePermissions(PERMISSIONS_GROUPS.NAV_BAR)

    const router = useRouter()
    const [userToken, setUserToken] = useCookie(
        COOKIES.TOKEN.NAME,
        COOKIES.TOKEN.DEFAULT_VALUE
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

    return (
        <LayoutLeftMenuStyled>
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
                <LayoutLeftMenuButton
                    tooltip={`Kontrolna Tabla`}
                    onClick={() => {
                        router.push('/')
                    }}
                >
                    {' '}
                    <Home />{' '}
                </LayoutLeftMenuButton>

                {hasPermission(permissions, USER_PERMISSIONS.PARTNERI.READ) && (
                    <LayoutLeftMenuButton
                        tooltip={`Partneri`}
                        onClick={() => {
                            router.push('/partneri')
                        }}
                    >
                        {' '}
                        <People />{' '}
                    </LayoutLeftMenuButton>
                )}

                {hasPermission(
                    permissions,
                    USER_PERMISSIONS.NALOG_ZA_PREVOZ.READ
                ) && (
                    <LayoutLeftMenuButton
                        tooltip={`Nalog za prevoz`}
                        onClick={() => {
                            router.push('/nalog-za-prevoz')
                        }}
                    >
                        {' '}
                        <LocalShipping />{' '}
                    </LayoutLeftMenuButton>
                )}

                <LayoutLeftMenuButton
                    tooltip={`Proračun`}
                    onClick={() => {
                        router.push('/proracun')
                    }}
                >
                    {' '}
                    <RequestQuote />{' '}
                </LayoutLeftMenuButton>

                <LayoutLeftMenuButton
                    tooltip={`Specifikacija novca`}
                    onClick={() => {
                        router.push('/specifikacija-novca')
                    }}
                >
                    {' '}
                    <LocalAtm />{' '}
                </LayoutLeftMenuButton>

                {hasPermission(permissions, USER_PERMISSIONS.WEB_SHOP.READ) && (
                    <LayoutLeftMenuButton
                        tooltip={`Web Prodavnica`}
                        onClick={() => {
                            router.push('/web-prodavnica')
                        }}
                    >
                        {' '}
                        <Language />{' '}
                    </LayoutLeftMenuButton>
                )}

                {hasPermission(
                    permissions,
                    USER_PERMISSIONS
                        .IZVESTAJ_UKUPNE_KOLICINE_PO_ROBI_U_FILTRIRANIM_DOKUMENTIMA
                        .READ
                ) && (
                    <LayoutLeftMenuButton
                        onClick={() => {
                            router.push('/izvestaji')
                        }}
                    >
                        {' '}
                        <Description />{' '}
                    </LayoutLeftMenuButton>
                )}

                {hasPermission(
                    permissions,
                    USER_PERMISSIONS.KORISNICI.READ
                ) && (
                    <LayoutLeftMenuButton
                        tooltip={`Korisnici`}
                        onClick={() => {
                            router.push('/korisnici')
                        }}
                    >
                        {' '}
                        <Person />{' '}
                    </LayoutLeftMenuButton>
                )}

                <LayoutLeftMenuButton
                    tooltip={`Odjavi se`}
                    onClick={() => {
                        setUserToken('')
                        router.reload()
                    }}
                >
                    {' '}
                    <Logout />{' '}
                </LayoutLeftMenuButton>
            </Grid>
        </LayoutLeftMenuStyled>
    )
}
