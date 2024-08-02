import { EnchantedTextField } from '@/widgets/EnchantedTextField/ui/EnchantedTextField'
import { Button, Grid, Stack } from '@mui/material'
import { SpecifikacijaNovcaBox } from './SpecifikacijaNovcaBox'
import { Bolt } from '@mui/icons-material'
import { ISpecifikacijaNovcaPoreskaProps } from '../interfaces/ISpecifikacijaNovcaPoreskaProps'

export const SpecifikacijaNovcaPoreska = ({
    poreska,
}: ISpecifikacijaNovcaPoreskaProps) => {
    return (
        <SpecifikacijaNovcaBox title={`Poreska`}>
            {poreska && (
                <Stack spacing={2}>
                    <Grid container spacing={2} alignItems={`center`}>
                        <EnchantedTextField
                            readOnly
                            label={`Fiskalizovani racuni:`}
                            defaultValue={poreska.fiskalizovaniRacuni}
                        />
                        <Grid item>
                            <Button variant={`contained`}>
                                <Bolt />
                            </Button>
                        </Grid>
                    </Grid>
                    <Grid container spacing={2} alignItems={`center`}>
                        <EnchantedTextField
                            readOnly
                            label={`Fiskalizovane povratnice:`}
                            defaultValue={poreska.fiskalizovanePovratnice}
                        />
                        <Grid item>
                            <Button variant={`contained`}>
                                <Bolt />
                            </Button>
                        </Grid>
                    </Grid>
                </Stack>
            )}
        </SpecifikacijaNovcaBox>
    )
}
