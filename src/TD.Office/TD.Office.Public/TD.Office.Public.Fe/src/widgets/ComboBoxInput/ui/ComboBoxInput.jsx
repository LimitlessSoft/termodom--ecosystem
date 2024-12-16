import {
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Checkbox,
} from '@mui/material'

export const ComboBoxInput = ({
    label,
    onSelectionChange,
    selectedValues,
    options,
    style,
    disabled,
}) => (
    <FormControl disabled={disabled}>
        <InputLabel>{label}</InputLabel>
        <Select
            multiple
            label={label}
            variant="outlined"
            renderValue={(selected) => {
                const selectedValuesAsString = selected.join(', ')
                return selectedValuesAsString.length > 50
                    ? `(${selected.length}) selektovanih`
                    : selectedValuesAsString
            }}
            onChange={onSelectionChange}
            value={selectedValues ?? []}
            sx={style}
        >
            {options.map((option) => (
                <MenuItem key={option.value} value={option.key}>
                    <Checkbox checked={selectedValues?.includes(option.key)} />
                    {option.key}
                </MenuItem>
            ))}
        </Select>
    </FormControl>
)
