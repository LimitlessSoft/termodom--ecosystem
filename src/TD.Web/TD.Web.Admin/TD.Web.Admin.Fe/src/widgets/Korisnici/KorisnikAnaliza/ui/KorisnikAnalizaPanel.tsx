import { Grid } from '@mui/material'
import { KorisnikAnalizaPanelStyled } from '../styled/KorisnikAnalizaPanelStyled'

export const KorisnikAnalizaPanel = (props: any): JSX.Element => {
    return (
        <Grid item {...props}>
            <KorisnikAnalizaPanelStyled>
                {props.children}
            </KorisnikAnalizaPanelStyled>
        </Grid>
    )
}
