import React from 'react'
import { Badge, Button, Grid, Stack } from '@mui/material'
import { useRouter } from 'next/router'
import { LockPerson } from '@mui/icons-material'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'

const SubModuledLayout = ({ modules, children }) => {
    const router = useRouter()

    const ModuleButton = ({
        label,
        href,
        permission_group,
        permission_name,
    }) => {
        const permissions = usePermissions(permission_group)

        const disabled = !hasPermission(permissions, permission_name)

        return (
            <Badge
                color="default"
                badgeContent={disabled && <LockPerson color="warning" />}
            >
                <Button
                    variant="contained"
                    disabled={disabled}
                    onClick={() => router.push(`/partneri${href}`)}
                >
                    {label}
                </Button>
            </Badge>
        )
    }

    return (
        <Stack gap={4} padding={4}>
            <Grid container gap={2}>
                {modules.length > 1 &&
                    modules.map((moduleData, index) => (
                        <Grid item key={index}>
                            <ModuleButton {...moduleData} />
                        </Grid>
                    ))}
            </Grid>
            {children}
        </Stack>
    )
}

export default React.memo(SubModuledLayout)
