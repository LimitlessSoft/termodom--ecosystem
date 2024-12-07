import {
    Badge,
    Button,
    CircularProgress,
    Grid,
    Stack,
    TextField,
    ToggleButton,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { getStatuses } from '@/helpers/productHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { USER_PERMISSIONS } from '@/constants'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { PERMISSIONS_CONSTANTS } from '../../../../constants'

export const ProizvodiProductsFilter = ({
    onPretrazi,
    isFetching,
    currentProducts,
}) => {
    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.PRODUCTS
    )
    const [text, setText] = useState('')
    const [statuses, setStatuses] = useState([-1])
    const [availableStatuses, setAvailableStatuses] = useState([])

    useEffect(() => {
        if (permissions === undefined) return

        if (statuses.includes(-1)) {
            let statuses = Object.values(getStatuses())

            if (
                hasPermission(
                    permissions,
                    PERMISSIONS_CONSTANTS.USER_PERMISSIONS.PROIZVODI.EDIT_ALL
                )
            ) {
                setStatuses(Object.values(getStatuses()))
                setAvailableStatuses(Object.values(getStatuses()))
            } else {
                setStatuses(statuses.filter((s) => s !== 0 && s !== 5))
                setAvailableStatuses(statuses.filter((s) => s !== 0 && s !== 5))
            }
            return
        }
        onPretrazi(text, statuses)
    }, [statuses, permissions])

    return (
        <Grid container alignItems={`center`} gap={2}>
            <Grid item xs={12}>
                <TextField
                    disabled={isFetching}
                    sx={{
                        minWidth: 400,
                    }}
                    onChange={(e) => {
                        setText(e.target.value)
                    }}
                    onKeyDown={(e) => {
                        if (e.key === 'Enter' || e.key === 'Return') {
                            onPretrazi(text, statuses)
                        }
                    }}
                    placeholder="Pretraga..."
                />
                <Button
                    variant={`contained`}
                    disabled={isFetching}
                    sx={{
                        m: 2,
                    }}
                    onClick={() => {
                        onPretrazi(text, statuses)
                    }}
                >
                    Pretrazi
                </Button>
            </Grid>
            <Grid item>
                <Stack direction={`row`} gap={2}>
                    {Object.entries(getStatuses())
                        .filter(([key, value]) => {
                            return availableStatuses.indexOf(value) !== -1
                        })
                        .map((e) => {
                            const key = e[0]
                            const value = e[1]
                            return (
                                <Badge
                                    badgeContent={
                                        currentProducts?.filter(
                                            (z) => z.status == value
                                        ).length
                                    }
                                    color={`warning`}
                                    key={key}
                                >
                                    <ToggleButton
                                        disabled={isFetching}
                                        value={value}
                                        selected={statuses?.includes(value)}
                                        onClick={() => {
                                            if (statuses == null) {
                                                setStatuses([value])
                                            } else {
                                                if (statuses.includes(value)) {
                                                    if (statuses.length === 1) {
                                                        setStatuses(
                                                            Object.values(
                                                                getStatuses()
                                                            )
                                                        )
                                                        return
                                                    }
                                                    setStatuses(
                                                        statuses.filter(
                                                            (s) => s !== value
                                                        )
                                                    )
                                                } else {
                                                    setStatuses([
                                                        ...statuses,
                                                        value,
                                                    ])
                                                }
                                            }
                                        }}
                                    >
                                        <Typography>
                                            {key.split(/(?=[A-Z])/).join(' ')}
                                        </Typography>
                                    </ToggleButton>
                                </Badge>
                            )
                        })}
                </Stack>
            </Grid>
        </Grid>
    )
}
