import { Autocomplete, LinearProgress, TextField } from '@mui/material'
import { useZMagacini } from '../../../zStore'
import { ComboBoxInput } from '../../ComboBoxInput/ui/ComboBoxInput'
import { useEffect, useState } from 'react'

// types = [] - filter magacini by type
// 1 = VP
// 2 = MP
export const MagaciniDropdown = (props) => {
    const magacini = useZMagacini()

    const [magaciniSortedAndFiltered, setMagaciniSortedAndFiltered] =
        useState(undefined)

    const [multiselectSelectedValues, setMultiselectMultiselectSelectedValues] =
        useState([])

    useEffect(() => {
        if (!props.multiselect) return

        props.onChange(multiselectSelectedValues)
    }, [multiselectSelectedValues])

    useEffect(() => {
        if (!magacini) {
            setMagaciniSortedAndFiltered(undefined)
            return
        }

        let m = magacini

        if (props.types !== undefined && props.types.length > 0) {
            m = m.filter((magacin) => props.types.includes(magacin.vrsta))
        }

        if (
            props.excluteContainingStar !== undefined &&
            props.excluteContainingStar === true
        ) {
            m = m.filter((magacin) => !magacin.name.includes('*'))
        }

        m.sort((a, b) => a.id - b.id)

        setMagaciniSortedAndFiltered(m)
    }, [magacini])

    useEffect(() => {
        if (!magaciniSortedAndFiltered) return
        props.onChange(magaciniSortedAndFiltered[0].id)
    }, [magaciniSortedAndFiltered])

    if (!magaciniSortedAndFiltered) return <LinearProgress />

    if (props.multiselect) {
        return (
            <ComboBoxInput
                disabled={props.disabled}
                label={'Magacini'}
                options={magaciniSortedAndFiltered.map((magacin) => ({
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
                options={magaciniSortedAndFiltered}
                defaultValue={magaciniSortedAndFiltered[0]}
                onChange={(e, value) => {
                    props.onChange(value.id)
                }}
            />
        )
    }
}
