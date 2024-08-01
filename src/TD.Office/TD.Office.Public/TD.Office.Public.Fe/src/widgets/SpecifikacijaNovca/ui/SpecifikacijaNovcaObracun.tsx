import { Grid, Typography } from '@mui/material'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { mainTheme } from '@/themes'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { ISpecifikacijaNovcaObracunProps } from '../interfaces/ISpecifikacijaNovcaObracunProps'

export const SpecifikacijaNovcaObracun = ({
    racunarTraziLabel,
    obracunRazlika,
}: ISpecifikacijaNovcaObracunProps) => {
    return (
        <Grid item xs={12}>
            <SpecifikacijaNovcaBox title={`Obracun`}>
                <Typography
                    textAlign={`center`}
                    color={mainTheme.palette.error.main}
                    fontSize={mainTheme.typography.h4.fontSize}
                >
                    Imate nefiskalizovanih racuna ili povratnica u racunaru
                </Typography>
                <Grid container spacing={2} my={3} justifyContent={`end`}>
                    <Grid item>
                        <EnchantedTextField
                            readOnly
                            label={`Racunar trazi:`}
                            value={racunarTraziLabel}
                        />
                    </Grid>
                    <Grid item>
                        <EnchantedTextField
                            readOnly
                            label={`Razlika:`}
                            value={obracunRazlika}
                            formatValue
                            formatValueSuffix={` RSD`}
                            inputType={`number`}
                        />
                    </Grid>
                </Grid>
            </SpecifikacijaNovcaBox>
        </Grid>
    )
}
