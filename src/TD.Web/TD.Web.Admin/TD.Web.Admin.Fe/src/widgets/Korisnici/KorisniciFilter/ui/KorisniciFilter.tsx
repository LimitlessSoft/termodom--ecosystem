import { IKorisniciFilterProps } from '../interfaces/IKorisniciFilterProps'
import { IKorisniciFilterData } from '../interfaces/IKorisniciFilterData'
import {
    Box,
    Chip,
    FormControl,
    Grid,
    InputLabel,
    Select,
    Stack,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { KorisniciListCheckBoxFilter } from './KorisniciListCheckBoxFilter'
import { KorisniciFilterSearch } from './KorisniciFilterSearch'
import { Close } from '@mui/icons-material'
import { FILTER_CONFIG, FILTER_KEYS, USER_STATUSES } from '../constants'

export const KorisniciFilter = ({
    onFilterChange,
}: IKorisniciFilterProps): JSX.Element => {
    const [data, setData] = useState<any>({
        userTypes: [],
        professions: [],
        stores: [],
        cities: [],
        statuses: [
            { id: 1, name: USER_STATUSES.ACTIVE },
            { id: 2, name: USER_STATUSES.INACTIVE },
        ],
    })

    const [currentFilter, setCurrentFilter] = useState<IKorisniciFilterData>({
        [FILTER_KEYS.CITIES]: [],
        [FILTER_KEYS.PROFESSIONS]: [],
        [FILTER_KEYS.STATUSES]: [],
        [FILTER_KEYS.STORES]: [],
        [FILTER_KEYS.TYPES]: [],
        [FILTER_KEYS.SEARCH]: '',
    })

    useEffect(() => {
        Promise.all([
            adminApi.get(`/user-types`),
            adminApi.get(`/professions?sortColumn=Name`),
            adminApi.get(`/stores?sortColumn=Name`),
            adminApi.get(`/cities?sortColumn=Name`),
        ])
            .then(
                ([
                    userTypesResponse,
                    professionsResponse,
                    storesResponse,
                    citiesResponse,
                ]) => {
                    setData((prevData: any) => ({
                        ...prevData,
                        userTypes: userTypesResponse.data,
                        professions: professionsResponse.data,
                        stores: storesResponse.data,
                        cities: citiesResponse.data,
                    }))
                }
            )
            .catch((err) => handleApiError(err))
    }, [])

    useEffect(() => {
        onFilterChange(currentFilter)
    }, [currentFilter, onFilterChange])

    const isAnyFilterApplied = FILTER_CONFIG.some(
        (config) => currentFilter[config.key].length > 0
    )

    const handleFilterChange = (
        key: keyof IKorisniciFilterData,
        value: number | string
    ) => {
        setCurrentFilter((prev) => {
            if (key === FILTER_KEYS.SEARCH) {
                return {
                    ...prev,
                    [key]: value.toString(),
                }
            } else {
                const currentValues = prev[key]
                const numericValue =
                    typeof value === 'string' ? parseFloat(value) : value
                const updatedValues = currentValues.includes(numericValue)
                    ? currentValues.filter((id) => id !== numericValue)
                    : [...currentValues, numericValue]

                return {
                    ...prev,
                    [key]: updatedValues,
                }
            }
        })
    }

    return (
        <Grid my={2} container spacing={2}>
            {FILTER_CONFIG.map(({ label, key, dataKey }) => (
                <Grid item key={key}>
                    <Box sx={{ minWidth: 200 }}>
                        <FormControl fullWidth>
                            <InputLabel>{label}</InputLabel>
                            <Select
                                label={label}
                                value={currentFilter[key]}
                                multiple
                                renderValue={(selected) =>
                                    `(${selected.length}) polja izabrana`
                                }
                            >
                                {data[dataKey].map((item: any) => {
                                    const isChecked = currentFilter[
                                        key
                                    ].includes(item.id)
                                    return (
                                        <KorisniciListCheckBoxFilter
                                            key={item.id}
                                            onClick={() =>
                                                handleFilterChange(key, item.id)
                                            }
                                            property={item}
                                            isChecked={isChecked}
                                        />
                                    )
                                })}
                            </Select>
                        </FormControl>
                    </Box>
                </Grid>
            ))}
            <Grid item sm={12}>
                <KorisniciFilterSearch
                    onSearchUsers={(e) =>
                        setCurrentFilter({
                            ...currentFilter,
                            [FILTER_KEYS.SEARCH]: e,
                        })
                    }
                />
            </Grid>
            {isAnyFilterApplied && (
                <Grid item xs={12}>
                    <Box>Trenutni prikaz je filtriran:</Box>
                    <Stack direction={`row`} gap={2} margin={1}>
                        {FILTER_CONFIG.map(({ key, label, dataKey }) => {
                            return currentFilter[key].map((id: any) => {
                                const instance = data[dataKey].find(
                                    (item: any) => item.id === id
                                )
                                return (
                                    <Chip
                                        key={id}
                                        label={`${label}: ${instance?.name}`}
                                        deleteIcon={<Close />}
                                        onDelete={() => {
                                            handleFilterChange(key, id)
                                        }}
                                    />
                                )
                            })
                        })}
                    </Stack>
                </Grid>
            )}
        </Grid>
    )
}
