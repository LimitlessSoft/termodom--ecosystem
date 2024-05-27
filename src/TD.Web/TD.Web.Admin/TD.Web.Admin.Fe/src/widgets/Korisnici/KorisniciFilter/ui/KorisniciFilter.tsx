import { ApiBase, fetchApi } from "@/app/api"
import { Grid, MenuItem, TextField } from "@mui/material"
import { useEffect, useState } from "react"
import { IKorisniciFilterProps } from "../interfaces/IKorisniciFilterProps"
import { IKorisniciFilterData } from "../interfaces/IKorisniciFilterData"

export const KorisniciFilter = (props: IKorisniciFilterProps): JSX.Element => {

    const [userTypes, setUserTypes] = useState<any[]| undefined>(undefined)
    const [professions, setProfessions] = useState<any[] | undefined>(undefined)
    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const [cities, setCities] = useState<any[] | undefined>(undefined)

    const [currentFilter, setCurrentFilter] = useState<IKorisniciFilterData>({
        filteredCity: -1,
        filteredProfession: -1,
        filteredStatus: 0,
        filteredStore: -1,
        filteredType: -1
    })

    useEffect(() => {
        fetchApi(ApiBase.Main, `/user-types`)
        .then((r) => {
            setUserTypes(r)
        })

        fetchApi(ApiBase.Main, `/professions?sortColumn=Name`)
        .then((r) => {
            setProfessions(r)
        })

        fetchApi(ApiBase.Main, `/stores?sortColumn=Name`)
        .then((r) => {
            setStores(r)
        })

        fetchApi(ApiBase.Main, `/cities?sortColumn=Name`)
        .then((r) => {
            setCities(r)
        })
    }, [])

    useEffect(() => {
        props.onFilterChange(currentFilter)
    }, [currentFilter])
    
    return (
        <Grid my={2} container spacing={2}>
            {
                userTypes !== undefined &&
                <Grid item>
                    <TextField
                        onChange={(e) => {
                            setCurrentFilter({
                                ...currentFilter,
                                filteredType: parseInt(e.target.value)
                            })
                        }}
                        defaultValue={currentFilter.filteredType}
                        select>
                        <MenuItem key={-1} value={-1}>
                            Svi tipovi
                        </MenuItem>
                        {
                            userTypes.map((ut: any) => {
                                return (
                                    <MenuItem key={ut.id} value={ut.id}>
                                        {ut.name}
                                    </MenuItem>
                                )
                            })
                        }
                    </TextField>
                </Grid>
            }

            <Grid item>
                <TextField
                    select
                    onChange={(e) => {
                        setCurrentFilter({
                            ...currentFilter,
                            filteredStatus: parseInt(e.target.value)
                        })
                    }}
                    defaultValue={currentFilter.filteredStatus}>
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

            {
                professions !== undefined &&
                <Grid item>
                    <TextField
                        onChange={(e) => {
                            setCurrentFilter({
                                ...currentFilter,
                                filteredProfession: parseInt(e.target.value)
                            })
                        }}
                        defaultValue={currentFilter.filteredProfession}
                        select>
                        <MenuItem key={-1} value={-1}>
                            Sve profesije
                        </MenuItem>
                        {
                            professions.map((p: any) => {
                                return (
                                    <MenuItem key={p.id} value={p.id}>
                                        {p.name}
                                    </MenuItem>
                                )
                            })
                        }
                    </TextField>
                </Grid>
            }

            {
                stores !== undefined &&
                <Grid item>
                    <TextField
                        onChange={(e) => {
                            setCurrentFilter({
                                ...currentFilter,
                                filteredStore: parseInt(e.target.value)
                            })
                        }}
                        defaultValue={currentFilter.filteredStore}
                        select>
                        <MenuItem key={-1} value={-1}>
                            Sve prodavnice
                        </MenuItem>
                        {
                            stores.map((s: any) => {
                                return (
                                    <MenuItem key={s.id} value={s.id}>
                                        {s.name}
                                    </MenuItem>
                                )
                            })
                        }
                    </TextField>
                </Grid>
            }

            {
                cities !== undefined &&
                <Grid item>
                    <TextField
                        onChange={(e) => {
                            setCurrentFilter({
                                ...currentFilter,
                                filteredCity: parseInt(e.target.value)
                            })
                        }}
                        defaultValue={currentFilter.filteredCity}
                        select>
                        <MenuItem key={-1} value={-1}>
                            Svi gradovi
                        </MenuItem>
                        {
                            cities.map((c: any) => {
                                return (
                                    <MenuItem key={c.id} value={c.id}>
                                        {c.name}
                                    </MenuItem>
                                )
                            })
                        }
                    </TextField>
                </Grid>
            }
        </Grid>
    )
}