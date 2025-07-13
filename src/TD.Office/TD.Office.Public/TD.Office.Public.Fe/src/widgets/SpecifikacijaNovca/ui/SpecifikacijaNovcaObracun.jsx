import { Grid, Typography } from '@mui/material'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { mainTheme } from '@/themes'
import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'

export const SpecifikacijaNovcaObracun = ({
    racunarTraziLabel,
    obracunRazlika,
    imaNefiskalizovanih,
}) => {
    return (
        <Grid item xs={12}>
            <SpecifikacijaNovcaBox
                title={`Obracun`}
                backgroundColor={
                    Math.abs(obracunRazlika) > 5 || imaNefiskalizovanih
                        ? mainTheme.palette.error.light
                        : mainTheme.palette.success.light
                }
            >
                {imaNefiskalizovanih && (
                    <Typography
                        textAlign={`center`}
                        color={mainTheme.palette.error.main}
                        fontSize={mainTheme.typography.h4.fontSize}
                    >
                        Imate nefiskalizovanih racuna ili povratnica u racunaru
                    </Typography>
                )}
                <Grid container spacing={2} my={3} justifyContent={`end`}>
                    <Grid item>
                        <EnchantedTextField
                            variant={`filled`}
                            readOnly
                            label={`Racunar trazi:`}
                            value={racunarTraziLabel}
                        />
                    </Grid>
                    <Grid item>
                        <EnchantedTextField
                            variant={`filled`}
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
