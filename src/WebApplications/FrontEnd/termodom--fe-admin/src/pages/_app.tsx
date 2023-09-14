import { AppProps } from "next/app"
import { Layout } from "../widgets/Layout"
import './../app/global.css'
import { Provider } from "react-redux"
import { store } from './../app/store'

export default function MyApp({ Component, pageProps }: AppProps) {
    return (
        <Provider store={store}>
            <Layout>
                <Component {...pageProps} />
            </Layout>
        </Provider>
    )
}