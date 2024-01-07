import { AppProps } from "next/app"
import { Layout } from "../widgets/Layout"
import './../app/global.css'
import { Provider } from "react-redux"
import { ToastContainer } from 'react-toastify'
import { store } from './../app/store'
import { ThemeProvider, createTheme } from "@mui/material/styles"
import { mainTheme } from "../app/theme"
import 'react-toastify/dist/ReactToastify.css'
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'

export default function MyApp({ Component, pageProps }: AppProps) {
    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <Provider store={store}>
                <Layout>
                    <ThemeProvider theme={mainTheme}>
                        <ToastContainer />
                        <Component {...pageProps} />
                    </ThemeProvider>
                </Layout>
            </Provider>
        </LocalizationProvider>
    )
}