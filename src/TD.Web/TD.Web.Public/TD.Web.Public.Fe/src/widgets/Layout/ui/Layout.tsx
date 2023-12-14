import { Header } from "@/widgets/Header"
import Head from "next/head";
import { ReactNode } from "react"

interface ILayoutProps {
    children: ReactNode;
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    const { children } = props;
    return (
        <div className={`mainWrapper`}>
            <Head>
                <title>Termodom</title>
                <meta name="viewport" content="width=device-width, initial-scale=1.0"></meta>
            </Head>
            <Header />
            <main>{children}</main>
        </div>
    )
}