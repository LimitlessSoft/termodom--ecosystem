import React from 'react'
import { Badge, Stack, Typography } from '@mui/material'
import { useRouter } from 'next/router'
import { LockPerson } from '@mui/icons-material'
import { SubModuleButtonStyled } from '../styled/SubModuleButtonStyled'
import Grid2 from '@mui/material/Unstable_Grid2'

const SubModuleLayout = ({ subModules, children }) => {
    const router = useRouter()

    const hasMoreThanOneSubmodule = subModules.length > 1

    return (
        <Stack gap={hasMoreThanOneSubmodule && 2} px={4}>
            {subModules && hasMoreThanOneSubmodule ? (
                <>
                    <Grid2 container direction={`row`} gap={2}>
                        {subModules.map((module, index) => {
                            const currentlyActive =
                                router.pathname === module.href
                            const noPermission = !module.hasPermission

                            return (
                                <Grid2 key={index}>
                                    <Badge
                                        color="default"
                                        badgeContent={
                                            noPermission && (
                                                <LockPerson color="warning" />
                                            )
                                        }
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
                                            onClick={() =>
                                                router.push(module.href)
                                            }
                                        >
                                            <Typography>
                                                {module.label}
                                            </Typography>
                                        </SubModuleButtonStyled>
                                    </Badge>
                                </Grid2>
                            )
                        })}
                    </Grid2>
                    {children}
                </>
            ) : (
                children
            )}
        </Stack>
    )
}

export default React.memo(SubModuleLayout)
