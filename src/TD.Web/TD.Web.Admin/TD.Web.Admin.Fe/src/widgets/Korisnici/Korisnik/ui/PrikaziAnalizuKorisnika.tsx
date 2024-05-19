import { Button } from "@mui/material"
import NextLink from "next/link"

export const PrikaziAnalizuKorisnika = (props: any): JSX.Element => {
    return (
        <Button color={`info`} variant={`contained`} fullWidth LinkComponent={NextLink} href={`/korisnici/${props.username}/analiza`}>
            Analiza korisnika
        </Button>
    )
}