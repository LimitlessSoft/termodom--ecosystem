import { Controller, useFormContext } from 'react-hook-form'
import {
    FormControl,
    FormHelperText,
    InputLabel,
    MenuItem,
    Select,
} from '@mui/material'
import { getFieldLabel } from '@/utils/formUtils'

const FormValidationSelect = ({
    data,
    options = [],
    helperText,
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
                <FormControl error={!!error} disabled={disabled}>
                    <InputLabel>{displayLabel}</InputLabel>
                    <Select
                        {...field}
                        {...rest}
                        label={displayLabel}
                        value={field.value ?? ''}
                        onChange={(event) =>
                            field.onChange(+event.target.value)
                        }
                    >
                        {options.map((option) => (
                            <MenuItem key={option.id} value={option.id}>
                                {option.name}
                            </MenuItem>
                        ))}
                    </Select>
                    <FormHelperText>
                        {error?.message || helperText || ''}
                    </FormHelperText>
                </FormControl>
            )}
        />
    )
}

export default FormValidationSelect
