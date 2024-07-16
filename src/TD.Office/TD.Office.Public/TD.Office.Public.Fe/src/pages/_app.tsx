import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { ThemeProvider } from '@mui/material/styles'
import { ToastContainer } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'
import { Provider } from 'react-redux'
import { AppProps } from 'next/app'
import { Layout } from '@/widgets'
import { mainTheme } from '@/themes'
import { store } from '@/store'
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
