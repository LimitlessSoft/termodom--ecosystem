import { KorisniciFilterSearch } from '@/widgets/Korisnici/KorisniciFilter/ui/KorisniciFilterSearch'
import { IKorisniciFilterProps } from '../interfaces/IKorisniciFilterProps'
import { IKorisniciFilterData } from '../interfaces/IKorisniciFilterData'
import { Grid, MenuItem, TextField } from '@mui/material'
import { useEffect, useState } from 'react'
import { adminApi } from '@/apis/adminApi'

export const KorisniciFilter = (props: IKorisniciFilterProps): JSX.Element => {
    const [userTypes, setUserTypes] = useState<any[] | undefined>(undefined)
    const [professions, setProfessions] = useState<any[] | undefined>(undefined)
    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const [cities, setCities] = useState<any[] | undefined>(undefined)

    const [currentFilter, setCurrentFilter] = useState<IKorisniciFilterData>({
        filteredCity: -1,
        filteredProfession: -1,
        filteredStatus: 0,
        filteredStore: -1,
        filteredType: -1,
        search: '',
    })

    useEffect(() => {
        adminApi.get(`/user-types`).then((response) => {
            setUserTypes(response.data)
        })

        adminApi.get(`/professions?sortColumn=Name`).then((response) => {
            setProfessions(response.data)
        })

        adminApi.get(`/stores?sortColumn=Name`).then((response) => {
            setStores(response.data)
        })

        adminApi.get(`/cities?sortColumn=Name`).then((response) => {
            setCities(response.data)
        })
    }, [])

    useEffect(() => {
        props.onFilterChange(currentFilter)
    }, [currentFilter])

    return (
        <Grid my={2} container spacing={2}>
            {userTypes !== undefined && (
                <Grid item>
                    <TextField
                        onChange={(e) => {
                            setCurrentFilter({
                                ...currentFilter,
                                filteredType: parseInt(e.target.value),
                            })
                        }}
                        defaultValue={currentFilter.filteredType}
                        select
                    >
                        <MenuItem key={-1} value={-1}>
                            Svi tipovi
                        </MenuItem>
                        {userTypes.map((ut: any) => {
                            return (
                                <MenuItem key={ut.id} value={ut.id}>
                                    {ut.name}
                                </MenuItem>
                            )
                        })}
                    </TextField>
                </Grid>
            )}

            <Grid item>
                <TextField
                    select
                    onChange={(e) => {
                        setCurrentFilter({
                            ...currentFilter,
                            filteredStatus: parseInt(e.target.value),
                        })
                    }}
                    defaultValue={currentFilter.filteredStatus}
                >
                    <MenuItem key={0} value={0}>
                        Svi statusi
                    </MenuItem>
                    <MenuItem key={1} value={1}>
                        Aktivni
                    </MenuItem>
                    <MenuItem key={2} value={2}>
                        Neaktivni
                    </MenuItem>
                </TextField>
            </Grid>

            {professions !== undefined && (
                <Grid item>
                    <TextField
                        onChange={(e) => {
                            setCurrentFilter({
                                ...currentFilter,
                                filteredProfession: parseInt(e.target.value),
                            })
                        }}
                        defaultValue={currentFilter.filteredProfession}
                        select
                    >
                        <MenuItem key={-1} value={-1}>
                            Sve profesije
                        </MenuItem>
                        {professions.map((p: any) => {
                            return (
                                <MenuItem key={p.id} value={p.id}>
                                    {p.name}
                                </MenuItem>
                            )
                        })}
                    </TextField>
                </Grid>
            )}

            {stores !== undefined && (
                <Grid item>
                    <TextField
                        onChange={(e) => {
                            setCurrentFilter({
                                ...currentFilter,
                                filteredStore: parseInt(e.target.value),
                            })
                        }}
                        defaultValue={currentFilter.filteredStore}
                        select
                    >
                        <MenuItem key={-1} value={-1}>
                            Sve prodavnice
                        </MenuItem>
                        {stores.map((s: any) => {
                            return (
                                <MenuItem key={s.id} value={s.id}>
                                    {s.name}
                                </MenuItem>
                            )
                        })}
                    </TextField>
                </Grid>
            )}

            {cities !== undefined && (
                <Grid item>
                    <TextField
                        onChange={(e) => {
                            setCurrentFilter({
                                ...currentFilter,
                                filteredCity: parseInt(e.target.value),
                            })
                        }}
                        defaultValue={currentFilter.filteredCity}
                        select
                    >
                        <MenuItem key={-1} value={-1}>
                            Svi gradovi
                        </MenuItem>
                        {cities.map((c: any) => {
                            return (
                                <MenuItem key={c.id} value={c.id}>
                                    {c.name}
                                </MenuItem>
                            )
                        })}
                    </TextField>
                </Grid>
            )}

            <Grid item sm={12}>
                <KorisniciFilterSearch
                    onSearchUsers={(e) => {
                        setCurrentFilter({
                            ...currentFilter,
                            search: e,
                        })
                    }}
                />
            </Grid>
        </Grid>
    )
}
