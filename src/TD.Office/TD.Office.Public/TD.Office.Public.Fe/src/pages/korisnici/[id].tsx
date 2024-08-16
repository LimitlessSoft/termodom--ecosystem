import {
    HorizontalActionBar,
    HorizontalActionBarButton,
    KorisniciSingular,
} from '@/widgets'
import { CircularProgress, Grid, Typography } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'

const KorisniciId = () => {
    const router = useRouter()

    const [id, setId] = useState<number | undefined>(undefined)
    const [data, setData] = useState<any | undefined>(undefined)

    useEffect(() => {
        if (router.query.id) setId(Number(router.query.id))
        else setId(undefined)
    }, [router, router.query.id])

    useEffect(() => {
        setData(undefined)

        if (!id) return

        officeApi
            .get(`/users/${id}`)
            .then((response: any) => {
                setData(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [id])

    return (
        <Grid>
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text="Nazad"
                    onClick={() => router.push(`/korisnici`)}
                />
            </HorizontalActionBar>
            {data === undefined ? (
                <CircularProgress />
            ) : data === null ? (
                <Typography>Korisnik nije pronađen</Typography>
            ) : (
                <KorisniciSingular user={data} />
            )}
        </Grid>
    )
}

export default KorisniciId
