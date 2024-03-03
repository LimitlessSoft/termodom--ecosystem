import { ApiBase, fetchApi } from "@/app/api"
import { KorisnikBody, KorisnikCene, KorisnikHeader } from "@/widgets/Korisnici/Korisnik"
import { CircularProgress, Grid } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"

const Korisnik = (): JSX.Element => {

    const router = useRouter()
    const username = router.query.username

    const [user, setUser] = useState<any | undefined>(undefined)

    useEffect(() => {
        if (username === undefined) return
        fetchApi(ApiBase.Main, `/users/${username}`)
        .then((response) => {
            setUser(response)
        })
    }, [username])
    
    return (
        <Grid>
            {
                user === undefined ?
                    <CircularProgress /> :
                    <Grid container
                        justifyContent={`center`}>
                        
                        <KorisnikHeader user={user} />
                        <KorisnikBody user={user} />
                        <KorisnikCene user={user} />
                    </Grid>
            }
        </Grid>
    )
}

export default Korisnik