import { SpecifikacijaNovcaDataField } from '@/widgets/SpecifikacijaNovca/ui/SpecifikacijaNovcaDataField'
import { formatNumber } from '@/helpers/numberHelpers'
import { ISpecifikacijaNovcaGotovinaInputFieldProps } from '@/widgets/SpecifikacijaNovca/interfaces/ISpecifikacijaNovcaGotovinaInputFieldProps'

export const SpecifikacijaNovcaGotovinaInputField = (
    props: ISpecifikacijaNovcaGotovinaInputFieldProps
) => {
    console.log(props.gotovinaReference)
    return (
        <SpecifikacijaNovcaDataField
            label={`${props.note} x`}
            defaultValue={props.gotovinaReference[`b${props.note}`]}
            onChange={(value) => {
                props.onChange(props.note, value)
            }}
            subLabel={formatNumber(
                props.note * props.gotovinaReference[`b${props.note}`]
            )}
        />
    )
}
