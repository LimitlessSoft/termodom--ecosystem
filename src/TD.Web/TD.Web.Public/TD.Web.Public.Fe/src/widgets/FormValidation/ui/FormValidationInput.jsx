import { TextField } from '@mui/material'
import { Controller, useFormContext } from 'react-hook-form'
import { getFieldLabel } from '@/utils/formUtils'

const FormValidationInput = ({
    data,
    label,
    required = false,
    disabled = false,
    ...rest
}) => {
    const {
        control,
        formState: { errors },
    } = useFormContext()

    const error = errors?.[data.FIELD]
    const displayLabel = getFieldLabel(label || data?.LABEL || '', required)

    return (
        <Controller
            name={data.FIELD}
            control={control}
            render={({ field }) => (
                <TextField
                    {...field}
                    {...rest}
                    label={displayLabel}
                    error={!!error}
                    helperText={error?.message || ''}
                    disabled={disabled}
                />
            )}
        />
    )
}

export default FormValidationInput
