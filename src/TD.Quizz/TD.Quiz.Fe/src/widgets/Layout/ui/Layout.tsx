import { ReactNode } from "react";

interface ILayoutProps {
    children: ReactNode
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    const { children } = props

    return (
        <div>
            <main>{ children }</main>
        </div>
    )
}