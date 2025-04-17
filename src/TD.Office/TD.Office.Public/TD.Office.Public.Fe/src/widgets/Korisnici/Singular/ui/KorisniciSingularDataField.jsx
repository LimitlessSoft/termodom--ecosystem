import { Button, CircularProgress, Grid, TextField } from '@mui/material'
import { KorisniciSingularDataFieldStyled } from '../styled/KorisniciSingularDataFieldStyled'
import { Save } from '@mui/icons-material'
import { useState } from 'react'
import { isInvalidIntegerKey, isValidInteger } from '@/helpers/numberHelpers'

export const KorisniciSingularDataField = (props) => {
    const [isUpdating, setIsUpdating] = useState(false)
    const [value, setValue] = useState(props.defaultValue)

    const isIntegerValidation = props.validation === 'integer'

    const handleInputChange = (e) => {
        const newValue = e.target.value

        if (!isIntegerValidation || isValidInteger(newValue)) {
            setValue(newValue)
        }
    }

    const handleInputKeyDown = (e) => {
        if (isIntegerValidation && isInvalidIntegerKey(e)) {
            e.preventDefault()
        }
    }

    return (
        <KorisniciSingularDataFieldStyled editable={props.editable}>
            <Grid container direction={`row`} alignItems={`center`} spacing={1}>
                {props.preLabel && <Grid item>{props.preLabel}</Grid>}
                <Grid item>
                    <TextField
                        aria-readonly
                        disabled={!props.editable || isUpdating}
                        size={`small`}
                        type={props.type}
                        value={value}
                        onChange={handleInputChange}
                        onKeyDown={handleInputKeyDown}
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
                                if (!props.onSave) return
                                setIsUpdating(true)

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
