import { PorudzbinaActionBar } from "@/widgets/Porudzbine/PorudzbinaActionbar"
import { PorudzbinaAdminInfo } from "@/widgets/Porudzbine/PorudzbinaAdminInfo"
import { PorudzbinaSummary } from "@/widgets/Porudzbine/PorudzbinaSummary"
import { PorudzbinaHeader } from "@/widgets/Porudzbine/PorudzbinaHeader"
import { PorudzbinaItems } from "@/widgets/Porudzbine/PorudzbinaItems"
import { IPorudzbina } from "@/widgets/Porudzbine/models/IPorudzbina"
import { CircularProgress, Grid } from "@mui/material"
import { UIDimensions } from "@/app/constants"
import { ApiBase, fetchApi } from "@/app/api"
import { useEffect, useState } from "react"
import { useRouter } from "next/router"

const Porudzbina = (): JSX.Element => {

    const router = useRouter()
    const oneTimeHash = router.query.hash
    
    const [porudzbina, setPorudzbina] = useState<IPorudzbina | undefined>(undefined)

    useEffect(() => {

        if(oneTimeHash == null) {
            setPorudzbina(undefined)
            return
        }

        fetchApi(ApiBase.Main, `/orders/${oneTimeHash}`)
        .then((r) => {
            setPorudzbina(r)
        })

    }, [oneTimeHash])

    return (
        porudzbina === undefined ?
        <CircularProgress /> :
        <Grid
            sx={{
                maxWidth: UIDimensions.maxWidth,
                margin: `auto`,
            }}>
            <PorudzbinaHeader porudzbina={porudzbina} />
            <PorudzbinaActionBar porudzbina={porudzbina} />
            <PorudzbinaAdminInfo porudzbina={porudzbina} />
            <PorudzbinaItems porudzbina={porudzbina} />
            <PorudzbinaSummary porudzbina={porudzbina} />
        </Grid>
    )
}

export default Porudzbina