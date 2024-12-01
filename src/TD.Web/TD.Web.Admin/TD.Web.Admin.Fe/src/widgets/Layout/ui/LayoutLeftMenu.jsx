import { NAV_BAR_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { LayoutLeftMenuStyled } from '../styled/LayoutLeftMenuStyled'
import LayoutLeftMenuButton from './LayoutLeftMenuButton'
import { Typography } from '@mui/material'
import { useRouter } from 'next/router'

export const LayoutLeftMenu = ({ isMobileMenuExpanded, onMobileMenuClose }) => {
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.NAV_BAR
    )

    const router = useRouter()

    return (
        <LayoutLeftMenuStyled $isMobileMenuExpanded={isMobileMenuExpanded}>
            {NAV_BAR_CONSTANTS.MODULES(permissions).map((navLink, index) => {
                if (
                    navLink.hasOwnProperty('hasPermission') &&
                    !navLink.hasPermission
                )
                    return

                return (
                    <LayoutLeftMenuButton
                        key={index}
                        onClick={() => {
                            router.push(navLink.href)
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
