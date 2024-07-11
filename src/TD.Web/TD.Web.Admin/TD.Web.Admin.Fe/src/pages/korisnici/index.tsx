import { ApiBase, fetchApi } from "@/app/api"
import { KorisniciFilter } from "@/widgets"
import { MasovniSms } from "@/widgets/Korisnici"
import { IKorisniciFilterData } from "@/widgets/Korisnici/KorisniciFilter/interfaces/IKorisniciFilterData"
import { IKorisnikData } from "@/widgets/Korisnici/KorisniciFilter/interfaces/IKorisnikData"
import { KorisniciSearchFilter } from "@/widgets/Korisnici/KorisniciFilter/ui/KorisniciSearchFilter"
import { KorisniciListRow, KorisniciListWithoutReferentItem } from "@/widgets/Korisnici/KorisniciListRow"
import { Button, Dialog, DialogContent, Grid, LinearProgress, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField, Typography } from "@mui/material"
import { useRouter } from "next/router"
import { useEffect, useState } from "react"

const Korisnici = () => {

    const userTypeColWidth = 1
    const router = useRouter()
    
    const [currentFilter, setCurrentFilter] = useState<IKorisniciFilterData | undefined>(undefined)
    const [usersWithoutReferent, setUsersWithoutReferent] = useState<any[] | undefined>(undefined)
    const [usersWithReferent, setUsersWithReferent] = useState<IKorisnikData[] | undefined>(undefined)
    const [filteredUsersWithReferent, setFilteredUsersWithReferent] = useState<IKorisnikData[] | undefined>(undefined)
    
    useEffect(() => {
        if (currentFilter === undefined || usersWithReferent === undefined) {
            return
        }

        setFilteredUsersWithReferent(usersWithReferent.filter((user) => {

            if (currentFilter.filteredCity !== -1 && user.cityId !== currentFilter.filteredCity) {
                return false
            }

            if (currentFilter.filteredProfession !== -1 && user.professionId !== currentFilter.filteredProfession) {
                return false
            }

            if (currentFilter.filteredStatus !== 0 && user.isActive !== (currentFilter.filteredStatus == 1)) {
                return false
            }

            if (currentFilter.filteredStore !== -1 && user.favoriteStoreId !== currentFilter.filteredStore) {
                return false
            }

            if (currentFilter.filteredType !== -1 && user.userTypeId !== currentFilter.filteredType) {
                return false
            }

            if (currentFilter.search !== '' && !Object.values(user).some((value: string | number | undefined) => 
                value?.toString().toLowerCase().includes(currentFilter.search.toLowerCase()))) {
                return false;
            }

            return true
        }))
    }, [currentFilter])

    const reloadUsersWithoutReferentAsync = async () => {
        await fetchApi(ApiBase.Main, `/users?hasReferent=false&pageSize=5000`)
        .then((response) => response.json())
        .then((data) => setUsersWithoutReferent(data.payload))
    }

    const reloadUsersWithReferentAsync = async () => {
        await fetchApi(ApiBase.Main, `/users?hasReferent=true&pageSize=5000`)
        .then((response) => response.json())
        .then((data) => setUsersWithReferent(data.payload))
    }

    const redirectToKorisnik = (username: string) => {
        router.push(`/korisnici/${username}`)
    }

    useEffect(() => {
        reloadUsersWithoutReferentAsync()
        reloadUsersWithReferentAsync()
    }, []);

    const onSearchUsersHandler = (searchTerm: string) => {
        setCurrentFilter((prevFilters) => {
            const defaultFilters: IKorisniciFilterData = {
                filteredType: 0,
                filteredProfession: 0,
                filteredStore: 0,
                filteredStatus: 0,
                filteredCity: 0,
                search: ''
            };
    
            return {
                ...defaultFilters,
                ...prevFilters,
                search: searchTerm
            };
        });
    };

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
                    usersWithReferent !== undefined && 
                    <Grid>
                        <KorisniciFilter search={currentFilter?.search ?? ''} onFilterChange={(filterData: IKorisniciFilterData) => {
                            setCurrentFilter(filterData)
                        }} />
                        <KorisniciSearchFilter onSearchUsers={onSearchUsersHandler} />
                    </Grid>
                }
                {
                    filteredUsersWithReferent === undefined ?
                        <LinearProgress /> :
                        filteredUsersWithReferent?.length == 0 ?
                            <Typography>
                                Nema korisnika
                            </Typography> :
                            <Grid container>
                                <MasovniSms currentFilter={currentFilter} />
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
                                                filteredUsersWithReferent.map((user, index) =>
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