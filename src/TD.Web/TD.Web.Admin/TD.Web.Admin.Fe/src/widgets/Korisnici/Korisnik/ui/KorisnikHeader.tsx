import { Grid, MenuItem, TextField, Typography } from "@mui/material"
import { KorisnikHeaderWrapperStyled } from "./KorisnikHeaderWrapperStyled"
import { KorisnikInfoBoxStyled } from "./KorisnikInfoBoxStyled"

export const KorisnikHeader = (props: any): JSX.Element => {
    return (
        <Grid item
            p={2}>
            <KorisnikHeaderWrapperStyled container>
                <Grid item>
                    <KorisnikInfoBoxStyled container>
                            <Grid item>
                                <Typography>
                                    {props.user.id}
                                </Typography>
                            </Grid>
                    </KorisnikInfoBoxStyled>
                </Grid>
                <Grid item>
                    <TextField
                        id='user-type'
                        select
                        value={0}
                        // value={0}
                        // onChange={(e) => {
                        // }}
                        label='Tip korisnika'>
                            <MenuItem value={0}>
                                Korisnik
                            </MenuItem>
                    </TextField>
                </Grid>
                <Grid item>
                    <TextField
                        id='user-type'
                        value={props.user.username}
                        label='Username'>
                            <MenuItem value={0}>
                                Korisnik
                            </MenuItem>
                    </TextField>
                </Grid>
                <Grid item>
                    <TextField
                        id='user-status'
                        select
                        value={0}
                        // value={0}
                        // onChange={(e) => {
                        // }}
                        label='Status'>
                            <MenuItem value={0}>
                                Aktivan
                            </MenuItem>
                    </TextField>
                </Grid>
                <Grid item>
                    <KorisnikInfoBoxStyled container>
                            <Grid item>
                                <Typography>
                                    Referent: { props.user.referent }
                                </Typography>
                            </Grid>
                    </KorisnikInfoBoxStyled>
                </Grid>
            </KorisnikHeaderWrapperStyled>
        </Grid>
    )
}