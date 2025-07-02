import { DatePicker } from '@mui/x-date-pickers'
import { Controller, useFormContext } from 'react-hook-form'
import { getFieldLabel } from '@/utils/formUtils'

const FormValidationDatePicker = ({
    data,
    label,
    disabled = false,
    required = false,
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
                <DatePicker
                    {...field}
                    {...rest}
                    disabled={disabled}
                    onChange={field.onChange}
                    value={field.value ?? null}
                    slotProps={{
                        textField: {
                            label: displayLabel,
                            helperText: error?.message || '',
                            error: !!error,
                            disabled,
                        },
                    }}
                />
            )}
        />
    )
}

export default FormValidationDatePicker
