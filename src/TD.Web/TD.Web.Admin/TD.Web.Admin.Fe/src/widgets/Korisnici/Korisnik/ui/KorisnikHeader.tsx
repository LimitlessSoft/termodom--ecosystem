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
import { toast } from 'react-toastify'
import { adminApi, handleApiError } from '@/apis/adminApi'

export const KorisnikHeader = (props: any): JSX.Element => {
    const [isActive, setIsActive] = useState<boolean>(props.user.isActive)
    const [userTypes, setUserTypes] = useState<any | undefined>(undefined)

    useEffect(() => {
        adminApi
            .get(`/user-types`)
            .then((response) => {
                setUserTypes(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [])

    useEffect(() => {
        setIsActive(props.user.isActive)
    }, [props.user.isActive])

    const updateUserType = (e: number) => {
        adminApi
            .put(`/users/${props.user.username}/type/${e}`)
            .then(() => {
                toast.success('Uspešno promenjen tip korisnika')
            })
            .catch((err) => handleApiError(err))
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
                            adminApi
                                .put(
                                    `/users/${props.user.username}/status/${parseInt(e.target.value) == 0 ? 'false' : 'true'}`
                                )
                                .then(() => {
                                    toast.success(
                                        'Uspešno promenjen status korisnika'
                                    )
                                    setIsActive(parseInt(e.target.value) == 1)
                                })
                                .catch((err) => handleApiError(err))
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
