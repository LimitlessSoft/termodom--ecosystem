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
    PARTNERI_NEW,
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

    const { VALIDATION_FIELDS } = PARTNERI_NEW

    const {
        handleSubmit,
        control,
        setValue,
        formState: { errors, isValid },
        reset,
        trigger,
    } = useForm({
        resolver: yupResolver(PartneriNewDialogValidation),
        mode: 'onChange',
        defaultValues: {
            [VALIDATION_FIELDS.NAME.FIELD]: '',
            [VALIDATION_FIELDS.ADDRESS.FIELD]: '',
            [VALIDATION_FIELDS.POSTAL_CODE.FIELD]: '',
            [VALIDATION_FIELDS.PLACE.FIELD]: '',
            [VALIDATION_FIELDS.EMAIL.FIELD]: '',
            [VALIDATION_FIELDS.CONTACT.FIELD]: '',
            [VALIDATION_FIELDS.MB.FIELD]: '',
            [VALIDATION_FIELDS.PIB.FIELD]: '',
            [VALIDATION_FIELDS.MOBILE.FIELD]: '',
            [VALIDATION_FIELDS.IN_PDV_SYSTEM.FIELD]: false,
            [VALIDATION_FIELDS.CATEGORY.FIELD]: 0,
            [VALIDATION_FIELDS.CITY.FIELD]: '',
        },
    })

    useEffect(() => {
        officeApi
            .get(ENDPOINTS_CONSTANTS.PARTNERS.GET_MESTA)
            .then((response) => {
                setMestaPayload(response.data)

                if (response.data.length > 0)
                    setValue(
                        VALIDATION_FIELDS.CITY.FIELD,
                        response.data[0].mestoId
                    )
            })
            .catch(handleApiError)
    }, [])

    useEffect(() => {
        officeApi
            .get(ENDPOINTS_CONSTANTS.PARTNERS.GET_KATEGORIJE)
            .then((response) => {
                setKategorijePayload(response.data)
            })
            .catch(handleApiError)
    }, [])

    useEffect(() => {
        if (!isOpen) {
            reset()
            setGroupsChecked(0)
        }
    }, [isOpen, reset])

    const handleSubmitAddingNewPartner = (data) => {
        setIsPosting(true)
        officeApi
            .post(ENDPOINTS_CONSTANTS.PARTNERS.POST, {
                ...data,
                ZapId: 0,
                RefId: 0,
            })
            .then(() => {
                toast.success('Partner uspesno kreiran')
            })
            .catch(handleApiError)
            .finally(() => {
                onClose()
                setIsPosting(false)
            })
    }

    const selectedMinGroups = groupsChecked >= PARTNERI_NEW_MIN_GROUPS_CHECKED

    return (
        <Dialog open={isOpen} onClose={onClose}>
            <form onSubmit={handleSubmit(handleSubmitAddingNewPartner)}>
                <Grid container gap={2} p={2} direction={`column`}>
                    <Grid item>
                        <Typography>Kreiraj novog partnera</Typography>
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.NAME.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.NAME.LABEL}
                                    error={
                                        !!errors[VALIDATION_FIELDS.NAME.FIELD]
                                    }
                                    helperText={
                                        errors[VALIDATION_FIELDS.NAME.FIELD]
                                            ? errors[
                                                  VALIDATION_FIELDS.NAME.FIELD
                                              ].message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger(VALIDATION_FIELDS.NAME.FIELD)
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.ADDRESS.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.ADDRESS.LABEL}
                                    error={
                                        !!errors[
                                            VALIDATION_FIELDS.ADDRESS.FIELD
                                        ]
                                    }
                                    helperText={
                                        errors[VALIDATION_FIELDS.ADDRESS.FIELD]
                                            ? errors[
                                                  VALIDATION_FIELDS.ADDRESS
                                                      .FIELD
                                              ].message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger(VALIDATION_FIELDS.ADDRESS.FIELD)
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.POSTAL_CODE.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.POSTAL_CODE.LABEL}
                                    error={
                                        !!errors[
                                            VALIDATION_FIELDS.POSTAL_CODE.FIELD
                                        ]
                                    }
                                    helperText={
                                        errors[
                                            VALIDATION_FIELDS.POSTAL_CODE.FIELD
                                        ]
                                            ? errors[
                                                  VALIDATION_FIELDS.POSTAL_CODE
                                                      .FIELD
                                              ].message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger(
                                            VALIDATION_FIELDS.POSTAL_CODE.FIELD
                                        )
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.CITY.FIELD}
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
                                            value={mestaPayload[0]}
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
                                                )
                                                trigger(
                                                    VALIDATION_FIELDS.CITY.FIELD
                                                )
                                            }}
                                            renderInput={(params) => (
                                                <PartnerNewDialogTextFieldStyled
                                                    {...params}
                                                    label={
                                                        VALIDATION_FIELDS.CITY
                                                            .LABEL
                                                    }
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
                            name={VALIDATION_FIELDS.PLACE.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.PLACE.LABEL}
                                    error={
                                        !!errors[VALIDATION_FIELDS.PLACE.FIELD]
                                    }
                                    helperText={
                                        errors[VALIDATION_FIELDS.PLACE.FIELD]
                                            ? errors[
                                                  VALIDATION_FIELDS.PLACE.FIELD
                                              ].message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger(VALIDATION_FIELDS.PLACE.FIELD)
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.EMAIL.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.EMAIL.LABEL}
                                    error={
                                        !!errors[VALIDATION_FIELDS.EMAIL.FIELD]
                                    }
                                    helperText={
                                        errors[VALIDATION_FIELDS.EMAIL.FIELD]
                                            ? errors[
                                                  VALIDATION_FIELDS.EMAIL.FIELD
                                              ].message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger(VALIDATION_FIELDS.EMAIL.FIELD)
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.CONTACT.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.CONTACT.LABEL}
                                    error={
                                        !!errors[
                                            VALIDATION_FIELDS.CONTACT.FIELD
                                        ]
                                    }
                                    helperText={
                                        errors[VALIDATION_FIELDS.CONTACT.FIELD]
                                            ? errors[
                                                  VALIDATION_FIELDS.CONTACT
                                                      .FIELD
                                              ]?.message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger([
                                            VALIDATION_FIELDS.CONTACT.FIELD,
                                        ])
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    {kategorijePayload?.length === 0 ? (
                        <CircularProgress />
                    ) : (
                        <Grid item textAlign="center">
                            <Controller
                                name={VALIDATION_FIELDS.CATEGORY.FIELD}
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
                                                field.onChange(val)
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
                                                color:
                                                    field.value > 0 &&
                                                    selectedMinGroups
                                                        ? 'green'
                                                        : 'red',
                                            }}
                                        >
                                            Izabrano grupa: {groupsChecked} /{' '}
                                            {PARTNERI_NEW_MIN_GROUPS_CHECKED}
                                        </Typography>
                                    </>
                                )}
                            />
                            {errors[VALIDATION_FIELDS.CATEGORY.FIELD] && (
                                <Typography color="red">
                                    {
                                        errors[VALIDATION_FIELDS.CATEGORY.FIELD]
                                            .message
                                    }
                                </Typography>
                            )}
                        </Grid>
                    )}
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.MB.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.MB.LABEL}
                                    error={!!errors[VALIDATION_FIELDS.MB.FIELD]}
                                    helperText={
                                        errors[VALIDATION_FIELDS.MB.FIELD]
                                            ? errors[VALIDATION_FIELDS.MB.FIELD]
                                                  .message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger(VALIDATION_FIELDS.MB.FIELD)
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.PIB.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.PIB.LABEL}
                                    error={
                                        !!errors[VALIDATION_FIELDS.PIB.FIELD]
                                    }
                                    helperText={
                                        errors[VALIDATION_FIELDS.PIB.FIELD]
                                            ? errors[
                                                  VALIDATION_FIELDS.PIB.FIELD
                                              ]?.message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger([VALIDATION_FIELDS.PIB.FIELD])
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.IN_PDV_SYSTEM.FIELD}
                            control={control}
                            render={({ field }) => (
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            {...field}
                                            checked={field.value}
                                            onChange={(e) => {
                                                field.onChange(e.target.checked)
                                                trigger(
                                                    VALIDATION_FIELDS
                                                        .IN_PDV_SYSTEM.FIELD
                                                )
                                            }}
                                            disabled={isPosting}
                                        />
                                    }
                                    label={
                                        VALIDATION_FIELDS.IN_PDV_SYSTEM.LABEL
                                    }
                                />
                            )}
                        />
                    </Grid>
                    <Grid item>
                        <Controller
                            name={VALIDATION_FIELDS.MOBILE.FIELD}
                            control={control}
                            render={({ field }) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...field}
                                    label={VALIDATION_FIELDS.MOBILE.LABEL}
                                    error={
                                        !!errors[VALIDATION_FIELDS.MOBILE.FIELD]
                                    }
                                    helperText={
                                        errors[VALIDATION_FIELDS.MOBILE.FIELD]
                                            ? errors[
                                                  VALIDATION_FIELDS.MOBILE.FIELD
                                              ]?.message
                                            : ''
                                    }
                                    disabled={isPosting}
                                    onChange={(e) => {
                                        field.onChange(e)
                                        trigger([
                                            VALIDATION_FIELDS.MOBILE.FIELD,
                                        ])
                                    }}
                                />
                            )}
                        />
                    </Grid>
                    <Grid item textAlign="center">
                        <Button
                            variant="contained"
                            type="submit"
                            disabled={
                                isPosting || !isValid || !selectedMinGroups
                            }
                        >
                            Kreiraj partnera
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
                </Grid>
            </form>
        </Dialog>
    )
}
