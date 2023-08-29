import { AppProps } from "next/app"
import { Layout } from "../widgets/Layout"
import './../app/global.css'

export default function MyApp({ Component, pageProps }: AppProps) {
    return (
        <Layout>
            <Component {...pageProps} />
        </Layout>
    )
}