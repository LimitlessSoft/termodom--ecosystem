import {
    COOKIES_CONSTANTS,
    NAV_BAR_CONSTANTS,
    PERMISSIONS_CONSTANTS,
} from '@/constants'
import { LayoutLeftMenuButton } from './LayoutLeftMenuButton'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { Typography } from '@mui/material'
import useCookie from 'react-use-cookie'
import { useRouter } from 'next/router'
import { LayoutLeftMenuStyled } from '../styled/LayoutLeftMenuStyled'

export const LayoutLeftMenu = ({ isMobileMenuExpanded, onMobileMenuClose }) => {
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.NAV_BAR
    )

    const router = useRouter()
    const [userToken, setUserToken] = useCookie(
        COOKIES_CONSTANTS.TOKEN.NAME,
        COOKIES_CONSTANTS.TOKEN.DEFAULT_VALUE
    )

    return (
        <LayoutLeftMenuStyled $isMobileMenuExpanded={isMobileMenuExpanded}>
            {NAV_BAR_CONSTANTS.MODULES(permissions).map((navLink, index) => {
                if (
                    navLink.hasOwnProperty('hasPermission') &&
                    !navLink.hasPermission
                )
                    return

                const isLogout =
                    navLink.label === NAV_BAR_CONSTANTS.MODULE_LABELS.ODJAVI_SE

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
                            onMobileMenuClose()
                        }}
                    >
                        {navLink.icon}
                        <Typography className={`nav-label`}>
                            {navLink.label}
                        </Typography>
                    </LayoutLeftMenuButton>
                )
            })}
        </LayoutLeftMenuStyled>
    )
}
