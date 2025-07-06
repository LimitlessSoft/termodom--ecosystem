import {
    Alert,
    Button,
    CircularProgress,
    Grid,
    Paper,
    Stack,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { useUser } from '@/app/hooks'
import { handleApiError, webApi } from '@/api/webApi'
import { FormProvider, useForm, useWatch } from 'react-hook-form'
import {
    FormValidationInput,
    FormValidationSelect,
} from '@/widgets/FormValidation'
import { ORDER_CONSTANTS, orderConclusionFormValidator } from '@/widgets/Order'
import { isDeliveryPickupPlace } from '../../../utils/storeUtils'
import { isWireTransferPaymentType } from '../../../utils/paymentTypeUtils'

const textFieldVariant = 'filled'

const OrderConclusion = (props) => {
    const user = useUser()
    const [stores, setStores] = useState(null)
    const [paymentTypes, setPaymentTypes] = useState(undefined)
    const [isLoadingData, setIsLoadingData] = useState(false)

    const { VALIDATION_FIELDS } = ORDER_CONSTANTS

    const defaultFormValues = {
        [VALIDATION_FIELDS.PICKUP_PLACE.FIELD]: props.favoriteStoreId,
        [VALIDATION_FIELDS.DELIVERY_ADDRESS.FIELD]: '',
        [VALIDATION_FIELDS.PAYMENT_TYPE.FIELD]: props.paymentTypeId,
        [VALIDATION_FIELDS.FULL_NAME.FIELD]: '',
        [VALIDATION_FIELDS.COMPANY.FIELD]: '',
        [VALIDATION_FIELDS.MOBILE.FIELD]: '',
        [VALIDATION_FIELDS.NOTE.FIELD]: '',
    }

    const methods = useForm({
        resolver: orderConclusionFormValidator(user.isLogged),
        mode: 'onChange',
        defaultValues: defaultFormValues,
    })

    const {
        handleSubmit,
        control,
        formState: { isValid, isSubmitting },
        reset,
        trigger,
    } = methods

    useEffect(() => {
        Promise.all([
            webApi.get('/stores?sortColumn=Name'),
            webApi.get('/payment-types'),
        ])
            .then(([stores, paymentTypes]) => {
                setStores(stores.data)
                setPaymentTypes(paymentTypes.data)
            })
            .catch((err) => handleApiError(err))

        trigger()
    }, [])

    const [storeId, paymentTypeId] = useWatch({
        control,
        name: [
            VALIDATION_FIELDS.PICKUP_PLACE.FIELD,
            VALIDATION_FIELDS.PAYMENT_TYPE.FIELD,
        ],
    })

    const isDeliveryPickupPlaceSelected = isDeliveryPickupPlace(storeId)
    const isWireTransferPaymentTypeSelected =
        isWireTransferPaymentType(paymentTypeId)

    const handleSubmitOrderConclusion = (data) => {
        let payload = { ...data }

        if (user.isLogged) {
            delete payload[VALIDATION_FIELDS.MOBILE.FIELD]
            delete payload[VALIDATION_FIELDS.FULL_NAME.FIELD]
        }

        if (isWireTransferPaymentTypeSelected) {
            payload.note += `${payload.note ? ' ' : ''}PIB/MB: ${data[VALIDATION_FIELDS.COMPANY.FIELD]}`
        }

        delete payload[VALIDATION_FIELDS.COMPANY.FIELD]

        if (!isDeliveryPickupPlaceSelected) {
            delete payload[VALIDATION_FIELDS.DELIVERY_ADDRESS.FIELD]
        }

        props.onProcessStart?.()
        setIsLoadingData(true)
        webApi
            .post('/checkout', {
                ...payload,
                oneTimeHash: props.oneTimeHash,
            })
            .then(() => {
                props.onSuccess?.()
                toast.success(`Uspešno ste zaključili porudžbinu!`)
                reset()
            })
            .catch((err) => {
                props.onFail?.()
                setIsLoadingData(false)
                handleApiError(err)
            })
            .finally(() => {
                props.onProcessEnd?.()
            })
    }

    const disabledForm = isLoadingData || isSubmitting

    return !user || user.isLoading ? (
        <CircularProgress />
    ) : (
        <Stack
            alignItems={`center`}
            width={`100%`}
            direction={`column`}
            spacing={2}
        >
            <Paper>
                <Alert
                    severity={`warning`}
                    color={`info`}
                    variant={`filled`}
                    sx={{
                        textAlign: 'center',
                        fontWeight: 'bold',

                        maxWidth: 350,
                    }}
                >
                    Popunite sva polja
                </Alert>
            </Paper>
            <Paper>
                <Alert
                    variant={`filled`}
                    sx={{
                        backgroundColor: '#3498db',
                        maxWidth: 350,
                        alignItems: `center`,
                    }}
                >
                    Vršimo prevoz robe na teritoriji cele Srbije uz simboličnu
                    naknadu!
                </Alert>
            </Paper>
            <Typography
                variant={`caption`}
                sx={{
                    fontWeight: `bold`,
                    maxWidth: 300,
                    textAlign: `center`,
                    my: 5,
                    color: '#555',
                }}
            >
                Robu možete preuzeti i u našim maloprodajnim objektima izmenom
                mesta preuzimanja.
            </Typography>
            <FormProvider {...methods}>
                <Stack
                    sx={{ width: '100%', gap: 2, alignItems: 'center' }}
                    component={`form`}
                    onSubmit={handleSubmit(handleSubmitOrderConclusion)}
                >
                    <Grid
                        sx={{
                            display: 'grid',
                            gridTemplateColumns: {
                                sm: 'repeat(2, 1fr)',
                            },
                            width: '100%',
                            gap: 2,
                        }}
                    >
                        {stores == null ? (
                            <CircularProgress />
                        ) : (
                            <FormValidationSelect
                                id="pickup-place"
                                data={VALIDATION_FIELDS.PICKUP_PLACE}
                                options={stores}
                                helperText="Izaberite mesto preuzimanja"
                                disabled={disabledForm}
                                required
                            />
                        )}
                        {isDeliveryPickupPlaceSelected && (
                            <FormValidationInput
                                id="delivery-address"
                                data={VALIDATION_FIELDS.DELIVERY_ADDRESS}
                                variant={textFieldVariant}
                                disabled={disabledForm}
                                required
                            />
                        )}
                        {paymentTypes == null ? (
                            <CircularProgress />
                        ) : (
                            <FormValidationSelect
                                id="payment-type"
                                data={VALIDATION_FIELDS.PAYMENT_TYPE}
                                options={paymentTypes}
                                helperText="Izaberite način plaćanja"
                                disabled={disabledForm}
                                required
                            />
                        )}
                        {!user.isLogged && (
                            <FormValidationInput
                                id="full-name"
                                data={VALIDATION_FIELDS.FULL_NAME}
                                disabled={disabledForm}
                                variant={textFieldVariant}
                                required
                            />
                        )}
                        {isWireTransferPaymentTypeSelected && (
                            <FormValidationInput
                                id="company"
                                data={VALIDATION_FIELDS.COMPANY}
                                disabled={disabledForm}
                                variant={textFieldVariant}
                                type={`number`}
                                required
                            />
                        )}
                        {!user.isLogged && (
                            <FormValidationInput
                                id="mobile"
                                data={VALIDATION_FIELDS.MOBILE}
                                disabled={disabledForm}
                                variant={textFieldVariant}
                                type={`number`}
                                required
                            />
                        )}
                        <FormValidationInput
                            id="note"
                            data={VALIDATION_FIELDS.NOTE}
                            variant={textFieldVariant}
                            disabled={disabledForm}
                        />
                    </Grid>
                    <Typography textAlign={`center`}>
                        Cene iz ove porudžbine važe 1 dan/a od dana
                        zaključivanja!
                    </Typography>
                    <Button
                        id="conclude-order__button"
                        sx={{
                            position: {
                                xs: `sticky`,
                                sm: 'unset',
                            },
                            bottom: {
                                xs: 10,
                                sm: 'unset',
                            },
                            width: {
                                xs: '100%',
                                sm: 'max-content',
                            },
                            boxShadow: {
                                xs: 8,
                                sm: 'none',
                            },
                            border: {
                                xs: '1px solid gray',
                                sm: 'none',
                            },
                            zIndex: 1000,
                        }}
                        color={`success`}
                        disabled={!isValid || disabledForm}
                        startIcon={
                            isSubmitting ? (
                                <CircularProgress size={`1em`} />
                            ) : null
                        }
                        type={`submit`}
                        variant={`contained`}
                    >
                        Zaključi porudžbinu
                    </Button>
                </Stack>
            </FormProvider>
        </Stack>
    )
}

export default OrderConclusion
