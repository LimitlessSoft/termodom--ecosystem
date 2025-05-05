import { TextField } from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import React from 'react'
import { Controller } from 'react-hook-form'

const FormValidationDatePicker = ({
    data,
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
                    <DatePicker
                        {...field}
                        onChange={(date) => {
                            field.onChange(date)
                            trigger(data.FIELD)
                        }}
                        disabled={disabled}
                        value={field?.value || null}
                        slots={{
                            textField: (params) => (
                                <TextField
                                    {...params}
                                    label={`${label} ${required ? '*' : ''}`}
                                    error={!!error}
                                    helperText={error?.message || ''}
                                />
                            ),
                        }}
                        {...rest}
                    />
                )
            }}
        />
    )
}

export default FormValidationDatePicker
