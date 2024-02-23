import { Header } from "@/widgets/Header"
import { ReactNode } from "react"
import { useUser } from "@/app/hooks";
import { CircularProgress } from "@mui/material";
import Head from "next/head";

interface ILayoutProps {
    children: ReactNode;
}

export const Layout = (props: ILayoutProps): JSX.Element => {
    
    const user = useUser(true, true)

    const { children } = props;

    return (
        user == null || user.isLoading ?
            <CircularProgress /> :
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