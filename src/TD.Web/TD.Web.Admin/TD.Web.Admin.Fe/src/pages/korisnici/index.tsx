import { ApiBase, fetchApi } from "@/app/api"
import { KorisniciListRow } from "@/widgets/Korisnici/KorisniciListRow"
import { Grid, LinearProgress, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"

const Korisnici = (): JSX.Element => {

    const userTypeColWidth = 1
    const router = useRouter()
    
    const [usersWithoutReferent, setUsersWithoutReferent] = useState<any[] | undefined>(undefined)
    const [usersWithReferent, setUsersWithReferent] = useState<any[] | undefined>(undefined)

    const reloadUsersWithoutReferentAsync = async () => {
        await fetchApi(ApiBase.Main, `/users?hasReferent=false&pageSize=5000`)
        .then((response) => {
            setUsersWithoutReferent(response)
        })
    }

    const reloadUsersWithReferentAsync = async () => {
        await fetchApi(ApiBase.Main, `/users?hasReferent=true&pageSize=5000`)
        .then((response) => {
            setUsersWithReferent(response)
        })
    }

    const redirectToKorisnik = (username: string) => {
        router.push(`/korisnici/${username}`)
    }

    useEffect(() => {
        reloadUsersWithoutReferentAsync()
        reloadUsersWithReferentAsync()
    }, [])

    return (
        <Grid container
            p={2}
            spacing={2}>
            <Grid item sm={4}>
                <Typography variant={`h6`}>
                    Čekaju obradu
                </Typography>
                {
                    usersWithoutReferent === undefined ?
                    <LinearProgress /> :
                        usersWithoutReferent?.length == 0 ?
                        <Typography>
                            Nema korisnika koji čekaju obradu
                        </Typography> :
                        <Stack>
                            {
                                usersWithoutReferent.map((user, index) => (
                                    <Typography key={index} onClick={() => {
                                        redirectToKorisnik(user.username)
                                    }}>
                                        {user.nickname} {user.username}
                                    </Typography>
                                ))
                            }
                        </Stack>
                }
            </Grid>
            <Grid item sm={8}>
                <Typography variant={`h6`}>
                    Svi korisnici
                </Typography>
                {
                    usersWithReferent === undefined ?
                        <LinearProgress /> :
                            usersWithReferent?.length == 0 ?
                            <Typography>
                                Nema korisnika
                            </Typography> :
                            <TableContainer component={Paper}>
                                <Table aria-label="Proizvodi">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell align="center" width={userTypeColWidth}></TableCell>
                                            <TableCell align="center">ID</TableCell>
                                            <TableCell align="center">Nadimak</TableCell>
                                            <TableCell align="center">Username</TableCell>
                                            <TableCell align="center">Mobilni</TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                    {
                                        usersWithReferent.map((user, index) =>
                                            <KorisniciListRow key={index}
                                                user={user}
                                                userTypeColWidth={userTypeColWidth}
                                                onClick={(username: string) => {
                                                    redirectToKorisnik(username)
                                                }}/>
                                        )
                                    }
                                    </TableBody>
                                </Table>
                            </TableContainer>
                }
            </Grid>
        </Grid>
    )
}

export default Korisnici