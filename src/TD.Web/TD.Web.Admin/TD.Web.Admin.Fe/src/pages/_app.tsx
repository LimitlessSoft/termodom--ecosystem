import { AppProps } from "next/app"
import { Layout } from "../widgets/Layout"
import './../app/global.css'
import { Provider } from "react-redux"
import { store } from './../app/store'
import { ThemeProvider, createTheme } from "@mui/material/styles"
import { mainTheme } from "../app/theme"


export default function MyApp({ Component, pageProps }: AppProps) {
    return (
        <Provider store={store}>
            <Layout>
                <ThemeProvider theme={mainTheme}>
                    <Component {...pageProps} />
                </ThemeProvider>
            </Layout>
        </Provider>
    )
}