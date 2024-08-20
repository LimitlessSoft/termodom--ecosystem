import {
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
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
import { mainTheme } from '@/theme'
import { Delete } from '@mui/icons-material'
import { useRouter } from 'next/router'

export const KorisnikHeader = (props: any) => {
    const router = useRouter()
    const [isDeleteDialogOpen, setIsDeleteDialogOpen] = useState<boolean>(false)
    const [userTypes, setUserTypes] = useState<any | undefined>(undefined)

    useEffect(() => {
        adminApi
            .get(`/user-types`)
            .then((response) => {
                setUserTypes(response.data)
            })
            .catch((err) => handleApiError(err))
    }, [])

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
                    <KorisnikInfoBoxStyled
                        container
                        sx={{
                            backgroundColor:
                                props.user.status === 'Na obradi (aktivan)'
                                    ? mainTheme.palette.info.light
                                    : props.user.status === 'Aktivan'
                                      ? mainTheme.palette.success.light
                                      : mainTheme.palette.error.light,
                        }}
                    >
                        <Grid item>
                            <Typography>Status: {props.user.status}</Typography>
                        </Grid>
                    </KorisnikInfoBoxStyled>
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

                {props.user.isActive && (
                    <Grid>
                        <KorisnikInfoBoxStyled container>
                            <Grid item>
                                <Dialog open={isDeleteDialogOpen}>
                                    <DialogTitle>
                                        Da li ste sigurni da želite da obrišete
                                        korisnika?
                                    </DialogTitle>
                                    <DialogActions>
                                        <Button
                                            onClick={() => {
                                                setIsDeleteDialogOpen(false)
                                            }}
                                        >
                                            Ne
                                        </Button>
                                        <Button
                                            onClick={() => {
                                                adminApi
                                                    .put(
                                                        `/users/${props.user.username}/status/false`
                                                    )
                                                    .then(() => {
                                                        toast.success(
                                                            'Uspešno promenjen status korisnika'
                                                        )
                                                        router.reload()
                                                    })
                                                    .catch((err) =>
                                                        handleApiError(err)
                                                    )
                                            }}
                                        >
                                            Da
                                        </Button>
                                    </DialogActions>
                                </Dialog>
                                <Button
                                    onClick={() => {
                                        setIsDeleteDialogOpen(true)
                                    }}
                                >
                                    <Delete />
                                </Button>
                            </Grid>
                        </KorisnikInfoBoxStyled>
                    </Grid>
                )}
            </KorisnikHeaderWrapperStyled>
        </Grid>
    )
}
