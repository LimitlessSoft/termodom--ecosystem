import {
    HorizontalActionBar,
    HorizontalActionBarButton,
    KorisniciNovaLozinka,
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

    const [novaLozinkaIsOpen, setNovaLozinkaIsOpen] = useState<boolean>(false)

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
            <KorisniciNovaLozinka
                id={id}
                isOpen={novaLozinkaIsOpen}
                onClose={() => {
                    setNovaLozinkaIsOpen(false)
                }}
            />
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text="Nazad"
                    onClick={() => router.push(`/korisnici`)}
                />
                <HorizontalActionBarButton
                    color={`secondary`}
                    text="Postavi novu lozinku"
                    onClick={() => setNovaLozinkaIsOpen(true)}
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
