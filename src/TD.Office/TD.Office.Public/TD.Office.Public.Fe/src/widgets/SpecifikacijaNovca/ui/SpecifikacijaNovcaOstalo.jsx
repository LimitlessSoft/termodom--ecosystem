import { Grid, Stack } from '@mui/material'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { SpecifikacijaNovcaOstaloCommentButton } from './SpecifikacijaNovcaOstaloCommentButton'

export const SpecifikacijaNovcaOstalo = ({ ostalo, onChange }) => {
    return (
        <SpecifikacijaNovcaBox title={`Specifikacija Novca - Ostalo`}>
            {ostalo && (
                <Stack gap={2}>
                    {ostalo.map((field, index) => {
                        const label = `${
                            field.key.charAt(0).toUpperCase() +
                            field.key.slice(1)
                        }:`

                        return (
                            <Grid
                                key={index}
                                container
                                spacing={2}
                                alignItems={`center`}
                            >
                                <EnchantedTextField
                                    textAlignment={`left`}
                                    inputType={`number`}
                                    allowDecimal
                                    label={label}
                                    defaultValue={field.vrednost}
                                    onChange={(e) =>
                                        onChange(field.key, parseFloat(e))
                                    }
                                />
                                <SpecifikacijaNovcaOstaloCommentButton
                                    comment={field.komentar}
                                    title={label}
                                    onSave={(comment) => {
                                        throw new Error('Not implemented')
                                    }}
                                />
                            </Grid>
                        )
                    })}
                </Stack>
            )}
        </SpecifikacijaNovcaBox>
    )
}
