import { Autocomplete, LinearProgress, TextField } from '@mui/material'
import { useZMagacini } from '../../../zStore'
import { ComboBoxInput } from '../../ComboBoxInput/ui/ComboBoxInput'
import { useEffect, useState } from 'react'

export const MagaciniDropdown = (props) => {
    const magacini = useZMagacini()

    const [magaciniSorted, setMagaciniSorted] = useState(undefined)

    const [multiselectSelectedValues, setMultiselectMultiselectSelectedValues] =
        useState([])

    useEffect(() => {
        if (!props.multiselect) return

        props.onChange(multiselectSelectedValues)
    }, [multiselectSelectedValues])

    useEffect(() => {
        if (!magacini) {
            setMagaciniSorted(undefined)
            return
        }

        setMagaciniSorted(magacini.sort((a, b) => a.name.localeCompare(b.name)))
    }, [magacini])

    if (!magaciniSorted) return <LinearProgress />

    if (props.multiselect) {
        return (
            <ComboBoxInput
                disabled={props.disabled}
                label={'Magacini'}
                options={magaciniSorted.map((magacin) => ({
                    key: magacin.id,
                    value: magacin.name,
                }))}
                onSelectionChange={(e) => {
                    setMultiselectMultiselectSelectedValues(e.target.value)
                }}
                selectedValues={multiselectSelectedValues}
                style={{
                    width: props.width ?? 500,
                }}
            />
        )
    } else {
        return (
            <Autocomplete
                sx={{
                    width: props.width ?? 500,
                }}
                getOptionLabel={(option) => option.name}
                renderInput={(params) => {
                    return <TextField {...params} label={'Magacin'} />
                }}
                disabled={props.disabled}
                options={magacini}
                defaultValue={magaciniSorted[0]}
                onChange={(e, value) => {
                    props.onChange(value.id)
                }}
            />
        )
    }
}
