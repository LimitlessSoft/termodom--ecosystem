import { ThemeProvider, createTheme } from "@mui/material/styles"
import { ToastContainer } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'
import { Layout } from "../widgets/Layout"
import { mainTheme } from "../app/theme"
import { Provider } from "react-redux"
import { store } from './../app/store'
import { AppProps } from "next/app"
import './../app/global.css'

export default function MyApp({ Component, pageProps }: AppProps) {
    return (
        <Provider store={store}>
            <ThemeProvider theme={mainTheme}>
                <Layout>
                    <ToastContainer />
                    <Component {...pageProps} />
                </Layout>
            </ThemeProvider>
        </Provider>
    )
}