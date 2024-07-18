import {
    CircularProgress,
    Grid,
    MenuItem,
    TextField,
    Typography,
} from '@mui/material'
import { KorisnikHeaderWrapperStyled } from './KorisnikHeaderWrapperStyled'
import { KorisnikInfoBoxStyled } from './KorisnikInfoBoxStyled'
import { useEffect, useState } from 'react'
import { ApiBase, fetchApi } from '@/api'
import { toast } from 'react-toastify'

export const KorisnikHeader = (props: any): JSX.Element => {
    const [isActive, setIsActive] = useState<boolean>(props.user.isActive)
    const [userTypes, setUserTypes] = useState<any | undefined>(undefined)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/user-types`)
            .then((response) => response.json())
            .then((data) => setUserTypes(data))
    }, [])

    useEffect(() => {
        setIsActive(props.user.isActive)
    }, [props.user.isActive])

    const updateUserType = (e: number) => {
        fetchApi(ApiBase.Main, `/users/${props.user.username}/type/${e}`, {
            method: 'PUT',
        }).then(() => {
            toast.success('Uspešno promenjen tip korisnika')
        })
    }

    return (
        <Grid item p={2}>
            <KorisnikHeaderWrapperStyled container>
                <Grid item>
                    <KorisnikInfoBoxStyled container>
                        <Grid item>
                            <Typography>Id: {props.user.id}</Typography>
                        </Grid>
                    </KorisnikInfoBoxStyled>
                </Grid>
                <Grid item>
                    {userTypes === undefined ? (
                        <CircularProgress />
                    ) : (
                        <TextField
                            id="user-type"
                            select
                            disabled={props.disabled}
                            defaultValue={props.user.type}
                            onChange={(e) => {
                                updateUserType(parseInt(e.target.value))
                            }}
                            label="Tip korisnika"
                        >
                            {userTypes.map((ut: any, index: number) => (
                                <MenuItem key={index} value={ut.id}>
                                    {ut.name}
                                </MenuItem>
                            ))}
                        </TextField>
                    )}
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
                        id="user-status"
                        select
                        disabled={props.disabled}
                        value={isActive ? 1 : 0}
                        onChange={(e) => {
                            fetchApi(
                                ApiBase.Main,
                                `/users/${props.user.username}/status/${parseInt(e.target.value) == 0 ? 'false' : 'true'}`,
                                {
                                    method: 'PUT',
                                }
                            ).then(() => {
                                toast.success(
                                    'Uspešno promenjen status korisnika'
                                )
                                setIsActive(parseInt(e.target.value) == 1)
                            })
                        }}
                        label="Status"
                    >
                        <MenuItem value={0}>Neaktivan</MenuItem>
                        <MenuItem value={1}>Aktivan</MenuItem>
                    </TextField>
                </Grid>
                <Grid item>
                    <KorisnikInfoBoxStyled container>
                        <Grid item>
                            <Typography>
                                Referent: {props.user.referent}
                            </Typography>
                        </Grid>
                    </KorisnikInfoBoxStyled>
                </Grid>
            </KorisnikHeaderWrapperStyled>
        </Grid>
    )
}
