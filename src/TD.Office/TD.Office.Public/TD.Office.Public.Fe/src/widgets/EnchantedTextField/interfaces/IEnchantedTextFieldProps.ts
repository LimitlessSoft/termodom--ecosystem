import { TextFieldVariants } from "@mui/material"

export interface IEnchantedTextFieldProps {
    readOnly?: boolean | undefined
    defaultValue?: string | number | undefined
    value?: string | number | undefined
    formatValue?: boolean | undefined
    textAlignment?: `left` | `center` | `right` | undefined
    formatValueSuffix?: string | undefined
    formatValuePrefix?: string | undefined
    label?: string | undefined
    subLabel?: string | undefined
    subLabelPrefix?: string | undefined
    subLabelSuffix?: string | undefined
    multiline?: boolean | undefined
    onChange?: (value: string) => void
    fullWidth?: boolean | undefined
    variant?: TextFieldVariants | undefined
    inputType?: `text` | `number`
    allowDecimal?: boolean | undefined
}