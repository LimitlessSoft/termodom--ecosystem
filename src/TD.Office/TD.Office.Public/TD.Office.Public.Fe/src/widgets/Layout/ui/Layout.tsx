import { Grid } from "@mui/material"
import Head from "next/head"
import { ReactNode } from "react"
import { LayoutLeftMenu } from "./LayoutLeftMenu"
import { useUser } from "@/app/hooks"

interface ILayoutProps {
    children: ReactNode
}

export const Layout = (props: ILayoutProps): JSX.Element => {

    const { children } = props
    const user = useUser(false, true)

    return (
        <div className={`mainWrapper`}>
            <Head>
                <title>TDOffice</title>
                <meta name="viewport" content="width=device-width, initial-scale=1.0"></meta>
            </Head>
            <main>
                <Grid container>
                    <Grid item>
                        {
                            user?.isLogged == null || user.isLogged == false ?
                                null :
                                <Grid>
                                    {/* One layout left menu is used just t ofset other content from left side, other is fixed to screen */}
                                    <LayoutLeftMenu fixed/>
                                    <LayoutLeftMenu />
                                </Grid>
                        }
                    </Grid>
                    <Grid item>{children}</Grid>
                </Grid>
            </main>
        </div>
    )
}