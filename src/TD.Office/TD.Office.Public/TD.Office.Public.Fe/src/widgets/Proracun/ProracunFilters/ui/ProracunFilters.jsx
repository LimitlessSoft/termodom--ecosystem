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
import { hasPermission } from '../../../../helpers/permissionsHelpers'
import { usePermissions } from '../../../../hooks/usePermissionsHook'
import { PERMISSIONS_CONSTANTS } from '../../../../constants'

export const ProracunFilters = ({
    onChange,
    magacini,
    disabled,
    defaultMagacin,
}) => {
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PRORACUNI
    )

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
                    minDate={
                        hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .OLDER_THAN_SEVEN_DAYS
                        )
                            ? null
                            : dayjs().subtract(7, 'days')
                    }
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
                    minDate={
                        hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .OLDER_THAN_SEVEN_DAYS
                        )
                            ? null
                            : dayjs().subtract(7, 'days')
                    }
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
