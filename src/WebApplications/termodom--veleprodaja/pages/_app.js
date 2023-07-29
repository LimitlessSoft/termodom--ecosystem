import { AppProps } from "next/app"
import Header from "../components/header"
import styles from "./_app.module.css"
import globalStyles from "../src/styles/global.css"

export default function MyApp({ Component, pageProps }) {
    return (
        <div className={`${styles.main}`}>
            <Header />
            <Component {...pageProps} />
        </div>
    )
}