import {
    Autocomplete,
    Box,
    CircularProgress,
    Stack,
    TextField,
} from '@mui/material'
import { useZMagacini } from '../../../../zStore'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { DatePicker } from '@mui/x-date-pickers'
import dayjs from 'dayjs'
import { useUser } from '../../../../hooks/useUserHook'

export const ProracunFilters = ({
    onChange,
    magacini,
    disabled,
    defaultMagacin,
}) => {
    const [filters, setFilters] = useState({
        fromLocal: dayjs(new Date()).startOf('day').toDate(),
        toLocal: dayjs(new Date()).endOf('day').toDate(),
        magacinId: defaultMagacin,
    })

    useEffect(() => {
        if (onChange === undefined) return

        onChange(filters)
    }, [filters])

    return (
        <Box>
            {magacini === undefined ? (
                <CircularProgress />
            ) : (
                <Autocomplete
                    disabled={disabled}
                    sx={{
                        mx: 2,
                        maxWidth: 500,
                    }}
                    defaultValue={magacini.find((x) => x.id === defaultMagacin)}
                    options={magacini}
                    onChange={(event, value) => {
                        setFilters((prev) => {
                            return {
                                ...prev,
                                magacinId: value.id,
                            }
                        })
                    }}
                    getOptionLabel={(option) => {
                        return `${option.name}`
                    }}
                    renderInput={(params) => <TextField {...params} />}
                />
            )}
            <Stack direction={`row`} m={2} gap={2}>
                <DatePicker
                    disabled={disabled}
                    label={`Od datuma`}
                    value={dayjs(filters.fromLocal)}
                    onChange={(e) => {
                        setFilters((prev) => {
                            return {
                                ...prev,
                                fromLocal: dayjs(e ?? new Date())
                                    .startOf('day')
                                    .toDate(),
                            }
                        })
                    }}
                />
                <DatePicker
                    disabled={disabled}
                    label={'Do datuma'}
                    value={dayjs(filters.toLocal)}
                    onChange={(e) => {
                        setFilters((prev) => {
                            return {
                                ...prev,
                                toLocal: dayjs(e ?? new Date())
                                    .endOf('day')
                                    .toDate(),
                            }
                        })
                    }}
                />
            </Stack>
        </Box>
    )
}
