import { SpecifikacijaNovcaDataField } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaDataField'
import { ISpecifikacijaNovcaOstaloInputFieldProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaOstaloInputFieldProps'

export const SpecifikacijaNovcaOstaloInputField = (
    props: ISpecifikacijaNovcaOstaloInputFieldProps
) => {
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
