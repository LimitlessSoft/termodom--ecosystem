import { KorisnikAnalizaRobe } from "@/widgets/Korisnici"
import { Grid } from "@mui/material"
import { useRouter } from "next/router"

const KorisnikAnaliza = () => {

    const router = useRouter()
    const { username } = router.query

    return (
        <Grid container spacing={2} p={2}>
            <KorisnikAnalizaRobe username={username} />
        </Grid>
    )
}

export default KorisnikAnaliza