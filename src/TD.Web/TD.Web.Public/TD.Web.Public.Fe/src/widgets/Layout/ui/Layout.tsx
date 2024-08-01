import { CustomHead } from '@/widgets/CustomHead'
import Footer from '@/widgets/Footer/ui/Footer'
import { Header } from '@/widgets/Header'
import { ReactNode } from 'react'

interface ILayoutProps {
    children: ReactNode
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    const { children } = props

    return (
        <div className={`mainWrapper`}>
            <Header />
            <main>{children}</main>
            <Footer />
        </div>
    )
}
