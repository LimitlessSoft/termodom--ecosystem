import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { LocalizationProvider } from '@mui/x-date-pickers';
import { ThemeProvider } from "@mui/material/styles"
import { ToastContainer } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'
import { mainTheme } from "../app/themes"
import { Provider } from "react-redux"
import { store } from "../app/store"
import { Layout } from "../widgets"
import { AppProps } from "next/app"
import '../app/global.css'

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
