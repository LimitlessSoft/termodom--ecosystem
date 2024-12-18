import {
    Autocomplete,
    Button,
    Checkbox,
    CircularProgress,
    Dialog,
    FormControlLabel,
    Grid,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { PartnerNewDialogTextFieldStyled } from '@/widgets/Partneri/PartneriList/styled/PartnerNewDialogTextFieldStyled'
import {
    PARTNERI_NEW_KATEGORIJE_PAYLOAD_DEFAULT_VALUE,
    PARTNERI_NEW_MESTA_PAYLOAD_DEFAULT_VALUE,
    PARTNERI_NEW_MIN_GROUPS_CHECKED,
} from '@/widgets/Partneri/PartneriList/constants'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '@/constants'
import { PartneriPickGroups } from '@/widgets/Partneri/PartneriList/ui/PartneriPickGroups'
import { toast } from 'react-toastify'
import { useForm, Controller } from 'react-hook-form'
import { yupResolver } from '@hookform/resolvers/yup'
import { PartneriNewDialogValidation } from '../validations'

export const PartneriNewDialog = ({ isOpen, onClose }) => {
    const [isPosting, setIsPosting] = useState(false)

    const [mestaPayload, setMestaPayload] = useState(
        PARTNERI_NEW_MESTA_PAYLOAD_DEFAULT_VALUE
    )
    const [kategorijePayload, setKategorijePayload] = useState(
        PARTNERI_NEW_KATEGORIJE_PAYLOAD_DEFAULT_VALUE
    )

    const [isPickGroupsOpen, setIsPickGroupsOpen] = useState(false)
    const [groupsChecked, setGroupsChecked] = useState(0)

    const [isKategorijaValid, setIsKategorijaValid] = useState(false)

    const {
        handleSubmit,
        control,
        formState: { errors, isValid },
        setValue,
        reset,
        trigger,
    } = useForm({
        resolver: yupResolver(PartneriNewDialogValidation),
        mode: 'onChange',
        defaultValues: {
            Naziv: '',
            Adresa: '',
            'Postanski broj': '',
            Mesto: '',
            Email: '',
            Kontakt: '',
            'Maticni broj': '',
            PIB: '',
            Mobilni: '',
            'U PDV Sistemu': false,
        },
    })

    useEffect(() => {
        if (!isOpen) {
            reset()
            setGroupsChecked(0)
        }
    }, [isOpen, reset])

    const handleSubmitAddingNewPartner = () => {
        setIsPosting(true)
        officeApi
            .post(ENDPOINTS_CONSTANTS.PARTNERS.POST, rBody)
            .then((e) => {
                toast.success('Partner uspesno kreiran')
            })
            .catch(handleApiError)
            .finally(() => {
                setIsPosting(false)
            })
    }

    const [rBody, setRBody] = useState({
        Naziv: '',
        Adresa: '',
        Posta: '',
        Mesto: '',
        Email: '',
        Kontakt: '',
        Kategorija: 0,
        Mbroj: '',
        MestoId: '',
        ZapId: 0,
        RefId: 0,
        Pib: '',
        UPdvSistemu: false,
        Mobilni: '',
    })

    useEffect(() => {
        const fetchMesta = async () => {
            return await officeApi
                .get(ENDPOINTS_CONSTANTS.PARTNERS.GET_MESTA)
                .catch(handleApiError)
        }

        fetchMesta().then((response: any) => {
            setMestaPayload(response.data)
        })
    }, [])

    useEffect(() => {
        const fetchKategorije = async () => {
            return await officeApi
                .get(ENDPOINTS_CONSTANTS.PARTNERS.GET_KATEGORIJE)
                .catch(handleApiError)
        }

        fetchKategorije().then((response: any) => {
            setKategorijePayload(response.data)
        })
    }, [])

    useEffect(() => {
        setIsKategorijaValid(
            rBody.Kategorija > 0 &&
                groupsChecked >= PARTNERI_NEW_MIN_GROUPS_CHECKED
        )
    }, [rBody.Kategorija, groupsChecked])
    return (
        <Dialog open={isOpen} onClose={onClose}>
            <form onSubmit={() => handleSubmit(handleSubmitAddingNewPartner)}>
                <Grid container gap={2} p={2} direction={`column`}>
                    <Grid item>
                        <Typography>Kreiraj novog partnera</Typography>
                    </Grid>
                    <Grid item>
                        <Controller
                            name="Naziv"
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label="Naziv"
                                    error={!!errors.Naziv}
                                    helperText={
                                        errors.Naziv ? errors.Naziv.message : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger('Naziv')
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name="Adresa"
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label="Adresa"
                                    error={!!errors.Adresa}
                                    helperText={
                                        errors.Adresa
                                            ? errors.Adresa.message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger('Adresa')
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name="Postanski broj"
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label="Postanski broj"
                                    error={!!errors['Postanski broj']}
                                    helperText={
                                        errors['Postanski broj']
                                            ? errors['Postanski broj'].message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger('Postanski broj')
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name="Grad"
                            control={control}
                            render={({ field, fieldState }) => (
                                <>
                                    {mestaPayload === undefined && (
                                        <CircularProgress />
                                    )}
                                    {mestaPayload !== undefined && (
                                        <Autocomplete
                                            {...field}
                                            options={mestaPayload}
                                            getOptionLabel={(option) =>
                                                `${option.naziv}`
                                            }
                                            defaultValue={mestaPayload[0]}
                                            isOptionEqualToValue={(
                                                option,
                                                value
                                            ) =>
                                                option.mestoId ===
                                                value?.mestoId
                                            }
                                            disabled={isPosting}
                                            onChange={(event, value) => {
                                                field.onChange(
                                                    value?.mestoId ?? ''
                                                ) // Update the value
                                                trigger('Grad') // Trigger validation
                                            }}
                                            renderInput={(params) => (
                                                <PartnerNewDialogTextFieldStyled
                                                    {...params}
                                                    label="Grad"
                                                    error={!!fieldState.error}
                                                    helperText={
                                                        fieldState.error
                                                            ?.message
                                                    }
                                                />
                                            )}
                                        />
                                    )}
                                </>
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name="Mesto"
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label="Mesto"
                                    error={!!errors.Mesto}
                                    helperText={
                                        errors.Mesto ? errors.Mesto.message : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger('Mesto')
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name="Kontakt"
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label="Kontakt"
                                    error={!!errors.Kontakt}
                                    helperText={
                                        errors.Kontakt
                                            ? errors.Kontakt.message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger('Kontakt')
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    {kategorijePayload === undefined && <CircularProgress />}
                    {kategorijePayload !== undefined && (
                        <Grid item textAlign="center">
                            <Controller
                                name="Kategorija"
                                control={control}
                                render={({ field }) => (
                                    <>
                                        <PartneriPickGroups
                                            onClose={() =>
                                                setIsPickGroupsOpen(false)
                                            }
                                            kategorije={kategorijePayload}
                                            open={isPickGroupsOpen}
                                            onChange={(val, groupsChecked) => {
                                                setGroupsChecked(groupsChecked)
                                                setValue('Kategorija', val)
                                            }}
                                        />
                                        <Button
                                            disabled={isPosting}
                                            variant="contained"
                                            onClick={() =>
                                                setIsPickGroupsOpen(true)
                                            }
                                        >
                                            Izaberi grupe
                                        </Button>
                                        <Typography
                                            sx={{
                                                color: errors.Kategorija
                                                    ? 'red'
                                                    : 'green',
                                            }}
                                        >
                                            Izabrano grupa: {groupsChecked} /{' '}
                                            {PARTNERI_NEW_MIN_GROUPS_CHECKED}
                                        </Typography>
                                    </>
                                )}
                            />
                        </Grid>
                    )}
                    <Grid item>
                        <Controller
                            name="Maticni broj"
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label="Maticni broj"
                                    error={!!errors['Maticni broj']}
                                    helperText={
                                        errors['Maticni broj']
                                            ? errors['Maticni broj'].message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger('Maticni broj')
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name="PIB"
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label="PIB"
                                    error={!!errors.PIB}
                                    helperText={
                                        errors.PIB ? errors.PIB.message : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger('PIB')
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name="U PDV Sistemu"
                            control={control}
                            render={({ field }) => (
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            {...field}
                                            checked={field.value}
                                            onChange={(e) => {
                                                field.onChange(e.target.checked)
                                                trigger('U PDV Sistemu')
                                            }}
                                            disabled={isPosting}
                                        />
                                    }
                                    label="U PDV Sistemu"
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name="Mobilni"
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label="Mobilni"
                                    error={!!errors.Mobilni}
                                    helperText={
                                        errors.Mobilni
                                            ? errors.Mobilni.message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger('Mobilni')
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item textAlign="center">
                        <Button
                            variant="contained"
                            type="submit"
                            disabled={isPosting || !isValid}
                        >
                            Kreiraj partnera{' '}
                            {isPosting && (
                                <CircularProgress
                                    size="2em"
                                    sx={{
                                        px: 2,
                                    }}
                                />
                            )}
                        </Button>
                    </Grid>

                    {/* <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Naziv'}
                        error={!isNazivValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Naziv: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Adresa'}
                        error={!isAdresaValid}
                        helperText={``}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Adresa: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Postanski Broj'}
                        error={!isPostaValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Posta: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    {mestaPayload === undefined && <CircularProgress />}
                    {mestaPayload !== undefined && (
                        <Autocomplete
                            disabled={isPosting}
                            defaultValue={mestaPayload[0]}
                            options={mestaPayload}
                            onChange={(event, value) => {
                                setRBody((prev) => {
                                    return {
                                        ...prev,
                                        MestoId: value?.mestoId ?? '',
                                    }
                                })
                            }}
                            getOptionLabel={(option) => {
                                return `${option.naziv}`
                            }}
                            renderInput={(params) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...params}
                                    label={`Grad`}
                                />
                            )}
                        />
                    )}
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Mesto'}
                        error={!isMestoValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Mesto: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Email'}
                        error={!isEmailValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Email: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Kontakt'}
                        error={!isKontaktValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Kontakt: e.target.value }
                            })
                        }}
                    />
                </Grid>
                {kategorijePayload === undefined && <CircularProgress />}
                {kategorijePayload !== undefined && (
                    <Grid item textAlign={`center`}>
                        <PartneriPickGroups
                            onClose={() => {
                                setIsPickGroupsOpen(false)
                            }}
                            kategorije={kategorijePayload}
                            open={isPickGroupsOpen}
                            onChange={(val, groupsChecked) => {
                                setGroupsChecked(groupsChecked)
                                setRBody((prev) => {
                                    return { ...prev, Kategorija: val }
                                })
                            }}
                        />
                        <Button
                            disabled={isPosting}
                            variant={`contained`}
                            onClick={() => {
                                setIsPickGroupsOpen(true)
                            }}
                        >
                            Izaberi grupe
                        </Button>
                        <Typography
                            sx={{
                                color: isKategorijaValid ? 'green' : 'red',
                            }}
                        >
                            Izabrano grupa: {groupsChecked} /{' '}
                            {PARTNERI_NEW_MIN_GROUPS_CHECKED}
                        </Typography>
                    </Grid>
                )}
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Maticni Broj'}
                        error={!isMbrojValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Mbroj: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Pib'}
                        error={!isPibValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Pib: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Mobilni'}
                        error={!isMobilniValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Mobilni: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item textAlign={`center`}>
                    <Button
                        variant={`contained`}
                        disabled={
                            isPosting ||
                            !isNazivValid ||
                            !isAdresaValid ||
                            !isPostaValid ||
                            !isGradValid ||
                            !isEmailValid ||
                            !isKontaktValid ||
                            !isKategorijaValid ||
                            !isMbrojValid ||
                            !isPibValid ||
                            !isMobilniValid
                        }
                        onClick={() => {
                            setIsPosting(true)
                            officeApi
                                .post(ENDPOINTS_CONSTANTS.PARTNERS.POST, rBody)
                                .then((e) => {
                                    toast.success('Partner uspesno kreiran')
                                })
                                .catch(handleApiError)
                                .finally(() => {
                                    setIsPosting(false)
                                })
                        }}
                    >
                        Kreiraj partnera{' '}
                        {isPosting && (
                            <CircularProgress
                                size={`2em`}
                                sx={{
                                    px: 2,
                                }}
                            />
                        )}
                    </Button>
                </Grid> */}
                </Grid>
            </form>
        </Dialog>
    )
}
