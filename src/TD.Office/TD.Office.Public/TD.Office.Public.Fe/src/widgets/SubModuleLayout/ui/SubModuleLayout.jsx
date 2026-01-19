import React from 'react'
import { Badge, Box, Stack, Typography, useMediaQuery, useTheme } from '@mui/material'
import { useRouter } from 'next/router'
import { LockPerson } from '@mui/icons-material'
import { SubModuleButtonStyled } from '../styled/SubModuleButtonStyled'

const SubModuleLayout = ({ subModules, children }) => {
    const router = useRouter()
    const theme = useTheme()
    const isMobile = useMediaQuery(theme.breakpoints.down('sm'))

    const hasMoreThanOneSubmodule = subModules.length > 1

    return (
        <Stack gap={hasMoreThanOneSubmodule ? (isMobile ? 1 : 2) : 0} px={isMobile ? 1 : 4}>
            {subModules && hasMoreThanOneSubmodule ? (
                <>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            gap: isMobile ? 0.5 : 2,
                            overflowX: 'auto',
                            pb: isMobile ? 0.5 : 0,
                            WebkitOverflowScrolling: 'touch',
                            '&::-webkit-scrollbar': {
                                height: isMobile ? 4 : 'auto',
                            },
                        }}
                    >
                        {subModules.map((module, index) => {
                            const currentlyActive =
                                router.pathname === module.href
                            const noPermission = !module.hasPermission

                            return (
                                <Box key={index} sx={{ flexShrink: 0 }}>
                                    <Badge
                                        color="default"
                                        badgeContent={
                                            noPermission && (
                                                <LockPerson
                                                    color="warning"
                                                    sx={{ fontSize: isMobile ? 16 : 24 }}
                                                />
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
                                            size={isMobile ? 'small' : 'medium'}
                                            disabled={
                                                noPermission || currentlyActive
                                            }
                                            onClick={() =>
                                                router.push(module.href)
                                            }
                                        >
                                            <Typography
                                                variant={isMobile ? 'caption' : 'body1'}
                                                sx={{ whiteSpace: 'nowrap' }}
                                            >
                                                {module.label}
                                            </Typography>
                                        </SubModuleButtonStyled>
                                    </Badge>
                                </Box>
                            )
                        })}
                    </Box>
                    {children}
                </>
            ) : (
                children
            )}
        </Stack>
    )
}

export default React.memo(SubModuleLayout)
