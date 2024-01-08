import { Layout } from "../widgets/Layout"
import { AppProps } from "next/app"
import { ToastContainer } from 'react-toastify'
import { ThemeProvider } from "@mui/material/styles"
import { mainTheme } from "../app/themes"
import { Provider } from "react-redux"
import { store } from "../app/store"
import 'react-toastify/dist/ReactToastify.css'
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
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
