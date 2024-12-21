import { Autocomplete, LinearProgress, TextField } from '@mui/material'
import { useVrDoks } from '../../../zStore'
import { useEffect, useState } from 'react'
import { ComboBoxInput } from '../../ComboBoxInput/ui/ComboBoxInput'

export const VrDoksDropdown = ({ onChange, multiselect, width, disabled }) => {
    const vrDoks = useVrDoks()

    const [multiselectSelectedValues, setMultiselectMultiselectSelectedValues] =
        useState([])

    useEffect(() => {
        if (!multiselect) return

        onChange(multiselectSelectedValues)
    }, [multiselectSelectedValues])

    if (!vrDoks) return <LinearProgress />

    if (multiselect) {
        return (
            <ComboBoxInput
                disabled={disabled}
                label={'Vrste dokumenata'}
                options={vrDoks
                    .toSorted((a, b) => a.vrDok - b.vrDok)
                    .map((v) => ({
                        key: v.vrDok,
                        value: `${v.vrDok} - ${v.nazivDok}`,
                    }))}
                onSelectionChange={(e) => {
                    setMultiselectMultiselectSelectedValues(e.target.value)
                }}
                selectedValues={multiselectSelectedValues}
                style={{
                    width: width ?? 500,
                }}
            />
        )
    } else {
        return (
            <Autocomplete
                sx={{
                    width: 400,
                }}
                getOptionLabel={(option) =>
                    `[${option.vrDok}] ${option.nazivDok}`
                }
                renderInput={(params) => {
                    return <TextField {...params} label={'Vrsta dokumenta'} />
                }}
                disabled={props.disabled}
                options={vrDoks}
                defaultValue={vrDoks[0]}
                onChange={(e, value) => {
                    onChange(value)
                }}
            />
        )
    }
}
