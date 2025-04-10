import {
    AUTOCOMPLETE_NO_OPTIONS_MESSAGE,
    ProfiKutakTitle,
} from '@/app/constants'
import { mainTheme } from '@/app/theme'
import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import { CustomHead } from '@/widgets/CustomHead'
import {
    Autocomplete,
    Button,
    CircularProgress,
    Grid,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'

import { DatePicker } from '@mui/x-date-pickers'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { Warning } from '@mui/icons-material'
import { handleApiError, webApi } from '@/api/webApi'

const textFieldVariant = 'outlined'
const errorTextVariant = `caption`

const Registrovanje = () => {
    const [cities, setCities] = useState(null)
    const [stores, setStores] = useState(null)

    const [newUser, setNewUser] = useState({})
    const [password1, setPassword1] = useState('')
    const [password2, setPassword2] = useState('')

    const [isNicknameValid, setIsNicknameValid] = useState(false)
    const [isUsernameValid, setIsUsernameValid] = useState(false)
    const [isPasswordValid, setIsPasswordValid] = useState(false)
    const [isMobileValid, setIsMobileValid] = useState(false)
    const [isAddressValid, setIsAddressValid] = useState(false)
    const [isCityIdValid, setIsCityIdValid] = useState(false)
    const [isStoreIdValid, setIsStoreIdValid] = useState(false)
    const [isMailValid, setIsMailValid] = useState(false)

    const [isSubmitting, setIsSubmitting] = useState(false)

    useEffect(() => {
        Promise.all([
            webApi.get('/cities?sortColumn=Name'),
            webApi.get('/stores?sortColumn=Name'),
        ])
            .then(([cities, stores]) => {
                setCities(cities.data)
                setStores(stores.data)
            })
            .catch((err) => handleApiError(err))
    }, [])

    const isAllValid = () => {
        return (
            isNicknameValid &&
            isUsernameValid &&
            isPasswordValid &&
            isMobileValid &&
            isAddressValid &&
            isCityIdValid &&
            isStoreIdValid &&
            isMailValid
        )
    }

    const nicknameMinLength = 6
    const nicknameMaxLength = 32

    useEffect(() => {
        setIsNicknameValid(
            !(
                newUser?.nickname == null ||
                newUser?.nickname == undefined ||
                newUser.nickname.length < nicknameMinLength ||
                newUser.nickname.length > nicknameMaxLength
            )
        )
    }, [newUser, newUser.nickname])

    const usernameMinLength = 6
    const usernameMaxLength = 32

    useEffect(() => {
        setIsUsernameValid(
            !(
                newUser?.username == null ||
                newUser?.username == undefined ||
                newUser.username.length < usernameMinLength ||
                newUser.username.length > usernameMaxLength
            )
        )
    }, [newUser, newUser.username])

    const passwordMinLength = 8

    useEffect(() => {
        let isOk = !(
            password1 == null ||
            password1 == undefined ||
            password1.length < passwordMinLength ||
            password2 == null ||
            password2 == undefined ||
            password2.length < passwordMinLength ||
            password1 != password2 ||
            !/[a-zA-Z]/.test(password1) ||
            !/[0-9]/.test(password1)
        )
        setIsPasswordValid(isOk)

        if (isOk)
            setNewUser((prev) => {
                return { ...prev, password: password1 }
            })
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

    useEffect(() => {
        setIsMobileValid(
            !(
                newUser?.mobile == null ||
                newUser?.mobile == undefined ||
                newUser?.mobile.length < 9 ||
                newUser?.mobile.length > 10
            )
        )
    }, [newUser, newUser.mobile])

    useEffect(() => {
        setIsAddressValid(
            !(
                newUser?.address == null ||
                newUser?.address == undefined ||
                newUser?.address.length < 5
            )
        )
    }, [newUser, newUser.address])

    useEffect(() => {
        setIsCityIdValid(
            !(newUser?.cityId == null || newUser?.cityId == undefined)
        )
    }, [newUser, newUser.cityId])

    useEffect(() => {
        setIsStoreIdValid(
            !(
                newUser?.favoriteStoreId == null ||
                newUser?.favoriteStoreId == undefined
            )
        )
    }, [newUser, newUser.favoriteStoreId])

    useEffect(() => {
        setIsMailValid(
            !(
                newUser?.mail == null ||
                newUser?.mail == undefined ||
                newUser.mail.length < 5 ||
                newUser.mail.indexOf('@') < 0
            )
        )
    }, [newUser, newUser.mail])

    return (
        <CenteredContentWrapper>
            <CustomHead title={ProfiKutakTitle} />
            <Stack
                direction={`column`}
                alignItems={`center`}
                gap={2}
                px={{ xs: 2, lg: 0 }}
            >
                <Stack gap={4}>
                    <Paper
                        elevation={8}
                        sx={{
                            backgroundColor: mainTheme.palette.warning.main,
                            color: mainTheme.palette.warning.contrastText,
                            p: 2,
                        }}
                    >
                        <Grid
                            container
                            alignItems={`center`}
                            gap={2}
                            justifyContent={`center`}
                        >
                            <Grid item>
                                <Warning />
                            </Grid>
                            <Grid item>
                                <Typography variant={`h6`} textAlign={`center`}>
                                    Kupovinu{' '}
                                    <b
                                        style={{
                                            color: mainTheme.palette.success
                                                .main,
                                        }}
                                    >
                                        sa popustom
                                    </b>{' '}
                                    možete izvršiti bez registrovanja! - Dodajte
                                    proizvode u korpu i završite kupovinu.
                                </Typography>
                            </Grid>
                        </Grid>
                    </Paper>
                    <Paper
                        elevation={8}
                        sx={{
                            backgroundColor: mainTheme.palette.warning.main,
                            color: mainTheme.palette.warning.contrastText,
                            p: 2,
                        }}
                    >
                        <Grid
                            container
                            alignItems={`center`}
                            gap={2}
                            justifyContent={`center`}
                        >
                            <Grid item>
                                <Warning />
                            </Grid>
                            <Grid item>
                                <Typography textAlign={`center`}>
                                    Registracija je potrebna samo ukoliko često
                                    kupujete i želite imati uvek najjeftinije
                                    cene bez obzira na količinu!
                                </Typography>
                            </Grid>
                        </Grid>
                    </Paper>

                    <Paper
                        elevation={8}
                        sx={{
                            backgroundColor: mainTheme.palette.warning.main,
                            color: mainTheme.palette.warning.contrastText,
                            p: 2,
                        }}
                    >
                        <Grid
                            container
                            alignItems={`center`}
                            gap={2}
                            justifyContent={`center`}
                        >
                            <Grid item>
                                <Warning />
                            </Grid>
                            <Grid item>
                                <Typography textAlign={`center`}>
                                    Nakon registracije, kontaktiraćemo Vas i
                                    ukoliko ispunjavate uslove (kupujete često),
                                    nalog će Vam biti aktiviran.
                                </Typography>
                            </Grid>
                        </Grid>
                    </Paper>
                </Stack>
                <Typography sx={{ my: 2 }} variant={`h6`}>
                    Postani profi kupac - registracija
                </Typography>
                <Stack sx={{ maxWidth: 400, gap: 2, width: `100%` }}>
                    <TextField
                        required
                        error={!isNicknameValid}
                        id="nickname"
                        label="Puno ime i prezime"
                        helperText={
                            isNicknameValid ? null : (
                                <Typography variant={errorTextVariant}>
                                    Ime i prezime mora imati između{' '}
                                    {nicknameMinLength} i {nicknameMaxLength}{' '}
                                    karaktera.
                                </Typography>
                            )
                        }
                        onChange={(e) => {
                            setNewUser((prev) => {
                                return { ...prev, nickname: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                    <TextField
                        required
                        error={!isUsernameValid}
                        id="username"
                        label="Korisničko ime"
                        helperText={
                            isUsernameValid ? null : (
                                <Typography variant={errorTextVariant}>
                                    Korisničko ime mora imati između{' '}
                                    {usernameMinLength} i {usernameMaxLength}{' '}
                                    karaktera.
                                </Typography>
                            )
                        }
                        onChange={(e) => {
                            setNewUser((prev) => {
                                return { ...prev, username: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                    <TextField
                        required
                        error={!isPasswordValid}
                        type={`password`}
                        id="password1"
                        label="Lozinka"
                        helperText={
                            isPasswordValid ? null : (
                                <>
                                    {isPasswordLengthOk() ? null : (
                                        <Typography variant={errorTextVariant}>
                                            Lozinka mora imati najmanje{' '}
                                            {passwordMinLength} karaktera.
                                        </Typography>
                                    )}
                                    {doesPasswordContainLetter() ? null : (
                                        <Typography variant={errorTextVariant}>
                                            Lozinka mora sadržati najmanje jedno
                                            slovo.
                                        </Typography>
                                    )}
                                    {doesPasswordContainNumber() ? null : (
                                        <Typography variant={errorTextVariant}>
                                            Lozinka mora sadržati najmanje jednu
                                            cifru.
                                        </Typography>
                                    )}
                                </>
                            )
                        }
                        onChange={(e) => {
                            setPassword1(e.target.value)
                        }}
                        variant={textFieldVariant}
                    />
                    <TextField
                        required
                        error={!isPasswordValid}
                        type={`password`}
                        id="password2"
                        label="Ponovi lozinku"
                        helperText={
                            isPsswordSame() ? null : (
                                <Typography variant={errorTextVariant}>
                                    Lozinke se ne poklapaju.
                                </Typography>
                            )
                        }
                        onChange={(e) => {
                            setPassword2(e.target.value)
                        }}
                        variant={textFieldVariant}
                    />
                    <Stack>
                        <Typography>Datum rođenja</Typography>
                        <DatePicker
                            onChange={(e) => {
                                setNewUser((prev) => {
                                    return {
                                        ...prev,
                                        dateOfBirth: new Date(e.$d),
                                    }
                                })
                            }}
                        />
                    </Stack>
                    <TextField
                        required
                        error={!isMobileValid}
                        id="mobile"
                        label="Mobilni telefon"
                        onChange={(e) => {
                            setNewUser((prev) => {
                                return { ...prev, mobile: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                        helperText={
                            isMobileValid ? null : (
                                <Typography variant={errorTextVariant}>
                                    Mobilni telefon nije ispravno unet. Unesite
                                    samo cifre bez razmaka.
                                </Typography>
                            )
                        }
                    />
                    <TextField
                        required
                        error={!isAddressValid}
                        id="address"
                        label="Adresa stanovanja"
                        onChange={(e) => {
                            setNewUser((prev) => {
                                return { ...prev, address: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                    {cities == null || cities.length == 0 ? (
                        <CircularProgress />
                    ) : (
                        <Autocomplete
                            options={[...cities].sort((a, b) =>
                                a.name.localeCompare(b.name)
                            )}
                            onChange={(_event, value) => {
                                console.log(value)
                                setNewUser((prev) => ({
                                    ...prev,
                                    cityId: value?.id ?? null,
                                }))
                            }}
                            getOptionLabel={(option) => option.name}
                            noOptionsText={AUTOCOMPLETE_NO_OPTIONS_MESSAGE}
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    label={`Mesto stanovanja`}
                                />
                            )}
                        />
                    )}
                    {stores == null || stores.length == 0 ? (
                        <CircularProgress />
                    ) : (
                        <Autocomplete
                            options={[...stores].sort((a, b) =>
                                a.name.localeCompare(b.name)
                            )}
                            getOptionLabel={(option) => option.name}
                            onChange={(_event, value) => {
                                console.log(value)
                                setNewUser((prev) => ({
                                    ...prev,
                                    favoriteStoreId: value?.id ?? null,
                                }))
                            }}
                            noOptionsText={AUTOCOMPLETE_NO_OPTIONS_MESSAGE}
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    label={`Omiljena radnja`}
                                />
                            )}
                        />
                    )}
                    <TextField
                        required
                        error={!isMailValid}
                        id="email"
                        label="Važeća email adresa"
                        onChange={(e) => {
                            setNewUser((prev) => {
                                return { ...prev, mail: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />

                    {isAllValid() ? null : (
                        <Typography
                            color={mainTheme.palette.error.light}
                            sx={{ my: 2, textAlign: `center` }}
                        >
                            Morate ispravno popuniti sva polja!
                        </Typography>
                    )}
                    <Button
                        disabled={!isAllValid() || isSubmitting}
                        variant={`contained`}
                        onClick={() => {
                            setIsSubmitting(true)
                            webApi
                                .put('register', newUser)
                                .then(() => {
                                    toast(
                                        'Zahtev za registraciju uspešno kreiran. Bićete obavešteni o aktivaciji naloga u roku od 7 dana.',
                                        {
                                            type: 'success',
                                            autoClose: false,
                                        }
                                    )
                                })
                                .catch((err) => {
                                    handleApiError(err)
                                })
                                .finally(() => setIsSubmitting(false))
                        }}
                    >
                        Podnesi zahtev za registraciju
                    </Button>
                </Stack>
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Registrovanje
