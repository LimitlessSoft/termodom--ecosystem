import {
    Autocomplete,
    LinearProgress,
    Paper,
    Stack,
    Switch,
    TextField,
    Typography,
} from '@mui/material'
import { useZMagacini } from '../../../zStore'
import { ComboBoxInput } from '../../ComboBoxInput/ui/ComboBoxInput'
import { useEffect, useRef, useState } from 'react'
import { useSviMagaciniState } from '../hooks/useSviMagaciniState'
import { MagaciniDropdownSviFilter } from './MagaciniDropdownSviFilter'
import { useSingleSelectState } from '../hooks/useSingeSelectState'

// types = [] - filter magacini by type
// 1 = VP
// 2 = MP
export const MagaciniDropdown = (props) => {
    const magacini = useZMagacini()
    const [singleSelect, setSingleSelect] = useSingleSelectState(props.onChange)
    const [magaciniSortedAndFiltered, setMagaciniSortedAndFiltered] =
        useState(undefined)
    const [sviMagaciniFilter, setSviMagaciniFilter] = useSviMagaciniState(
        (e) => {
            props.onChange(
                e === true
                    ? null
                    : props.multiselect
                      ? multiselectSelectedValues
                      : singleSelect
            )
        }
    )

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
        setSingleSelect(magaciniSortedAndFiltered[0].id)
    }, [magaciniSortedAndFiltered])

    if (!magaciniSortedAndFiltered) return <LinearProgress />

    if (props.multiselect) {
        return (
            <Stack direction={`row`} gap={2}>
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
                {props.allowSviMagaciniFilter && (
                    <MagaciniDropdownSviFilter
                        value={sviMagaciniFilter}
                        setValue={setSviMagaciniFilter}
                        disabled={props.disabled}
                    />
                )}
            </Stack>
        )
    } else {
        return (
            <Stack direction={`row`} gap={2}>
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
                        setSingleSelect(value.id)
                    }}
                />
                {props.allowSviMagaciniFilter && (
                    <MagaciniDropdownSviFilter
                        value={sviMagaciniFilter}
                        setValue={setSviMagaciniFilter}
                        disabled={props.disabled}
                    />
                )}
            </Stack>
        )
    }
}
