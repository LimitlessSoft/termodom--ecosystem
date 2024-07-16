import { HorizontalActionBar, HorizontalActionBarButton, KorisniciSingular } from "@/widgets"
import { CircularProgress, Grid, Typography } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"
import {officeApi} from "@/apis/officeApi";

const KorisniciId = (): JSX.Element => {

    const router = useRouter()

    const [id, setId] = useState<number | undefined>(undefined)
    const [data, setData] = useState<any | undefined>(undefined)

    useEffect(() => {
        if (router.query.id)
            setId(Number(router.query.id))
        else
            setId(undefined)
    }, [router, router.query.id])

    useEffect(() => {

        setData(undefined)
        
        if(id === undefined)
            return

        officeApi.get(`/users/${id}`)
            .then((response: any) => {
                setData(response.data)
            })
    }, [id])

    return (
        <Grid>
            <HorizontalActionBar>
                <HorizontalActionBarButton text="Nazad" onClick={() => router.push(`/korisnici`)} />
            </HorizontalActionBar>
            { data === undefined
                ? <CircularProgress />
                : data === null
                    ? <Typography>Korisnik nije pronađen</Typography>
                    : <KorisniciSingular user={data} />
            }
        </Grid>
    )
}

export default KorisniciId