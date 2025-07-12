import { Stack } from '@mui/material'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { SpecifikacijaNovcaGotovinaInputField } from './SpecifikacijaNovcaGotovinaInputField'
import { getUkupnoGotovine } from '@/widgets/SpecifikacijaNovca/helpers/SpecifikacijaHelpers'
import { useEffect } from 'react'

export const SpecifikacijaNovcaGotovina = ({
    specifikacija,
    onChange,
    disabled,
}) => {
    return (
        <SpecifikacijaNovcaBox title={`Specifikacija novca - gotovina`}>
            <Stack spacing={2}>
                {specifikacija.specifikacijaNovca.novcanice.map(
                    (novcanica, i) => {
                        return (
                            <SpecifikacijaNovcaGotovinaInputField
                                disabled={disabled}
                                key={i}
                                note={novcanica.key}
                                gotovinaReference={(
                                    novcanica.key * novcanica.value
                                ).toString()}
                                value={novcanica.value}
                                onChange={(note, value) => {
                                    onChange(note, parseFloat(value))
                                }}
                            />
                        )
                    }
                )}
                <EnchantedTextField
                    disabled={disabled}
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
