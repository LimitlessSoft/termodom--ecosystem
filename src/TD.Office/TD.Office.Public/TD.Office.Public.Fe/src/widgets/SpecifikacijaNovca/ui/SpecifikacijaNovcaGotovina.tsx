import { Stack } from '@mui/material'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { SpecifikacijaNovcaGotovinaInputField } from './SpecifikacijaNovcaGotovinaInputField'
import { ISpecifikacijaNovcaGotovinaProps } from '../interfaces/ISpecifikacijaNovcaGotovinaProps'

export const SpecifikacijaNovcaGotovina = ({
    gotovina,
    ukupnoGotovine,
    onChange,
}: ISpecifikacijaNovcaGotovinaProps) => {
    return (
        <SpecifikacijaNovcaBox title={`Specifikacija novca - gotovina`}>
            <Stack spacing={2}>
                {gotovina &&
                    gotovina.map((novcanica, i) => {
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
                    })}
                <EnchantedTextField
                    label={`Ukupno gotovine:`}
                    value={ukupnoGotovine}
                    readOnly
                    formatValue
                    inputType={`number`}
                    formatValueSuffix={` RSD`}
                />
            </Stack>
        </SpecifikacijaNovcaBox>
    )
}
