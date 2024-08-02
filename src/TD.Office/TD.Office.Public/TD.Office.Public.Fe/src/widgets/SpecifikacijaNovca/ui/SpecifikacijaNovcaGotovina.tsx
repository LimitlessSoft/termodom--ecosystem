import { Stack } from '@mui/material'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { SpecifikacijaNovcaGotovinaInputField } from './SpecifikacijaNovcaGotovinaInputField'
import { ISpecifikacijaNovcaGotovinaProps } from '../interfaces/ISpecifikacijaNovcaGotovinaProps'
import { getUkupnoGotovine } from '@/widgets/SpecifikacijaNovca/helpers/SpecifikacijaHelpers'

export const SpecifikacijaNovcaGotovina = ({
    specifikacija,
    onChange,
}: ISpecifikacijaNovcaGotovinaProps) => {
    return (
        <SpecifikacijaNovcaBox title={`Specifikacija novca - gotovina`}>
            <Stack spacing={2}>
                {specifikacija.specifikacijaNovca.novcanice.map(
                    (novcanica, i) => {
                        return (
                            <SpecifikacijaNovcaGotovinaInputField
                                key={i}
                                note={novcanica.key}
                                gotovinaReference={(
                                    novcanica.key * novcanica.value
                                ).toString()}
                                value={novcanica.value}
                                onChange={(note: number, value: string) => {
                                    onChange(note, parseFloat(value))
                                }}
                            />
                        )
                    }
                )}
                <EnchantedTextField
                    label={`Ukupno gotovine:`}
                    value={getUkupnoGotovine(specifikacija)}
                    readOnly
                    formatValue
                    inputType={`number`}
                    formatValueSuffix={` RSD`}
                />
            </Stack>
        </SpecifikacijaNovcaBox>
    )
}
