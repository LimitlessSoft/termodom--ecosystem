import { ISpecifikacijaNovcaGotovinaInputFieldProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaGotovinaInputFieldProps'
import { EnchantedTextField } from '@/widgets'

export const SpecifikacijaNovcaGotovinaInputField = (
    props: ISpecifikacijaNovcaGotovinaInputFieldProps
) => {
    return (
        <EnchantedTextField
            subLabelPrefix={`= `}
            inputType={`number`}
            label={`${props.note} x`}
            defaultValue={props.value}
            textAlignment={`left`}
            onChange={(value) => {
                if (props.note) {
                    props.onChange(props.note, value)
                }
            }}
            subLabel={props.gotovinaReference}
        />
    )
}
