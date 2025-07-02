import { ProfiKutakTitle } from '@/app/constants'
import { mainTheme } from '@/app/theme'
import { CenteredContentWrapper } from '@/widgets/CenteredContentWrapper'
import { CustomHead } from '@/widgets/CustomHead'
import {
    Button,
    CircularProgress,
    Grid,
    Paper,
    Stack,
    Switch,
    Typography,
} from '@mui/material'
import { FormProvider, useForm } from 'react-hook-form'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { Warning } from '@mui/icons-material'
import { handleApiError, webApi } from '@/api/webApi'
import { REGISTER_CONSTANTS } from '@/widgets/Register'
import {
    FormValidationInput,
    FormValidationAutocomplete,
    FormValidationDatePicker,
} from '@/widgets/FormValidation'
import { registerFormValidator } from '@/widgets/Register'

const Registrovanje = () => {
    const [cities, setCities] = useState(null)
    const [stores, setStores] = useState(null)
    const [isIndividual, setIsIndividual] = useState(true)

    const { VALIDATION_FIELDS } = REGISTER_CONSTANTS

    const defaultFormValues = {
        [VALIDATION_FIELDS.NICKNAME.FIELD]: '',
        [VALIDATION_FIELDS.COMPANY.FIELD]: '',
        [VALIDATION_FIELDS.PIB.FIELD]: '',
        [VALIDATION_FIELDS.MB.FIELD]: '',
        [VALIDATION_FIELDS.USERNAME.FIELD]: '',
        [VALIDATION_FIELDS.PASSWORD.FIELD]: '',
        [VALIDATION_FIELDS.CONFIRM_PASSWORD.FIELD]: '',
        [VALIDATION_FIELDS.DATE_OF_BIRTH.FIELD]: null,
        [VALIDATION_FIELDS.MOBILE.FIELD]: '',
        [VALIDATION_FIELDS.ADDRESS.FIELD]: '',
        [VALIDATION_FIELDS.CITY.FIELD]: null,
        [VALIDATION_FIELDS.FAVORITE_STORE.FIELD]: null,
        [VALIDATION_FIELDS.MAIL.FIELD]: '',
    }

    const methods = useForm({
        resolver: registerFormValidator(isIndividual),
        mode: 'onChange',
        defaultValues: defaultFormValues,
    })

    const {
        handleSubmit,
        formState: { isValid, isSubmitting },
        reset,
        watch,
        trigger,
    } = methods

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

    useEffect(() => {
        if (isIndividual) {
            reset()
        } else {
            const nickname = watch(VALIDATION_FIELDS.NICKNAME.FIELD)

            reset({
                ...defaultFormValues,
                [VALIDATION_FIELDS.COMPANY.FIELD]: nickname,
            })
        }

        trigger()
    }, [isIndividual])

    const handleSubmitRegistering = (data) => {
        let payload = { ...data }

        if (!isIndividual) {
            payload.nickname = `${data[VALIDATION_FIELDS.COMPANY.FIELD]} (${VALIDATION_FIELDS.PIB.LABEL}: ${data[VALIDATION_FIELDS.PIB.FIELD]}) (${VALIDATION_FIELDS.MB.LABEL}: ${data[VALIDATION_FIELDS.MB.FIELD]})`
        }

        delete payload[VALIDATION_FIELDS.COMPANY.FIELD]
        delete payload[VALIDATION_FIELDS.PIB.FIELD]
        delete payload[VALIDATION_FIELDS.MB.FIELD]
        delete payload[VALIDATION_FIELDS.CONFIRM_PASSWORD.FIELD]

        webApi
            .put('register', payload)
            .then(() => {
                toast(
                    'Zahtev za registraciju uspešno kreiran. Bićete obavešteni o aktivaciji naloga u roku od 7 dana.',
                    {
                        type: 'success',
                        autoClose: false,
                    }
                )
                reset()
            })
            .catch(handleApiError)
    }

    const userTypeModeColor = isIndividual
        ? mainTheme.palette.error.main
        : mainTheme.palette.secondary.light
    const userTypeDifferenceInputBackgroundColor = `${userTypeModeColor}0D`

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
                <Typography sx={{ my: 2 }} variant={`h6`} textAlign={`center`}>
                    Postani profi kupac - registracija
                </Typography>
                <FormProvider {...methods}>
                    <Stack
                        sx={{ maxWidth: 400, gap: 2, width: `100%` }}
                        component={`form`}
                        onSubmit={handleSubmit(handleSubmitRegistering)}
                    >
                        <Paper
                            sx={{
                                backgroundColor: userTypeModeColor,
                            }}
                        >
                            <Stack
                                direction={`row`}
                                justifyContent={`center`}
                                alignItems={`center`}
                                color={`white`}
                                my={1}
                            >
                                <Typography
                                    sx={{
                                        width: `100%`,
                                        py: 0.5,
                                        textAlign: `right`,
                                        cursor: `pointer`,
                                    }}
                                    onClick={() => setIsIndividual(true)}
                                >
                                    Fizičko lice
                                </Typography>
                                <Switch
                                    checked={!isIndividual}
                                    onChange={(e) =>
                                        setIsIndividual(!e.target.checked)
                                    }
                                    color={userTypeModeColor}
                                />
                                <Typography
                                    sx={{
                                        width: `100%`,
                                        py: 0.5,
                                        textAlign: `left`,
                                        cursor: `pointer`,
                                    }}
                                    onClick={() => setIsIndividual(false)}
                                >
                                    Pravno lice
                                </Typography>
                            </Stack>
                        </Paper>
                        {isIndividual ? (
                            <FormValidationInput
                                data={VALIDATION_FIELDS.NICKNAME}
                                disabled={isSubmitting}
                                required
                                InputProps={{
                                    sx: {
                                        backgroundColor:
                                            userTypeDifferenceInputBackgroundColor,
                                    },
                                }}
                            />
                        ) : (
                            <>
                                <FormValidationInput
                                    data={VALIDATION_FIELDS.COMPANY}
                                    disabled={isSubmitting}
                                    required
                                    InputProps={{
                                        sx: {
                                            backgroundColor:
                                                userTypeDifferenceInputBackgroundColor,
                                        },
                                    }}
                                />
                                <FormValidationInput
                                    data={VALIDATION_FIELDS.PIB}
                                    disabled={isSubmitting}
                                    type={`number`}
                                    required
                                    InputProps={{
                                        sx: {
                                            backgroundColor:
                                                userTypeDifferenceInputBackgroundColor,
                                        },
                                    }}
                                />
                                <FormValidationInput
                                    data={VALIDATION_FIELDS.MB}
                                    disabled={isSubmitting}
                                    type={`number`}
                                    required
                                    InputProps={{
                                        sx: {
                                            backgroundColor:
                                                userTypeDifferenceInputBackgroundColor,
                                        },
                                    }}
                                />
                            </>
                        )}
                        <FormValidationInput
                            data={VALIDATION_FIELDS.USERNAME}
                            disabled={isSubmitting}
                            required
                        />
                        <FormValidationInput
                            data={VALIDATION_FIELDS.PASSWORD}
                            disabled={isSubmitting}
                            type={`password`}
                            required
                        />
                        <FormValidationInput
                            data={VALIDATION_FIELDS.CONFIRM_PASSWORD}
                            disabled={isSubmitting}
                            type={`password`}
                            required
                        />
                        <FormValidationDatePicker
                            data={VALIDATION_FIELDS.DATE_OF_BIRTH}
                            label={`Datum rođenja`}
                            disabled={isSubmitting}
                            disableFuture
                            required
                        />
                        <FormValidationInput
                            data={VALIDATION_FIELDS.MOBILE}
                            disabled={isSubmitting}
                            type={`number`}
                            required
                        />
                        <FormValidationInput
                            data={VALIDATION_FIELDS.ADDRESS}
                            disabled={isSubmitting}
                            required
                        />
                        {!cities || cities.length == 0 ? (
                            <CircularProgress />
                        ) : (
                            <FormValidationAutocomplete
                                data={VALIDATION_FIELDS.CITY}
                                options={cities}
                                label={`Mesto stanovanja`}
                                disabled={isSubmitting}
                            />
                        )}
                        {!stores || stores.length == 0 ? (
                            <CircularProgress />
                        ) : (
                            <FormValidationAutocomplete
                                data={VALIDATION_FIELDS.FAVORITE_STORE}
                                options={stores}
                                label={`Omiljena radnja`}
                                disabled={isSubmitting}
                            />
                        )}
                        <FormValidationInput
                            data={VALIDATION_FIELDS.MAIL}
                            disabled={isSubmitting}
                            type={`email`}
                            required
                        />
                        {!isValid && (
                            <Typography
                                color={mainTheme.palette.error.light}
                                sx={{ my: 2, textAlign: `center` }}
                            >
                                Morate ispravno popuniti sva polja!
                            </Typography>
                        )}
                        <Button
                            disabled={!isValid || isSubmitting}
                            variant={`contained`}
                            type={`submit`}
                        >
                            Podnesi zahtev za registraciju
                            {isSubmitting && (
                                <CircularProgress size={`2em`} sx={{ px: 2 }} />
                            )}
                        </Button>
                    </Stack>
                </FormProvider>
            </Stack>
        </CenteredContentWrapper>
    )
}

export default Registrovanje
