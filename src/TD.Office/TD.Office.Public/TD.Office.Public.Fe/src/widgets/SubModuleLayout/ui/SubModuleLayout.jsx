import React from 'react'
import { Badge, Button, Stack } from '@mui/material'
import { useRouter } from 'next/router'
import { LockPerson } from '@mui/icons-material'

const SubModuleLayout = ({ subModules, children }) => {
    const router = useRouter()

    const SubModuleButton = ({ module }) => {
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
                    onClick={() => router.push(module.href)}
                >
                    {module.label}
                </Button>
            </Badge>
        )
    }

    if (!subModules || subModules.length <= 1) {
        return children
    }
    return (
        <Stack gap={2} padding={4}>
            <Stack direction={`row`} gap={2}>
                {subModules.map((module, index) => (
                    <SubModuleButton key={index} module={module} />
                ))}
            </Stack>
            {children}
        </Stack>
    )
}

export default React.memo(SubModuleLayout)
