import { CircularProgress, Grid, MenuItem, TextField, Typography } from "@mui/material"
import { KorisnikHeaderWrapperStyled } from "./KorisnikHeaderWrapperStyled"
import { KorisnikInfoBoxStyled } from "./KorisnikInfoBoxStyled"
import { useEffect, useState } from "react"
import { ApiBase, fetchApi } from "@/app/api"

export const KorisnikHeader = (props: any): JSX.Element => {

    const [userTypes, setUserTypes] = useState<any | undefined>(undefined)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/user-types`)
        .then((r) => {
            setUserTypes(r)
        })
    }, [])

    return (
        <Grid item
            p={2}>
            <KorisnikHeaderWrapperStyled container>
                <Grid item>
                    <KorisnikInfoBoxStyled container>
                        <Grid item>
                            <Typography>
                                Id: {props.user.id}
                            </Typography>
                        </Grid>
                    </KorisnikInfoBoxStyled>
                </Grid>
                <Grid item>
                    {
                        userTypes === undefined ?
                            <CircularProgress /> :
                            <TextField
                                id='user-type'
                                select
                                defaultValue={props.user.type}
                                label='Tip korisnika'>
                                    {
                                        userTypes.map((ut: any, index: number) => (
                                            <MenuItem key={index} value={ut.id}>
                                                {ut.name}
                                            </MenuItem>
                                        ))
                                    }
                            </TextField>
                    }
                </Grid>
                <Grid item>
                    <KorisnikInfoBoxStyled container>
                        <Grid item>
                            <Typography>
                                Username: <b>{props.user.username}</b>
                            </Typography>
                        </Grid>
                    </KorisnikInfoBoxStyled>
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