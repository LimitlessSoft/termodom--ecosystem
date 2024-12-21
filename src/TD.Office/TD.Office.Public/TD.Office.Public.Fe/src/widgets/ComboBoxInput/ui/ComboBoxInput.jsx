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
                const selectedValuesAsString = options
                    .map((option) =>
                        selected.includes(option.key) ? option.value : ''
                    )
                    .filter((v) => v)
                    .join(', ')
                return selectedValuesAsString.length > 50
                    ? `(${selected.length}) selektovanih`
                    : selectedValuesAsString
            }}
            onChange={onSelectionChange}
            value={selectedValues ?? []}
            sx={style}
        >
            {options.map((option, i) => (
                <MenuItem key={i} value={option.key}>
                    <Checkbox checked={selectedValues?.includes(option.key)} />
                    {option.value}
                </MenuItem>
            ))}
        </Select>
    </FormControl>
)
