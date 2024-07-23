import {
    Button,
    CircularProgress,
    Grid,
    LinearProgress,
    Stack,
    TextField,
    ToggleButton,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { getStatuses } from '@/helpers/productHelpers'

export const ProizvodiProductsFilter = (props: any): JSX.Element => {
    const [text, setText] = useState<string>('')
    const [statuses, setStatuses] = useState<number[]>([
        ...Object.values(getStatuses()),
        -1,
    ])

    useEffect(() => {
        if (statuses.includes(-1)) {
            setStatuses(Object.values(getStatuses()))
            return
        }
        props.onPretrazi(text, statuses)
    }, [statuses])

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
                    {Object.keys(getStatuses()).map((key: string) => {
                        let val: number = getStatuses()[key]
                        return (
                            <ToggleButton
                                disabled={props.isFetching}
                                key={key}
                                value={val}
                                selected={statuses?.includes(val)}
                                onClick={() => {
                                    if (statuses == null) {
                                        setStatuses([val])
                                    } else {
                                        if (statuses.includes(val)) {
                                            if (statuses.length === 1) {
                                                setStatuses(
                                                    Object.values(getStatuses())
                                                )
                                                return
                                            }
                                            setStatuses(
                                                statuses.filter(
                                                    (s) => s !== val
                                                )
                                            )
                                        } else {
                                            setStatuses([...statuses, val])
                                        }
                                    }
                                }}
                            >
                                {key.split(/(?=[A-Z])/).join(' ')}
                            </ToggleButton>
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
