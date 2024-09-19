import { ILayoutProps } from '../interfaces/ILayoutProps'
import { LayoutLeftMenu } from './LayoutLeftMenu'
import { useUser } from '@/hooks/useUserHook'
import { useRouter } from 'next/router'
import { Grid, Snackbar } from '@mui/material'
import Head from 'next/head'
import { useEffect, useState } from 'react'
import { Menu } from '@mui/icons-material'
import { mainTheme } from '@/themes'

export const Layout = (props: ILayoutProps) => {
    const { children } = props
    const user = useUser(true, true)
    const router = useRouter()

    const [isMobileHide, setIsMobileHide] = useState(false)

    useEffect(() => {
        setIsMobileHide(true)
    }, [router.route])

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
                <Grid container>
                    {router.query.noLayout !== 'true' && (
                        <Grid
                            item
                            sx={{
                                zIndex: 10000,
                            }}
                            className={`no-print`}
                        >
                            {user?.isLogged == null ||
                            user.isLogged == false ? null : (
                                <Grid>
                                    <Grid
                                        onClick={() => {
                                            setIsMobileHide(!isMobileHide)
                                        }}
                                        sx={{
                                            position: 'fixed',
                                            top: 0,
                                            left: `2px`,
                                            zIndex: 10000,
                                            textAlign: 'center',
                                            display: {
                                                xs: 'block',
                                                md: 'none',
                                            },
                                        }}
                                    >
                                        <Menu
                                            fontSize={`large`}
                                            color={`inherit`}
                                            sx={{
                                                p: 1,
                                            }}
                                        />
                                    </Grid>
                                    {/* One layout left menu is used just to offset other content from left side, other is fixed to screen */}

                                    <LayoutLeftMenu
                                        mobileHide={isMobileHide}
                                        fixed
                                    />

                                    <LayoutLeftMenu mobileHide={isMobileHide} />
                                </Grid>
                            )}
                        </Grid>
                    )}
                    <Grid item flex={1} py={4}>
                        {children}
                    </Grid>
                </Grid>
            </main>
        </div>
    )
}
