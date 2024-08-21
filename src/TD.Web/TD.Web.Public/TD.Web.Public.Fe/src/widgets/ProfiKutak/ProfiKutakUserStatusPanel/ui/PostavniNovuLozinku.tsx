import { useEffect, useState } from 'react'
import { IPostavniNovuLozinkuProps } from '../interfaces/IPostavniNovuLozinkuProps'
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Grid,
    TextField,
    Typography,
} from '@mui/material'
import { toast } from 'react-toastify'
import { handleApiError, webApi } from '@/api/webApi'

export const PostaviNovuLozinku = (
    props: IPostavniNovuLozinkuProps
): JSX.Element => {
    const [isOpened, setIsOpened] = useState<boolean>(false)
    const [oldPassword, setOldPassword] = useState<string>(``)
    const [password1, setPassword1] = useState<string>(``)
    const [password2, setPassword2] = useState<string>(``)
    const [isPasswordValid, setIsPasswordValid] = useState<boolean>(false)

    const passwordMinLength = 8
    const errorTextComponent = `p`
    const errorTextVariant = `caption`

    useEffect(() => {
        let isOk = !(
            password1 == null ||
            password1 == undefined ||
            password1.length < passwordMinLength ||
            password2 == null ||
            password2 == undefined ||
            password2.length < passwordMinLength ||
            password1 != password2 ||
            !isPasswordLengthOk() ||
            !doesPasswordContainLetter() ||
            !doesPasswordContainNumber() ||
            !isPsswordSame()
        )
        setIsPasswordValid(isOk)
    }, [password1, password2])

    const isPasswordLengthOk = () => {
        return password1.length >= passwordMinLength
    }

    const doesPasswordContainLetter = () => {
        return /[a-zA-Z]/.test(password1)
    }

    const doesPasswordContainNumber = () => {
        return /[0-9]/.test(password1)
    }

    const isPsswordSame = () => {
        return password1 == password2
    }

    return (
        <Grid>
            <Dialog open={isOpened}>
                <DialogTitle>Postavi novu lozinku</DialogTitle>
                <DialogContent>
                    <Grid container spacing={2} py={2}>
                        <Grid item lg={12}>
                            <TextField
                                fullWidth
                                label={`Stara lozinka`}
                                type={`password`}
                                onChange={(e) => {
                                    setOldPassword(e.target.value)
                                }}
                            />
                        </Grid>
                        <Grid item lg={12}>
                            <TextField
                                fullWidth
                                label={`Nova lozinka`}
                                type={`password`}
                                onChange={(e) => {
                                    setPassword1(e.target.value)
                                }}
                            />
                        </Grid>
                        <Grid item lg={12}>
                            <TextField
                                fullWidth
                                label={`Potvrdi novu lozinku`}
                                type={`password`}
                                onChange={(e) => {
                                    setPassword2(e.target.value)
                                }}
                                helperText={
                                    isPasswordValid ? null : (
                                        <Grid>
                                            {isPasswordLengthOk() ? null : (
                                                <Typography
                                                    component={
                                                        errorTextComponent
                                                    }
                                                    variant={errorTextVariant}
                                                >
                                                    Lozinka mora imati najmanje{' '}
                                                    {passwordMinLength}{' '}
                                                    karaktera.
                                                </Typography>
                                            )}
                                            {doesPasswordContainLetter() ? null : (
                                                <Typography
                                                    component={
                                                        errorTextComponent
                                                    }
                                                    variant={errorTextVariant}
                                                >
                                                    Lozinka mora sadržati
                                                    najmanje jedno slovo.
                                                </Typography>
                                            )}
                                            {doesPasswordContainNumber() ? null : (
                                                <Typography
                                                    component={
                                                        errorTextComponent
                                                    }
                                                    variant={errorTextVariant}
                                                >
                                                    Lozinka mora sadržati
                                                    najmanje jednu cifru.
                                                </Typography>
                                            )}
                                            {isPsswordSame() ? null : (
                                                <Grid>
                                                    <Typography
                                                        component={
                                                            errorTextComponent
                                                        }
                                                        variant={
                                                            errorTextVariant
                                                        }
                                                    >
                                                        Lozinke se ne poklapaju.
                                                    </Typography>
                                                </Grid>
                                            )}
                                        </Grid>
                                    )
                                }
                            />
                        </Grid>
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            setIsOpened(false)
                        }}
                    >
                        Otkaži
                    </Button>
                    <Button
                        variant={`contained`}
                        disabled={isPasswordValid === false}
                        onClick={() => {
                            webApi
                                .put('/set-password', {
                                    password: password1,
                                    oldPassword: oldPassword,
                                })
                                .then(() => {
                                    toast.success(`Lozinka uspešno promenjena.`)
                                })
                                .catch((err) => handleApiError(err))
                                .finally(() => {
                                    setIsOpened(false)
                                })
                        }}
                    >
                        Postavi novu lozniku
                    </Button>
                </DialogActions>
            </Dialog>
            <Button
                variant={`contained`}
                fullWidth
                onClick={() => {
                    setIsOpened(true)
                }}
            >
                Promeni lozinku
            </Button>
        </Grid>
    )
}
