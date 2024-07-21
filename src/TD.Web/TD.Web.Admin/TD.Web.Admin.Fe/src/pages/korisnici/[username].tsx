import {
    KorisnikBody,
    KorisnikCene,
    KorisnikHeader,
} from '@/widgets/Korisnici/Korisnik'
import { CircularProgress, Grid } from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { adminApi } from '@/apis/adminApi'
import { KorisnikAdminSettings } from '@/widgets/Korisnici/Korisnik/ui/KorisnikAdminSettings'

const Korisnik = () => {
    const router = useRouter()
    const username = router.query.username

    const [loading, setLoading] = useState<boolean>(true)

    const [user, setUser] = useState<any | undefined>(undefined)

    const reloadData = (un: string) => {
        setLoading(true)

        adminApi.get(`/users/${un}`).then((response) => {
            setLoading(false)
            setUser(response.data)
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
                    {user.type === 1 && (
                        <KorisnikAdminSettings username={username} />
                    )}
                </Grid>
            )}
        </Grid>
    )
}

export default Korisnik
