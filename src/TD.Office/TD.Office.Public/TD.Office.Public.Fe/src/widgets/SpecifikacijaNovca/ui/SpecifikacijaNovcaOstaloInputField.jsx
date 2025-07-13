import { SpecifikacijaNovcaDataField } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaDataField'

export const SpecifikacijaNovcaOstaloInputField = (props) => {
    return (
        <SpecifikacijaNovcaDataField
            label={`${props.label}`}
            value={props.value}
            onChange={(value) => {
                if (props.label.toLowerCase()) {
                    props.onChange(props.label.toLowerCase(), +value)
                }
            }}
        />
    )
}
