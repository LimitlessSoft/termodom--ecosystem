import { Grid, Typography } from "@mui/material"

export const KorpaDiscountAlert = (): JSX.Element => {
    return (
        <Grid m={5}>
            <Typography
                align={`justify`}>
                Trenutna ukupna vrednost vašeg računa bez PDV-a iznosi X.XXX.XX i dodeljeni su Vam rabati stepena X.
                Ukoliko ukupna vrednost računa pređe X.XXX.XX RSD stepen rabata će biti ažuriran!
                *Rabat se obracunava na ukupnu vrednost korpe bez pdv-a
            </Typography>
        </Grid>
    )
}