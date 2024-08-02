import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { Stack } from '@mui/material'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { ISpecifikacijaNovcaRacunarProps } from '../interfaces/ISpecifikacijaNovcaRacunarProps'

export const SpecifikacijaNovcaRacunar = ({
    racunar,
}: ISpecifikacijaNovcaRacunarProps) => {
    return (
        <SpecifikacijaNovcaBox title={`Racunar`}>
            {racunar && (
                <Stack spacing={2}>
                    <EnchantedTextField
                        readOnly
                        fullWidth
                        label={`1) Gotovinski racuni:`}
                        defaultValue={racunar.gotovinskiRacuni}
                    />
                    <EnchantedTextField
                        readOnly
                        fullWidth
                        label={`2) Virmanski racuni:`}
                        defaultValue={racunar.virmanskiRacuni}
                    />
                    <EnchantedTextField
                        readOnly
                        fullWidth
                        label={`3) Kartice:`}
                        defaultValue={racunar.kartice}
                    />
                    <EnchantedTextField
                        readOnly
                        fullWidth
                        label={`Ukupno racunar (1+2+3):`}
                        defaultValue={racunar.ukupnoRacunar}
                    />
                    <EnchantedTextField
                        readOnly
                        fullWidth
                        label={`Gotovinske povratnice:`}
                        defaultValue={racunar.gotovinskePovratnice}
                    />
                    <EnchantedTextField
                        readOnly
                        fullWidth
                        label={`Virmanske povratnice:`}
                        defaultValue={racunar.virmanskePovratnice}
                    />
                    <EnchantedTextField
                        readOnly
                        fullWidth
                        label={`Ostale povratnice:`}
                        defaultValue={racunar.ostalePovratnice}
                    />
                </Stack>
            )}
        </SpecifikacijaNovcaBox>
    )
}
