import {
    Home,
    Language,
    LocalAtm,
    LocalShipping,
    Logout,
    People,
    Person,
    Person2,
} from '@mui/icons-material'
import { ILayoutLeftMenuProps } from '../interfaces/ILayoutLeftMenuProps'
import { COOKIES, PERMISSIONS_GROUPS, USER_PERMISSIONS } from '@/constants'
import { LayoutLeftMenuButton } from './LayoutLeftMenuButton'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { useAppDispatch } from '@/hooks/useUserHook'
import { Grid, styled } from '@mui/material'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'
import { useState } from 'react'

export const LayoutLeftMenu = ({ fixed, mobileHide }: ILayoutLeftMenuProps) => {
    const permissions = usePermissions(PERMISSIONS_GROUPS.NAV_BAR)

    const router = useRouter()
    const dispatch = useAppDispatch()
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
                    onClick={() => {
                        router.push('/')
                    }}
                >
                    <Home />
                </LayoutLeftMenuButton>

                {hasPermission(permissions, USER_PERMISSIONS.PARTNERI.READ) && (
                    <LayoutLeftMenuButton
                        onClick={() => {
                            router.push('/partneri')
                        }}
                    >
                        <People />
                    </LayoutLeftMenuButton>
                )}

                {hasPermission(
                    permissions,
                    USER_PERMISSIONS.NALOG_ZA_PREVOZ.READ
                ) && (
                    <LayoutLeftMenuButton
                        onClick={() => {
                            router.push('/nalog-za-prevoz')
                        }}
                    >
                        <LocalShipping />
                    </LayoutLeftMenuButton>
                )}

                {hasPermission(
                    permissions,
                    USER_PERMISSIONS.SPECIFIKACIJA_NOVCA.READ
                ) && (
                    <LayoutLeftMenuButton
                        onClick={() => {
                            router.push('/specifikacija-novca')
                        }}
                    >
                        <LocalAtm />
                    </LayoutLeftMenuButton>
                )}

                {hasPermission(permissions, USER_PERMISSIONS.WEB_SHOP.READ) && (
                    <LayoutLeftMenuButton
                        onClick={() => {
                            router.push('/web-prodavnica')
                        }}
                    >
                        <Language />
                    </LayoutLeftMenuButton>
                )}

                {hasPermission(
                    permissions,
                    USER_PERMISSIONS.KORISNICI.READ
                ) && (
                    <LayoutLeftMenuButton
                        onClick={() => {
                            router.push('/korisnici')
                        }}
                    >
                        <Person />
                    </LayoutLeftMenuButton>
                )}

                <LayoutLeftMenuButton
                    onClick={() => {
                        setUserToken('')
                        router.reload()
                    }}
                >
                    <Logout />
                </LayoutLeftMenuButton>
            </Grid>
        </LayoutLeftMenuStyled>
    )
}
