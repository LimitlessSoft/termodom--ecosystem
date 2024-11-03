import React from 'react'
import { Badge, Stack } from '@mui/material'
import { useRouter } from 'next/router'
import { LockPerson } from '@mui/icons-material'
import { SubModuleButtonStyled } from '../styled/SubModuleButtonStyled'

const SubModuleLayout = ({ subModules, children }) => {
    const router = useRouter()

    const hasMoreThanOneSubmodule = subModules.length > 1

    return (
        <Stack gap={hasMoreThanOneSubmodule && 2} padding={4}>
            {subModules && hasMoreThanOneSubmodule ? (
                <>
                    <Stack direction={`row`} gap={2}>
                        {subModules.map((module, index) => {
                            const currentlyActive =
                                router.pathname === module.href
                            const noPermission = !module.hasPermission

                            return (
                                <Badge
                                    color="default"
                                    badgeContent={
                                        noPermission && (
                                            <LockPerson color="warning" />
                                        )
                                    }
                                    key={index}
                                >
                                    <SubModuleButtonStyled
                                        props={{
                                            currentlyActive,
                                            noPermission,
                                        }}
                                        variant={
                                            currentlyActive
                                                ? `outlined`
                                                : `contained`
                                        }
                                        disabled={
                                            noPermission || currentlyActive
                                        }
                                        onClick={() => router.push(module.href)}
                                    >
                                        {module.label}
                                    </SubModuleButtonStyled>
                                </Badge>
                            )
                        })}
                    </Stack>
                    {children}
                </>
            ) : (
                children
            )}
        </Stack>
    )
}

export default React.memo(SubModuleLayout)
