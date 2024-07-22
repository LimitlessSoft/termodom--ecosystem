export interface ISpecifikacijaNovcaDataFieldProps {
    readonly?: boolean
    defaultValue?: string | number
    value?: string | number
    label?: string
    subLabel?: string
    multiline?: boolean
    onChange?: (value: string) => void
}
