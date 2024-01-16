import { Header } from "@/widgets/Header"
import { ReactNode } from "react"
import styles from "./Layout.module.css"
import { useUser } from "@/app/hooks";

interface ILayoutProps {
    children: ReactNode;
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    
    const user = useUser(true, true)

    const { children } = props;

    return (
        <div className={`mainWrapper`}>
            <Header />
            <main>{children}</main>
        </div>
    )
}