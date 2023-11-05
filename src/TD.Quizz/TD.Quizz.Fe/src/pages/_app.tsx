import { Layout } from "@/widgets/Layout";
import { ThemeProvider } from "@mui/material";
import { AppProps } from "next/app";
import { ToastContainer } from "react-toastify";
import './../app/globals.css'
import { Provider } from "react-redux";
import { store } from "@/app/store";

export default function MyApp({ Component, pageProps }: AppProps) {
    return (
        <Provider store={store}>
            <Layout>
                <ThemeProvider theme={{}}>
                    <ToastContainer />
                    <Component {...pageProps} />
                </ThemeProvider>
            </Layout>
        </Provider>
    )
}