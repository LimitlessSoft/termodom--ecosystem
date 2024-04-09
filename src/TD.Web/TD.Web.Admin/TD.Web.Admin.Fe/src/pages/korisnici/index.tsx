import { ApiBase, fetchApi } from "@/app/api"
import { MasovniSms } from "@/widgets/Korisnici"
import { KorisniciListRow, KorisniciListWithoutReferentItem } from "@/widgets/Korisnici/KorisniciListRow"
import { Button, Dialog, DialogContent, Grid, LinearProgress, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography } from "@mui/material"
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
            <Grid item sm={4}
                sx={{
                    textAlign: `center`
                }}>
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
                        <Stack
                            p={1}
                            spacing={1}>
                            {
                                usersWithoutReferent.map((user, index) => (
                                    <KorisniciListWithoutReferentItem
                                        key={index}
                                        onClick={() => {
                                            redirectToKorisnik(user.username)
                                        }}
                                        user={user} />
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
                            <Grid container>
                                <MasovniSms />
                                <Grid item lg={12}>
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
                                </Grid>
                            </Grid>
                }
            </Grid>
        </Grid>
    )
}

export default Korisnici