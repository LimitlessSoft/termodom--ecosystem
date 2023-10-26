import { Header } from "@/widgets/Header"
import { ReactNode } from "react"
import styles from "./Layout.module.css"

interface ILayoutProps {
    children: ReactNode;
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    const { children } = props;
    return (
        <div className={`mainWrapper`}>
            <Header />
            <main>{children}</main>
        </div>
    )
}