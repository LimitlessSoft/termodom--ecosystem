import { LayoutLeftMenu } from './LayoutLeftMenu'
import { useUser } from '@/hooks/useUserHook'
import { useRouter } from 'next/router'
import { Box, Grid } from '@mui/material'
import Head from 'next/head'
import { useState } from 'react'
import { Close, Menu } from '@mui/icons-material'
import { mainTheme } from '@/themes'
import { PRINT_CONSTANTS } from '@/constants'

export const Layout = (props) => {
    const { children } = props
    const user = useUser(true, true)
    const router = useRouter()

    const [isMobileMenuExpanded, setIsMobileMenuExpanded] = useState(false)

    const handleMobileMenuClose = () => {
        setIsMobileMenuExpanded(false)
    }

    return (
        <div className={`mainWrapper`}>
            <Head>
                <title>TDOffice</title>
                <meta
                    name="viewport"
                    content="width=device-width, initial-scale=1.0"
                ></meta>
            </Head>
            <main>
                {router.query.noLayout !== 'true' &&
                    (user?.isLogged !== null || user.isLogged !== false) && (
                        <>
                            <Grid
                                container
                                className={
                                    PRINT_CONSTANTS.PRINT_CLASSNAMES.NO_PRINT
                                }
                                position={`fixed`}
                                top={0}
                                zIndex={999999}
                                height={`7vh`}
                                bgcolor={`#1976d2`}
                                width={`100%`}
                                maxWidth={`100vw`}
                                alignItems={`center`}
                                justifyContent={`flex-end`}
                                sx={{
                                    display: {
                                        md: 'none',
                                    },
                                    color: 'white',
                                }}
                            >
                                <Grid
                                    item
                                    onClick={() => {
                                        setIsMobileMenuExpanded((prev) => !prev)
                                    }}
                                >
                                    {isMobileMenuExpanded ? (
                                        <Close
                                            sx={{
                                                width: 40,
                                                height: 40,
                                                px: 1,
                                            }}
                                        />
                                    ) : (
                                        <Menu
                                            sx={{
                                                width: 40,
                                                height: 40,
                                                px: 1,
                                            }}
                                        />
                                    )}
                                </Grid>
                            </Grid>
                            <LayoutLeftMenu
                                isMobileMenuExpanded={isMobileMenuExpanded}
                                onMobileMenuClose={handleMobileMenuClose}
                            />
                        </>
                    )}
                <Grid container justifyContent={`end`}>
                    <Grid
                        item
                        flex={1}
                        maxWidth={{
                            xs: `100%`,
                            md: `calc(100% - ${mainTheme.spacing(8)})`,
                        }}
                    >
                        {children}
                    </Grid>
                </Grid>
            </main>
        </div>
    )
}
