export interface IKorisniciSingularDataFieldProps {
    preLabel?: string
    defaultValue: string
    editable?: boolean
    onSave?: (value: string) => Promise<void>
}
