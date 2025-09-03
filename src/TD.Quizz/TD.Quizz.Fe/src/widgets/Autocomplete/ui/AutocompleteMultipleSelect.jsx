import { CheckBox, CheckBoxOutlineBlank } from '@mui/icons-material'
import {
    Autocomplete,
    Checkbox,
    CircularProgress,
    TextField,
} from '@mui/material'
import React from 'react'

export default function AutocompleteMultipleSelect({
    options,
    onChange,
    disabled,
    selected,
    loading,
    label,
}) {
    return (
        <Autocomplete
            multiple
            disabled={disabled}
            options={options}
            disableCloseOnSelect
            getOptionLabel={(option) => option.name}
            onChange={(_event, value) => onChange(value)}
            value={selected}
            isOptionEqualToValue={(option, value) => option.id === value.id}
            renderOption={(props, option) => {
                const { key, ...optionProps } = props
                const isChecked = selected.some((item) => item.id === option.id)

                return (
                    <li key={key} {...optionProps}>
                        <Checkbox
                            icon={<CheckBoxOutlineBlank />}
                            checkedIcon={<CheckBox />}
                            style={{ marginRight: 8 }}
                            checked={isChecked}
                        />
                        {option.name}
                    </li>
                )
            }}
            sx={{ py: 2, width: 500 }}
            renderInput={(params) => (
                <TextField
                    {...params}
                    label={label}
                    slotProps={{
                        input: {
                            ...params.InputProps,
                            endAdornment: (
                                <React.Fragment>
                                    {loading && (
                                        <CircularProgress
                                            color="inherit"
                                            size={20}
                                        />
                                    )}
                                    {params.InputProps.endAdornment}
                                </React.Fragment>
                            ),
                        },
                    }}
                />
            )}
        />
    )
}
