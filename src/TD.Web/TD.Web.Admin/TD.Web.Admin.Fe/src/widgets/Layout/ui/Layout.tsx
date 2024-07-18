import { CircularProgress } from '@mui/material'
import { useUser } from '@/hooks/useUserHook'
import { ReactNode, useEffect } from 'react'
import { Header } from '@/widgets/Header'
import { useRouter } from 'next/router'
import Head from 'next/head'

interface ILayoutProps {
    children: ReactNode
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    const user = useUser(true, true)
    const router = useRouter()

    const { children } = props

    useEffect(() => {}, [user, user.isLoading])

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
            <Header />
            <main>{children}</main>
        </div>
    )
}
