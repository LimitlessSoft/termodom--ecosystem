import { ApiBase, fetchApi } from "@/app/api"
import { useEffect } from "react"

const Home = (): JSX.Element => {
    useEffect(() => {
        fetchApi(ApiBase.Main, "/ping")
        .then(res => console.log(res))
    }, [])
    
    return (
        <div>
            Home 1
        </div>
    )
}

export default Home