import { Autocomplete, LinearProgress, Stack, TextField } from '@mui/material'
import { useZMagacini } from '../../../zStore'
import { ComboBoxInput } from '../../ComboBoxInput/ui/ComboBoxInput'
import { useEffect, useState } from 'react'
import { MagaciniDropdownSviFilter } from './MagaciniDropdownSviFilter'
import { useMountedState } from '../../../hooks'

// types = [] - filter magacini by type
// 1 = VP
// 2 = MP
export const MagaciniDropdown = (props) => {
    const magacini = useZMagacini()

    const [magaciniSortedAndFiltered, setMagaciniSortedAndFiltered] = useState()
    const [singleSelect, setSingleSelect] = useMountedState({
        initialValue: props.defaultValue,
        onChange: props.onChange,
    })
    const [multiselectSelectedValues, setMultiselectMultiselectSelectedValues] =
        useMountedState({
            initialValue: Array.isArray(props.defaultValue)
                ? props.defaultValue
                : [],
            onChange: (values) => {
                if (!props.multiselect) return
                props.onChange(values)
            },
        })
    const [sviMagaciniFilter, setSviMagaciniFilter] = useMountedState({
        initialValue: props.defaultValue === '',
        onChange: (e) => {
            const checked = e === true

            setIsSvimagaciniFilterEnabled(checked)

            const isSingleSelectEmpty = !props.multiselect && !singleSelect
            if (isSingleSelectEmpty && !checked) {
                console.log('Not sure', magaciniSortedAndFiltered[0].id)
                setSingleSelect(magaciniSortedAndFiltered[0].id)
                return
            }

            props.onChange(
                checked
                    ? null
                    : props.multiselect
                      ? multiselectSelectedValues
                      : singleSelect
            )
        },
    })
    const [isSvimagaciniFilterEnabled, setIsSvimagaciniFilterEnabled] =
        useState(props.defaultValue === '')

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
        if (props.multiselect) return
        if (props.defaultValue != null) return

        setSingleSelect(magaciniSortedAndFiltered[0].id)
    }, [magaciniSortedAndFiltered])

    if (!magaciniSortedAndFiltered) return <LinearProgress />

    if (props.multiselect) {
        return (
            <Stack direction={`row`} gap={2}>
                <ComboBoxInput
                    disabled={props.disabled || isSvimagaciniFilterEnabled}
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
        if (singleSelect == null && !isSvimagaciniFilterEnabled)
            return <LinearProgress />
        return (
            <Stack direction={`row`} gap={2}>
                <Autocomplete
                    sx={{
                        width: props.width ?? 500,
                    }}
                    getOptionLabel={(option) => {
                        return isSvimagaciniFilterEnabled
                            ? `< Svi magacini >`
                            : `${option.name}`
                    }}
                    renderInput={(params) => {
                        return <TextField {...params} label={'Magacin'} />
                    }}
                    disabled={props.disabled || isSvimagaciniFilterEnabled}
                    options={magaciniSortedAndFiltered}
                    value={
                        isSvimagaciniFilterEnabled
                            ? magaciniSortedAndFiltered[0]
                            : magaciniSortedAndFiltered.find(
                                  (magacin) => magacin.id == singleSelect
                              )
                    }
                    onChange={(_, value) => {
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
