import Footer from '@/widgets/Footer/ui/Footer'
import { Header } from '@/widgets/Header'
import { ReactNode, useEffect } from 'react'
import Head from 'next/head'

interface ILayoutProps {
    children: ReactNode
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    const { children } = props

    useEffect(() => {
        const viewport = document.querySelector('meta[name=viewport]')
        if (viewport) {
            viewport.setAttribute(
                'content',
                'width=device-width, initial-scale=1'
            )
        }
    }, [])

    return (
        <>
            <div className={`mainWrapper`}>
                <Header />
                <main style={{ position: 'relative' }}>{children}</main>
                <Footer />
            </div>
        </>
    )
}
