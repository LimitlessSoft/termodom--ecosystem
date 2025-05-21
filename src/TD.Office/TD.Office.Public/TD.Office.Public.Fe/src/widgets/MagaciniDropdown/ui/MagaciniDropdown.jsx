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

    const [selected, setSelected] = useMountedState({
        initialValue: props.multiselect
            ? Array.isArray(props.defaultValue)
                ? props.defaultValue
                : []
            : props.defaultValue,
        onChange: props.onChange,
    })
    const [allWarehousesSelected, setAllWarehousesSelected] = useMountedState({
        initialValue: props.defaultValue === '',
        onChange: (e) => {
            const checked = e === true

            const isSingleSelectEmpty = !props.multiselect && !selected
            if (isSingleSelectEmpty && !checked) {
                setSelected(magaciniSortedAndFiltered[0].id)
                return
            }

            props.onChange(checked ? null : selected)
        },
    })

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

        setSelected(magaciniSortedAndFiltered[0].id)
    }, [magaciniSortedAndFiltered])

    if (!magaciniSortedAndFiltered) return <LinearProgress />

    if (props.multiselect) {
        return (
            <Stack direction={`row`} gap={2}>
                <ComboBoxInput
                    disabled={props.disabled || allWarehousesSelected}
                    label={'Magacini'}
                    options={magaciniSortedAndFiltered.map((magacin) => ({
                        key: magacin.id,
                        value: magacin.name,
                    }))}
                    onSelectionChange={(e) => setSelected(e.target.value)}
                    selectedValues={selected}
                    style={{
                        width: props.width ?? 500,
                    }}
                />
                {props.allowSviMagaciniFilter && (
                    <MagaciniDropdownSviFilter
                        disabled={props.disabled}
                        value={allWarehousesSelected}
                        setValue={setAllWarehousesSelected}
                    />
                )}
            </Stack>
        )
    } else {
        if (selected == null && allWarehousesSelected !== true)
            return <LinearProgress />

        return (
            <Stack direction={`row`} gap={2}>
                <Autocomplete
                    sx={{
                        width: props.width ?? 500,
                    }}
                    renderInput={(params) => {
                        return <TextField {...params} label={'Magacin'} />
                    }}
                    disabled={props.disabled || allWarehousesSelected}
                    options={magaciniSortedAndFiltered}
                    value={
                        allWarehousesSelected
                            ? magaciniSortedAndFiltered[0]
                            : magaciniSortedAndFiltered.find(
                                  (magacin) => magacin.id == selected
                              )
                    }
                    getOptionLabel={(option) => {
                        return allWarehousesSelected
                            ? `< Svi magacini >`
                            : `${option.name}`
                    }}
                    onChange={(_, value) => setSelected(value.id)}
                />
                {props.allowSviMagaciniFilter && (
                    <MagaciniDropdownSviFilter
                        disabled={props.disabled}
                        value={allWarehousesSelected}
                        setValue={setAllWarehousesSelected}
                    />
                )}
            </Stack>
        )
    }
}
