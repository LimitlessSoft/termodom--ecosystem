import { TextField } from '@mui/material'
import React from 'react'
import { Controller } from 'react-hook-form'

const FormValidationInput = ({
    data,
    control,
    trigger,
    errors,
    disabled,
    required,
    ...rest
}) => {
    return (
        <Controller
            name={data.FIELD}
            control={control}
            render={({ field }) => {
                const error = errors[data.FIELD]

                return (
                    <TextField
                        {...field}
                        label={`${data.LABEL} ${required ? '*' : ''}`}
                        error={!!error}
                        helperText={error?.message || ''}
                        disabled={disabled}
                        onChange={field.onChange}
                        {...rest}
                    />
                )
            }}
        />
    )
}

export default FormValidationInput
