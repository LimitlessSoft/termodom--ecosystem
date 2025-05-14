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
                        onChange={field.onChange}
                        disabled={disabled}
                        value={field?.value || null}
                        slotProps={{
                            textField: {
                                helperText: error?.message || '',
                                error: !!error,
                                label: `${label} ${required ? '*' : ''}`,
                            },
                        }}
                        {...rest}
                    />
                )
            }}
        />
    )
}

export default FormValidationDatePicker
