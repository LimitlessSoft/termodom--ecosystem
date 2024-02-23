import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { ThemeProvider } from "@mui/material/styles"
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
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <Provider store={store}>
                <ThemeProvider theme={mainTheme}>
                    <Layout>
                            <ToastContainer
                                position="top-center"/>
                            <Component {...pageProps} />
                    </Layout>
                </ThemeProvider>
            </Provider>
        </LocalizationProvider>
    )
}