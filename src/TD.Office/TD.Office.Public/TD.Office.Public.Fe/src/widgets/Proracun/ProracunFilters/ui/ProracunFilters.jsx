import {
    Autocomplete,
    Box,
    CircularProgress,
    Paper,
    Stack,
    Switch,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { DatePicker } from '@mui/x-date-pickers'
import dayjs from 'dayjs'
import { hasPermission } from '../../../../helpers/permissionsHelpers'
import { usePermissions } from '../../../../hooks/usePermissionsHook'
import { PERMISSIONS_CONSTANTS } from '../../../../constants'
import { useQuery } from '@/hooks'

export const ProracunFilters = ({
    onChange,
    magacini,
    disabled,
    defaultMagacin,
}) => {
    const [sviMagacini, setSviMagacini] = useState(false)

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PRORACUNI
    )

    const [lastMagacin, setLastMagacin] = useState(defaultMagacin)

    const [filters, setFilters] = useQuery(
        {
            fromLocal: dayjs(new Date()).startOf('day').toDate(),
            toLocal: dayjs(new Date()).endOf('day').toDate(),
            magacinId: defaultMagacin,
        },
        (query) => {
            setSviMagacini(query.magacinId == null)
            setLastMagacin(+(query.magacinId || defaultMagacin))
        }
    )

    useEffect(() => {
        setFilters((prev) => {
            return {
                ...prev,
                magacinId: sviMagacini === true ? null : lastMagacin,
            }
        })
    }, [sviMagacini])

    useEffect(() => {
        onChange?.(filters)
    }, [filters])

    useEffect(() => {
        setFilters((prev) => ({
            ...prev,
            magacinId: lastMagacin,
        }))
    }, [lastMagacin])

    return (
        <Box>
            {magacini === undefined ? (
                <CircularProgress />
            ) : (
                <Stack direction={`row`} alignItems={`center`}>
                    <Autocomplete
                        disabled={disabled || sviMagacini}
                        sx={{
                            mx: 2,
                            width: 500,
                        }}
                        value={magacini.find(
                            (x) => x.id === (lastMagacin || defaultMagacin)
                        )}
                        options={magacini}
                        disableClearable={true}
                        onChange={(_, value) => {
                            setLastMagacin(value.id)
                        }}
                        getOptionLabel={(option) => {
                            return sviMagacini
                                ? `< Svi magacini >`
                                : `${option.name}`
                        }}
                        renderInput={(params) => <TextField {...params} />}
                    />
                    {hasPermission(
                        permissions,
                        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PRORACUNI
                            .RAD_SA_SVIM_MAGACINIMA
                    ) && (
                        <Paper
                            sx={{
                                backgroundColor: disabled
                                    ? `grey.100`
                                    : `white`,
                            }}
                        >
                            <Stack
                                m={1}
                                direction={`row`}
                                alignItems={`center`}
                            >
                                <Typography>Svi magacini</Typography>
                                <Switch
                                    disabled={disabled}
                                    checked={sviMagacini}
                                    onChange={(e) => {
                                        setSviMagacini(e.target.checked)
                                    }}
                                />
                            </Stack>
                        </Paper>
                    )}
                </Stack>
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
