import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { LocalizationProvider } from '@mui/x-date-pickers'
import { ThemeProvider } from '@mui/material/styles'
import { ToastContainer } from 'react-toastify'
import 'react-toastify/dist/ReactToastify.css'
import { Provider } from 'react-redux'
import { Layout } from '@/widgets'
import { mainTheme } from '@/themes'
import { store } from '@/store'
import '../app/global.css'
import ActiveLayout from '@/widgets/ActiveLayout/ui/ActiveLayout'

export default function MyApp({ Component, pageProps }) {
    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <Provider store={store}>
                <Layout>
                    <ThemeProvider theme={mainTheme}>
                        <ActiveLayout>
                            <ToastContainer />
                            <Component {...pageProps} />
                        </ActiveLayout>
                    </ThemeProvider>
                </Layout>
            </Provider>
        </LocalizationProvider>
    )
}
