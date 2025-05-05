import React from 'react'
import { Controller } from 'react-hook-form'
import { AUTOCOMPLETE_NO_OPTIONS_MESSAGE } from '@/app/constants'
import { Autocomplete, TextField } from '@mui/material'

const FormValidationAutocomplete = ({
    data,
    options,
    label,
    control,
    trigger,
    errors,
    required,
    disabled,
    ...rest
}) => {
    return (
        <Controller
            name={data.FIELD}
            control={control}
            render={({ field }) => {
                const error = errors[data.FIELD]

                return (
                    <Autocomplete
                        options={options.sort((a, b) =>
                            a.name.localeCompare(b.name)
                        )}
                        getOptionLabel={(option) => option.name}
                        isOptionEqualToValue={(option, value) =>
                            option.id === value.id
                        }
                        onChange={(_, value) => {
                            field.onChange(value?.id)
                            trigger(data.FIELD)
                        }}
                        value={
                            options.find(
                                (option) => option.id === field.value
                            ) || null
                        }
                        noOptionsText={AUTOCOMPLETE_NO_OPTIONS_MESSAGE}
                        renderInput={(params) => (
                            <TextField
                                {...params}
                                label={`${label} ${required ? '*' : ''}`}
                                error={!!error}
                                helperText={error?.message || ''}
                            />
                        )}
                        disabled={disabled}
                        {...rest}
                    />
                )
            }}
        />
    )
}

export default FormValidationAutocomplete
