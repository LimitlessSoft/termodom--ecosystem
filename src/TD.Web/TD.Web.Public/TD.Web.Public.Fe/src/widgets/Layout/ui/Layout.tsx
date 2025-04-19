import Footer from '@/widgets/Footer/ui/Footer'
import { Header } from '@/widgets/Header'
import { ReactNode } from 'react'
import Head from 'next/head'

interface ILayoutProps {
    children: ReactNode
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    const { children } = props

    return (
        <>
            <Head>
                <meta
                    name="viewport"
                    content="width=device-width, initial-scale=1.0"
                ></meta>
            </Head>
            <div className={`mainWrapper`}>
                <Header />
                <main style={{ position: 'relative' }}>{children}</main>
                <Footer />
            </div>
        </>
    )
}
