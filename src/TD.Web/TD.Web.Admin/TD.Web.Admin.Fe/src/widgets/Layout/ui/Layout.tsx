import { CircularProgress, Grid } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { ReactNode, useEffect, useState } from 'react'
import { useRouter } from 'next/router'
import Head from 'next/head'
import { Close, Menu } from '@mui/icons-material'
import { LayoutLeftMenu } from './LayoutLeftMenu'
import { mainTheme } from '@/theme'

interface ILayoutProps {
    children: ReactNode
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    const user = useUser(true, true)
    const router = useRouter()

    const [isMobileMenuExpanded, setIsMobileMenuExpanded] = useState(false)

    const { children } = props

    useEffect(() => {}, [user, user.isLoading])

    const handleMobileMenuClose = () => setIsMobileMenuExpanded(false)

    return user.isLoading ||
        (user.isLogged !== true && router.route !== '/logovanje') ? (
        <CircularProgress />
    ) : (
        <div className={`mainWrapper`}>
            <Head>
                <title>Termodom</title>
                <meta
                    name="viewport"
                    content="width=device-width, initial-scale=1.0"
                ></meta>
            </Head>
            <main>
                <Grid
                    container
                    position={`fixed`}
                    top={0}
                    zIndex={99999}
                    height={`7vh`}
                    bgcolor={`#e12121`}
                    width={`100vw`}
                    alignItems={`center`}
                    justifyContent={`flex-end`}
                    color={`white`}
                    sx={{
                        display: {
                            md: `none`,
                        },
                    }}
                >
                    <Grid
                        item
                        onClick={() => setIsMobileMenuExpanded((prev) => !prev)}
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
                <Grid
                    container
                    justifyContent={`end`}
                    minHeight={`100vh`}
                    paddingTop={{
                        xs: 12,
                        md: 4,
                    }}
                >
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
