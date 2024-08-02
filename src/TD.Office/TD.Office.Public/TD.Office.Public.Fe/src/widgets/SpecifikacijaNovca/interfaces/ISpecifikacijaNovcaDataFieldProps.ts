export interface ISpecifikacijaNovcaDataFieldProps {
    readOnly?: boolean
    defaultValue?: string | number | undefined
    value?: string | number
    label?: string
    subLabel?: string
    multiline?: boolean
    onChange?: (value: string) => void
}
