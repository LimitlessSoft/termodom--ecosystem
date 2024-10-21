import React from 'react'
import { Badge, Button, Grid, Stack } from '@mui/material'
import { useRouter } from 'next/router'
import { LockPerson } from '@mui/icons-material'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'

const SubModuledLayout = ({ modules, children }) => {
    const router = useRouter()

    const ModuleButton = ({ module }) => {
        return (
            <Badge
                color="default"
                badgeContent={
                    !module.hasPermission && <LockPerson color="warning" />
                }
            >
                <Button
                    variant="contained"
                    disabled={!module.hasPermission}
                    onClick={() => router.push(`/partneri${module.href}`)}
                >
                    {module.label}
                </Button>
            </Badge>
        )
    }

    return (
        <Stack gap={4} padding={4}>
            <Stack direction={`row`} gap={2}>
                {modules.length > 1 &&
                    modules.map((moduleData, index) => (
                        <ModuleButton key={index} module={moduleData} />
                    ))}
            </Stack>
            {children}
        </Stack>
    )
}

export default React.memo(SubModuledLayout)
