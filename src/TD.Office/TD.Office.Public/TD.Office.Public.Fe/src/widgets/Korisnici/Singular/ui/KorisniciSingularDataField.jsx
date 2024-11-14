import { Button, CircularProgress, Grid, TextField } from '@mui/material'
import { KorisniciSingularDataFieldStyled } from '../styled/KorisniciSingularDataFieldStyled'
import { Save } from '@mui/icons-material'
import { useState } from 'react'

export const KorisniciSingularDataField = (props) => {
    const [isUpdating, setIsUpdating] = useState(false)
    const [value, setValue] = useState(props.defaultValue)

    return (
        <KorisniciSingularDataFieldStyled editable={props.editable}>
            <Grid container direction={`row`} alignItems={`center`} spacing={1}>
                {props.preLabel === undefined ? null : (
                    <Grid item>{props.preLabel}</Grid>
                )}
                <Grid item>
                    <TextField
                        aria-readonly
                        disabled={!props.editable || isUpdating}
                        size={`small`}
                        value={value}
                        onChange={(e) => {
                            setValue(e.target.value)
                        }}
                    />
                </Grid>
                {props.editable && props.defaultValue != value && (
                    <Grid item>
                        <Button
                            disabled={isUpdating}
                            startIcon={
                                isUpdating ? (
                                    <CircularProgress size={`1em`} />
                                ) : (
                                    <Save />
                                )
                            }
                            variant={`contained`}
                            onClick={() => {
                                setIsUpdating(true)
                                if (!props.onSave) return

                                props
                                    .onSave(value)
                                    .catch(() => setValue(props.defaultValue))
                                    .finally(() => setIsUpdating(false))
                            }}
                        >
                            Sačuvaj
                        </Button>
                    </Grid>
                )}
            </Grid>
        </KorisniciSingularDataFieldStyled>
    )
}
