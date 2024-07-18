import { ApiBase, fetchApi } from '@/api'
import {
    KorisnikBody,
    KorisnikCene,
    KorisnikHeader,
} from '@/widgets/Korisnici/Korisnik'
import { CircularProgress, Grid } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'

const Korisnik = (): JSX.Element => {
    const router = useRouter()
    const username = router.query.username

    const [loading, setLoading] = useState<boolean>(true)

    const [user, setUser] = useState<any | undefined>(undefined)

    const reloadData = (un: string) => {
        setLoading(true)
        fetchApi(ApiBase.Main, `/users/${un}`)
            .then((response) => response.json())
            .then((data) => {
                setLoading(false)
                setUser(data)
            })
    }

    useEffect(() => {
        if (username === undefined) return

        reloadData(username.toString())
    }, [username])

    return (
        <Grid>
            {user === undefined ? (
                <CircularProgress />
            ) : (
                <Grid container justifyContent={`center`}>
                    <KorisnikHeader
                        user={user}
                        disabled={loading || user.AmIOwner == false}
                    />
                    <KorisnikBody
                        user={user}
                        disabled={loading || user.AmIOwner == false}
                        onRealoadRequest={() => {
                            reloadData(user.username)
                        }}
                    />
                    <KorisnikCene
                        user={user}
                        disabled={loading || user.AmIOwner == false}
                    />
                </Grid>
            )}
        </Grid>
    )
}

export default Korisnik
