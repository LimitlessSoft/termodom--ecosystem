import { ILayoutProps } from "../interfaces/ILayoutProps"
import { LayoutLeftMenu } from "./LayoutLeftMenu"
import { useUser } from "@/hooks/useUserHook"
import { useRouter } from "next/router"
import { Grid } from "@mui/material"
import Head from "next/head"

export const Layout = (props: ILayoutProps): JSX.Element => {

    const { children } = props
    const user = useUser(true, true)
    const router = useRouter()

    return (
        <div className={`mainWrapper`}>
            <Head>
                <title>TDOffice</title>
                <meta name="viewport" content="width=device-width, initial-scale=1.0"></meta>
            </Head>
            <main>
                <Grid container>
                    {
                        router.query.noLayout !== 'true'
                        && <Grid item>
                            {
                                user?.isLogged == null || user.isLogged == false ?
                                    null :
                                    <Grid>
                                        {/* One layout left menu is used just to offset other content from left side, other is fixed to screen */}
                                        <LayoutLeftMenu fixed/>
                                        <LayoutLeftMenu />
                                    </Grid>
                            }
                        </Grid>
                    }
                    <Grid item flex={1}>{children}</Grid>
                </Grid>
            </main>
        </div>
    )
}