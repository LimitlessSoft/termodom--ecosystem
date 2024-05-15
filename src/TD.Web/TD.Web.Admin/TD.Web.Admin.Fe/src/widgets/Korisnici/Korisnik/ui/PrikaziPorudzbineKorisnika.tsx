import { IPrikaziPorudzbineKorisnikaProps } from "@/widgets/Korisnici/Korisnik/interfaces/IPrikaziPorudzbineKorisnikaProps";
import { Button } from "@mui/material";
import NextLink from "next/link";

export const PrikaziPorudzbineKorisnika = (props: IPrikaziPorudzbineKorisnikaProps): JSX.Element => {
    return (
        <Button color={`info`} variant={`contained`} fullWidth LinkComponent={NextLink} href={`/korisnici/${props.username}/porudzbine?userId=${props.userId}`}>
            Prikaži porudžbine korisnika
        </Button>
    )
}