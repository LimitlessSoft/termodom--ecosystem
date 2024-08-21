import {
    Grid,
    LinearProgress,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material'
import { IKorisniciFilterData } from '@/widgets/Korisnici/KorisniciFilter/interfaces/IKorisniciFilterData'
import {
    KorisniciListRow,
    KorisniciListWithoutReferentItem,
} from '@/widgets/Korisnici/KorisniciListRow'
import { MasovniSms } from '@/widgets/Korisnici'
import { useEffect, useState } from 'react'
import { KorisniciFilter } from '@/widgets'
import { useRouter } from 'next/router'
import { IKorisnikData } from '@/dtos/responses/users/IKorisnikData'
import { adminApi, handleApiError } from '@/apis/adminApi'

const Korisnici = () => {
    const userTypeColWidth = 1
    const router = useRouter()

    const [currentFilter, setCurrentFilter] = useState<
        IKorisniciFilterData | undefined
    >(undefined)
    const [usersWithoutReferent, setUsersWithoutReferent] = useState<
        IKorisnikData[] | undefined
    >(undefined)
    const [usersWithReferent, setUsersWithReferent] = useState<
        IKorisnikData[] | undefined
    >(undefined)
    const [filteredUsersWithReferent, setFilteredUsersWithReferent] = useState<
        IKorisnikData[] | undefined
    >(undefined)

    useEffect(() => {
        if (currentFilter === undefined || usersWithReferent === undefined) {
            return
        }

        setFilteredUsersWithReferent(
            usersWithReferent.filter((user) => {
                if (
                    currentFilter.filteredCities.length > 0 &&
                    !currentFilter.filteredCities.includes(user.cityId)
                )
                    return false

                if (
                    currentFilter.filteredProfessions.length > 0 &&
                    !currentFilter.filteredProfessions.includes(
                        user.professionId
                    )
                )
                    return false

                if (
                    currentFilter.filteredStatuses.length > 0 &&
                    user.isActive !== currentFilter.filteredStatuses.includes(1)
                )
                    return false

                if (
                    currentFilter.filteredStores.length > 0 &&
                    !currentFilter.filteredStores.includes(user.favoriteStoreId)
                )
                    return false

                if (
                    currentFilter.filteredTypes.length > 0 &&
                    !currentFilter.filteredTypes.includes(user.userTypeId)
                )
                    return false

                return !(
                    currentFilter.search !== '' &&
                    !Object.values(user).some(
                        (value: string | number | undefined) =>
                            value
                                ?.toString()
                                .toLowerCase()
                                .includes(currentFilter.search.toLowerCase())
                    )
                )
            })
        )
    }, [currentFilter])

    const reloadUsersWithoutReferentAsync = async () => {
        adminApi
            .get(`/users?hasReferent=false&pageSize=5000`)
            .then((response) => setUsersWithoutReferent(response.data.payload))
            .catch((err) => handleApiError(err))
    }

    const reloadUsersWithReferentAsync = async () => {
        adminApi
            .get(`/users?hasReferent=true&pageSize=5000`)
            .then((response) => setUsersWithReferent(response.data.payload))
            .catch((err) => handleApiError(err))
    }

    const redirectToKorisnik = (username: string) => {
        router.push(`/korisnici/${username}`)
    }

    useEffect(() => {
        reloadUsersWithoutReferentAsync()
        reloadUsersWithReferentAsync()
    }, [])

    return (
        <Grid container p={2} spacing={2}>
            <Grid
                item
                sm={4}
                sx={{
                    textAlign: `center`,
                }}
            >
                <Typography variant={`h6`}>Čekaju obradu</Typography>
                {usersWithoutReferent === undefined ? (
                    <LinearProgress />
                ) : usersWithoutReferent?.length == 0 ? (
                    <Typography>Nema korisnika koji čekaju obradu</Typography>
                ) : (
                    <Stack p={1} spacing={1}>
                        {usersWithoutReferent.map((user, index) => (
                            <KorisniciListWithoutReferentItem
                                key={index}
                                onClick={() => {
                                    redirectToKorisnik(user.username)
                                }}
                                user={user}
                            />
                        ))}
                    </Stack>
                )}
            </Grid>
            <Grid item sm={8}>
                <Typography variant={`h6`}>Svi korisnici</Typography>
                {usersWithReferent !== undefined && (
                    <KorisniciFilter
                        onFilterChange={(filterData: IKorisniciFilterData) => {
                            setCurrentFilter(filterData)
                        }}
                    />
                )}
                {filteredUsersWithReferent === undefined ? (
                    <LinearProgress />
                ) : filteredUsersWithReferent?.length == 0 ? (
                    <Typography>Nema korisnika</Typography>
                ) : (
                    <Grid container>
                        <MasovniSms currentFilter={currentFilter} />
                        <Grid item lg={12}>
                            <TableContainer component={Paper}>
                                <Table aria-label="Proizvodi">
                                    <TableHead>
                                        <TableRow>
                                            <TableCell
                                                align="center"
                                                width={userTypeColWidth}
                                            ></TableCell>
                                            <TableCell align="center">
                                                ID
                                            </TableCell>
                                            <TableCell align="center">
                                                Nadimak
                                            </TableCell>
                                            <TableCell align="center">
                                                Username
                                            </TableCell>
                                            <TableCell align="center">
                                                Mobilni
                                            </TableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {filteredUsersWithReferent.map(
                                            (user, index) => (
                                                <KorisniciListRow
                                                    key={index}
                                                    user={user}
                                                    userTypeColWidth={
                                                        userTypeColWidth
                                                    }
                                                    onClick={(
                                                        username: string
                                                    ) => {
                                                        redirectToKorisnik(
                                                            username
                                                        )
                                                    }}
                                                />
                                            )
                                        )}
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </Grid>
                    </Grid>
                )}
            </Grid>
        </Grid>
    )
}

export default Korisnici
