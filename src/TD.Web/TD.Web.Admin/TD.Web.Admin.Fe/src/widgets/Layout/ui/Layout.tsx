import { Header } from "@/widgets/Header"
import { ReactNode } from "react"
import { useUser } from "@/app/hooks";
import { CircularProgress } from "@mui/material";

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
                <Header />
                <main>{children}</main>
            </div>
    )
}