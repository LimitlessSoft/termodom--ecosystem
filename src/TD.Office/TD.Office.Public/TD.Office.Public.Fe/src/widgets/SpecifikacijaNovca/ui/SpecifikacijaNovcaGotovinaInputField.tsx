import { SpecifikacijaNovcaDataField } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaDataField'
import { formatNumber } from '@/helpers/numberHelpers'
import { ISpecifikacijaNovcaGotovinaInputFieldProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaGotovinaInputFieldProps'

export const SpecifikacijaNovcaGotovinaInputField = (
    props: ISpecifikacijaNovcaGotovinaInputFieldProps
) => {
    return (
        <SpecifikacijaNovcaDataField
            label={`${props.note} x`}
            value={props.value}
            onChange={(value) => {
                if (props.note) {
                    props.onChange(props.note, value)
                }
            }}
            subLabel={props.gotovinaReference}
        />
    )
}
