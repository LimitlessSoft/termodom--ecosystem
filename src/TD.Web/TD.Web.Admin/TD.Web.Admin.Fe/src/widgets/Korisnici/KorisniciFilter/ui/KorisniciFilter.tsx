import { IKorisniciFilterProps } from '../interfaces/IKorisniciFilterProps'
import { IKorisniciFilterData } from '../interfaces/IKorisniciFilterData'
import {
    Badge,
    Box,
    Chip,
    FormControl,
    Grid,
    InputLabel,
    Select,
    Stack,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { adminApi } from '@/apis/adminApi'
import { KorisniciListCheckBoxFilter } from './KorisniciListCheckBoxFilter'
import { KorisniciFilterSearch } from './KorisniciFilterSearch'
import { USER_FILTERS, USER_STATUSES } from '@/constants'
import { Close } from '@mui/icons-material'

export const KorisniciFilter = ({
    onFilterChange,
}: IKorisniciFilterProps): JSX.Element => {
    const [userTypes, setUserTypes] = useState<any[] | undefined>(undefined)
    const [professions, setProfessions] = useState<any[] | undefined>(undefined)
    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const [cities, setCities] = useState<any[] | undefined>(undefined)

    const [currentFilter, setCurrentFilter] = useState<IKorisniciFilterData>({
        filteredCities: [],
        filteredProfessions: [],
        filteredStatuses: [],
        filteredStores: [],
        filteredTypes: [],
        search: '',
    })

    const chipIterationFilter = (propertyName: string): boolean => {
        const visibleOnes = [
            'filteredCities',
            'filteredProfessions',
            'filteredStatuses',
            'filteredStores',
            'filteredTypes',
        ]

        return visibleOnes.indexOf(propertyName) !== -1
    }

    const isAnyFilterApplied = () => {
        return (
            currentFilter.filteredCities.length > 0 ||
            currentFilter.filteredProfessions.length > 0 ||
            currentFilter.filteredStatuses.length > 0 ||
            currentFilter.filteredStores.length > 0 ||
            currentFilter.filteredTypes.length > 0 ||
            currentFilter.search.length > 0
        )
    }

    const getDataBasedOnFilteringKey = (key: string): any[] | undefined => {
        switch (key) {
            case `filteredCities`:
                return cities
            case `filteredProfessions`:
                return professions
            case `filteredStatuses`:
                return statuses
            case `filteredStores`:
                return stores
            case `filteredTypes`:
                return userTypes
            default:
                return undefined
        }
    }

    const getFilterLabelBasedOnFilteringKey = (key: string): string => {
        switch (key) {
            case `filteredCities`:
                return 'Grad: '
            case `filteredProfessions`:
                return 'Profesija: '
            case `filteredStatuses`:
                return 'Status: '
            case `filteredStores`:
                return 'Radnja: '
            case `filteredTypes`:
                return 'Tip: '
            default:
                return 'undefined'
        }
    }

    const statuses = [
        { id: 1, name: USER_STATUSES.ACTIVE },
        { id: 2, name: USER_STATUSES.INACTIVE },
    ]

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
        onFilterChange(currentFilter)
    }, [currentFilter])

    return (
        <Grid my={2} container spacing={2}>
            <Grid item>
                <Box sx={{ minWidth: 200 }}>
                    <FormControl fullWidth>
                        <InputLabel>{USER_FILTERS.TYPES}</InputLabel>
                        <Select
                            label={USER_FILTERS.TYPES}
                            value={currentFilter.filteredTypes}
                            MenuProps={{
                                disableAutoFocusItem: true,
                            }}
                            multiple
                        >
                            {userTypes &&
                                userTypes.map((type) => {
                                    const isChecked =
                                        currentFilter.filteredTypes.includes(
                                            type.id
                                        )

                                    return (
                                        <KorisniciListCheckBoxFilter
                                            key={type.id}
                                            onClick={() =>
                                                setCurrentFilter({
                                                    ...currentFilter,
                                                    filteredTypes: isChecked
                                                        ? currentFilter.filteredTypes.filter(
                                                              (filterType) =>
                                                                  filterType !==
                                                                  type.id
                                                          )
                                                        : [
                                                              ...currentFilter.filteredTypes,
                                                              type.id,
                                                          ],
                                                })
                                            }
                                            property={type}
                                            isChecked={isChecked}
                                        />
                                    )
                                })}
                        </Select>
                    </FormControl>
                </Box>
            </Grid>
            <Grid item>
                <Box sx={{ minWidth: 200 }}>
                    <FormControl fullWidth>
                        <InputLabel>{USER_FILTERS.STATUSES}</InputLabel>
                        <Select
                            label={USER_FILTERS.STATUSES}
                            value={currentFilter.filteredStatuses}
                            MenuProps={{
                                disableAutoFocusItem: true,
                            }}
                            multiple
                        >
                            {statuses &&
                                statuses.map((status) => {
                                    const isChecked =
                                        currentFilter.filteredStatuses.includes(
                                            status.id
                                        )

                                    return (
                                        <KorisniciListCheckBoxFilter
                                            key={status.id}
                                            onClick={() =>
                                                setCurrentFilter({
                                                    ...currentFilter,
                                                    filteredStatuses: isChecked
                                                        ? currentFilter.filteredStatuses.filter(
                                                              (filterStatus) =>
                                                                  filterStatus !==
                                                                  status.id
                                                          )
                                                        : [
                                                              ...currentFilter.filteredStatuses,
                                                              status.id,
                                                          ],
                                                })
                                            }
                                            property={status}
                                            isChecked={isChecked}
                                        />
                                    )
                                })}
                        </Select>
                    </FormControl>
                </Box>
            </Grid>
            <Grid item>
                <Box sx={{ minWidth: 200 }}>
                    <FormControl fullWidth>
                        <InputLabel>{USER_FILTERS.PROFESSIONS}</InputLabel>
                        <Select
                            label={USER_FILTERS.PROFESSIONS}
                            value={currentFilter.filteredProfessions}
                            MenuProps={{
                                disableAutoFocusItem: true,
                            }}
                            multiple
                        >
                            {professions &&
                                professions.map((profession) => {
                                    const isChecked =
                                        currentFilter.filteredProfessions.includes(
                                            profession.id
                                        )

                                    return (
                                        <KorisniciListCheckBoxFilter
                                            key={profession.id}
                                            onClick={() =>
                                                setCurrentFilter({
                                                    ...currentFilter,
                                                    filteredProfessions:
                                                        isChecked
                                                            ? currentFilter.filteredProfessions.filter(
                                                                  (
                                                                      filterProfession
                                                                  ) =>
                                                                      filterProfession !==
                                                                      profession.id
                                                              )
                                                            : [
                                                                  ...currentFilter.filteredProfessions,
                                                                  profession.id,
                                                              ],
                                                })
                                            }
                                            property={profession}
                                            isChecked={isChecked}
                                        />
                                    )
                                })}
                        </Select>
                    </FormControl>
                </Box>
            </Grid>
            <Grid item>
                <Box sx={{ minWidth: 200 }}>
                    <FormControl fullWidth>
                        <InputLabel>{USER_FILTERS.STORES}</InputLabel>
                        <Select
                            label={USER_FILTERS.STORES}
                            value={currentFilter.filteredStores}
                            MenuProps={{
                                disableAutoFocusItem: true,
                            }}
                            multiple
                        >
                            {stores &&
                                stores.map((store) => {
                                    const isChecked =
                                        currentFilter.filteredStores.includes(
                                            store.id
                                        )

                                    return (
                                        <KorisniciListCheckBoxFilter
                                            key={store.id}
                                            onClick={() =>
                                                setCurrentFilter(
                                                    (prevState) => ({
                                                        ...prevState,
                                                        filteredStores:
                                                            isChecked
                                                                ? prevState.filteredStores.filter(
                                                                      (
                                                                          filterStore
                                                                      ) =>
                                                                          filterStore !==
                                                                          store.id
                                                                  )
                                                                : [
                                                                      ...prevState.filteredStores,
                                                                      store.id,
                                                                  ],
                                                    })
                                                )
                                            }
                                            property={store}
                                            isChecked={isChecked}
                                        />
                                    )
                                })}
                        </Select>
                    </FormControl>
                </Box>
            </Grid>
            <Grid item>
                <Box sx={{ minWidth: 200 }}>
                    <FormControl fullWidth>
                        <InputLabel>{USER_FILTERS.CITIES}</InputLabel>
                        <Select
                            label={USER_FILTERS.CITIES}
                            value={currentFilter.filteredCities}
                            MenuProps={{
                                disableAutoFocusItem: true,
                            }}
                            multiple
                        >
                            {cities &&
                                cities.map((city) => {
                                    const isChecked =
                                        currentFilter.filteredCities.includes(
                                            city.id
                                        )

                                    return (
                                        <KorisniciListCheckBoxFilter
                                            key={city.id}
                                            onClick={() =>
                                                setCurrentFilter({
                                                    ...currentFilter,
                                                    filteredCities: isChecked
                                                        ? currentFilter.filteredCities.filter(
                                                              (filterCity) =>
                                                                  filterCity !==
                                                                  city.id
                                                          )
                                                        : [
                                                              ...currentFilter.filteredCities,
                                                              city.id,
                                                          ],
                                                })
                                            }
                                            property={city}
                                            isChecked={isChecked}
                                        />
                                    )
                                })}
                        </Select>
                    </FormControl>
                </Box>
            </Grid>
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
            {isAnyFilterApplied() && (
                <Grid item xs={12}>
                    <Box>Trenutni prikaz je filtriran:</Box>
                    <Stack direction={`row`} gap={2} margin={1}>
                        {Object.keys(currentFilter)
                            .filter((k: string) => currentFilter[k].chipVisible)
                            .map((key: string) => {
                                return currentFilter[key].map(
                                    (id: number, ik: any) => {
                                        const instance =
                                            getDataBasedOnFilteringKey(
                                                key
                                            )?.find((c) => c.id === id)
                                        return (
                                            <Chip
                                                key={ik}
                                                label={`${getFilterLabelBasedOnFilteringKey(key)}${instance?.name}`}
                                                deleteIcon={<Close />}
                                                onDelete={() => {
                                                    setCurrentFilter({
                                                        ...currentFilter,
                                                        [key]: currentFilter[
                                                            key
                                                        ].filter(
                                                            (c: any) => c !== id
                                                        ),
                                                    })
                                                }}
                                            />
                                        )
                                    }
                                )
                            })}
                    </Stack>
                </Grid>
            )}
        </Grid>
    )
}
