import { Controller, useFormContext } from 'react-hook-form'
import { AUTOCOMPLETE_NO_OPTIONS_MESSAGE } from '@/app/constants'
import { Autocomplete, TextField } from '@mui/material'
import { getFieldLabel } from '@/utils/formUtils'

const FormValidationAutocomplete = ({
    data,
    options = [],
    label,
    required = false,
    helperText,
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
            render={({ field }) => {
                return (
                    <Autocomplete
                        {...rest}
                        options={options.sort((a, b) =>
                            a.name.localeCompare(b.name)
                        )}
                        getOptionLabel={(option) => option.name}
                        isOptionEqualToValue={(option, value) =>
                            option.id === value?.id
                        }
                        onChange={(_, value) =>
                            field.onChange(value?.id ?? null)
                        }
                        value={
                            options.find(
                                (option) => option.id === field.value
                            ) || null
                        }
                        noOptionsText={AUTOCOMPLETE_NO_OPTIONS_MESSAGE}
                        disabled={disabled}
                        renderInput={(params) => (
                            <TextField
                                {...params}
                                label={displayLabel}
                                error={!!error}
                                helperText={error?.message || helperText || ''}
                                disabled={disabled}
                            />
                        )}
                    />
                )
            }}
        />
    )
}

export default FormValidationAutocomplete
