import {
    Badge,
    Button,
    CircularProgress,
    Grid,
    LinearProgress,
    Stack,
    TextField,
    ToggleButton,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { getStatuses } from '@/helpers/productHelpers'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { PERMISSIONS_GROUPS, USER_PERMISSIONS } from '@/constants'
import { hasPermission } from '@/helpers/permissionsHelpers'

export const ProizvodiProductsFilter = (props: any): JSX.Element => {
    const permissions = usePermissions(PERMISSIONS_GROUPS.PRODUCTS)
    const [text, setText] = useState<string>('')
    const [statuses, setStatuses] = useState<number[]>([-1])
    const [availableStatuses, setAvailableStatuses] = useState<number[]>([])

    useEffect(() => {
        if (permissions === undefined) return

        if (statuses.includes(-1)) {
            let statuses = Object.values(getStatuses())

            if (
                hasPermission(permissions, USER_PERMISSIONS.PROIZVODI.EDIT_ALL)
            ) {
                setStatuses(Object.values(getStatuses()))
                setAvailableStatuses(Object.values(getStatuses()))
            } else {
                setStatuses(statuses.filter((s) => s !== 0 && s !== 5))
                setAvailableStatuses(statuses.filter((s) => s !== 0 && s !== 5))
            }
            return
        }
        props.onPretrazi(text, statuses)
    }, [statuses, permissions])

    return (
        <Grid container alignItems={`center`} p={2} gap={2}>
            <Grid item xs={12}>
                <TextField
                    disabled={props.isFetching}
                    sx={{
                        minWidth: 400,
                    }}
                    onChange={(e) => {
                        setText(e.target.value)
                    }}
                    onKeyDown={(e) => {
                        if (e.key === 'Enter' || e.key === 'Return') {
                            props.onPretrazi(text, statuses)
                        }
                    }}
                    placeholder="Pretraga..."
                />
                <Button
                    variant={`contained`}
                    disabled={props.isFetching}
                    sx={{
                        m: 2,
                    }}
                    onClick={() => {
                        props.onPretrazi(text, statuses)
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
                                        props.currentProducts.filter(
                                            (z: any) => z.status == value
                                        ).length
                                    }
                                    color={`warning`}
                                    key={key}
                                >
                                    <ToggleButton
                                        disabled={props.isFetching}
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
            <Grid item xs={12}>
                {props.isFetching && <CircularProgress />}
            </Grid>
        </Grid>
    )
}
