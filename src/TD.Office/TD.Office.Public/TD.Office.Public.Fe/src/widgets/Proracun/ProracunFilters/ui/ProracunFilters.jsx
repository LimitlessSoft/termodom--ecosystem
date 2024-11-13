import {
    Autocomplete,
    Box,
    CircularProgress,
    Stack,
    TextField,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import dayjs from 'dayjs'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { DATE_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'

export const ProracunFilters = ({
    onFilterChange,
    filters,
    magacini,
    disabled,
    defaultMagacin,
}) => {
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PRORACUNI
    )

    return (
        <Box>
            {magacini && magacini.length > 0 ? (
                <Autocomplete
                    disabled={disabled}
                    sx={{
                        mx: 2,
                        maxWidth: 500,
                    }}
                    value={magacini.find(
                        (magacin) => magacin.id === defaultMagacin
                    )}
                    options={magacini}
                    onChange={(event, value) => {
                        onFilterChange({
                            ...filters,
                            magacinId: value.id,
                        })
                    }}
                    getOptionLabel={(option) => {
                        return `${option.name}`
                    }}
                    renderInput={(params) => <TextField {...params} />}
                />
            ) : (
                <CircularProgress />
            )}

            <Stack direction={`row`} m={2} gap={2}>
                <DatePicker
                    disabled={disabled}
                    disableFuture
                    label={`Od datuma`}
                    value={dayjs(filters.fromUtc)}
                    format={DATE_CONSTANTS.FORMAT}
                    minDate={
                        hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .OLDER_THAN_SEVEN_DAYS
                        )
                            ? null
                            : dayjs().subtract(7, 'days')
                    }
                    onChange={(e) =>
                        onFilterChange({
                            ...filters,
                            fromUtc: e || new Date(),
                        })
                    }
                />
                <DatePicker
                    disabled={disabled}
                    disableFuture
                    label={'Do datuma'}
                    value={dayjs(filters.toUtc)}
                    format={DATE_CONSTANTS.FORMAT}
                    minDate={
                        hasPermission(
                            permissions,
                            PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                                .OLDER_THAN_SEVEN_DAYS
                        )
                            ? null
                            : dayjs().subtract(7, 'days')
                    }
                    onChange={(e) =>
                        onFilterChange({
                            ...filters,
                            toUtc: e || new Date(),
                        })
                    }
                />
            </Stack>
        </Box>
    )
}
