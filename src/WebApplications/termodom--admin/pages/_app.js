import { AppProps } from "next/app"
import Header from "../components/header"
import styles from "./_app.module.css"
import globalStyles from "../src/styles/global.css"
import { useEffect, useState } from "react"
import { apiGetAsync } from "../api/api"
import Login from "../components/login/login"

export default function MyApp({ Component, pageProps }) {

    const [isAuthenticated, setIsAuthenticated] = useState(false)
    
    useEffect(() => {
    }, [])

    return (
        <div className={`${styles.main}`}>
            <Header setIsAuthenticated={setIsAuthenticated} />
            {
                isAuthenticated ?
                <Component {...pageProps} /> :
                <Login />
            }
        </div>
    )
}