import {
    Alert,
    Button,
    CircularProgress,
    Grid,
    MenuItem,
    Paper,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { useUser } from '@/app/hooks'
import { handleApiError, webApi } from '@/api/webApi'
import { blockNonDigitKeys } from '@/helpers/inputHelpers'

const textFieldVariant = 'filled'

const OrderConclusion = (props) => {
    const user = useUser()
    const [stores, setStores] = useState(null)
    const [paymentTypes, setPaymentTypes] = useState(undefined)
    const [pibmb, setPibmb] = useState('')
    const [request, setRequest] = useState({
        storeId: props.favoriteStoreId,
        name: undefined,
        mobile: undefined,
        note: undefined,
        paymentTypeId: props.paymentTypeId,
        oneTimeHash: props.oneTimeHash,
    })

    const [isInProgress, setIsInProgress] = useState(false)

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
    }, [])

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
            <Grid
                spacing={2}
                sx={{
                    display: 'grid',
                    gridTemplateColumns: {
                        sm: 'repeat(2, 1fr)',
                    },
                    width: '100%',
                    gap: 2,
                }}
            >
                {!stores ? (
                    <CircularProgress />
                ) : (
                    <TextField
                        disabled={isInProgress}
                        id="mesto-preuzimanja"
                        select
                        required
                        fullWidth
                        defaultValue={props.favoriteStoreId}
                        label="Mesto preuzimanja"
                        onChange={(e) => {
                            setRequest((prev) => {
                                return {
                                    ...prev,
                                    storeId: Number.parseInt(e.target.value),
                                }
                            })
                        }}
                        helperText="Izaberite mesto preuzimanja"
                    >
                        {stores.map((store) => {
                            return (
                                <MenuItem key={store.id} value={store.id}>
                                    {store.name}
                                </MenuItem>
                            )
                        })}
                    </TextField>
                )}
                {request.storeId === -5 && (
                    <TextField
                        required
                        disabled={isInProgress}
                        id="adresa-dostave"
                        label="Adresa dostave"
                        onChange={(e) => {
                            setRequest((prev) => {
                                return {
                                    ...prev,
                                    deliveryAddress: e.target.value,
                                }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                )}
                {paymentTypes == undefined || paymentTypes == null ? (
                    <CircularProgress />
                ) : (
                    <TextField
                        disabled={isInProgress}
                        id="nacini-placanja"
                        select
                        required
                        defaultValue={props.paymentTypeId}
                        label="Način plaćanja"
                        onChange={(e) => {
                            setRequest((prev) => {
                                return {
                                    ...prev,
                                    paymentTypeId: Number.parseInt(
                                        e.target.value
                                    ),
                                }
                            })
                        }}
                        helperText="Izaberite način plaćanja"
                    >
                        {paymentTypes.map((pt) => {
                            return (
                                <MenuItem key={pt.id} value={pt.id}>
                                    {pt.name}
                                </MenuItem>
                            )
                        })}
                    </TextField>
                )}
                {request.paymentTypeId === 6 && (
                    <TextField
                        required
                        disabled={isInProgress}
                        id="pib-mb"
                        label="PIB/MB"
                        type="number"
                        onChange={(e) => {
                            setPibmb(e.target.value)
                        }}
                        onKeyDown={blockNonDigitKeys}
                        variant={textFieldVariant}
                    />
                )}
                {user.isLogged ? null : (
                    <TextField
                        required
                        disabled={isInProgress}
                        id="ime-i-prezime"
                        label="Ime i prezime"
                        onChange={(e) => {
                            setRequest((prev) => {
                                return { ...prev, name: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                )}

                {user.isLogged ? null : (
                    <TextField
                        required
                        disabled={isInProgress}
                        id="mobilni"
                        label="Mobilni telefon"
                        type="number"
                        onChange={(e) => {
                            setRequest((prev) => {
                                return { ...prev, mobile: e.target.value }
                            })
                        }}
                        variant={textFieldVariant}
                    />
                )}
                <TextField
                    disabled={isInProgress}
                    id="napomena"
                    label="Napomena"
                    onChange={(e) => {
                        setRequest((prev) => {
                            return { ...prev, note: e.target.value }
                        })
                    }}
                    variant={textFieldVariant}
                />
            </Grid>
            <Typography textAlign={`center`}>
                Cene iz ove porudžbine važe 1 dan/a od dana zaključivanja!
            </Typography>
            <Button
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
                disabled={isInProgress}
                startIcon={
                    isInProgress ? <CircularProgress size={`1em`} /> : null
                }
                variant={`contained`}
                onClick={() => {
                    const req = request
                    if (req.storeId === -5 && !req.deliveryAddress) {
                        toast.error(`Morate popuniti adresu dostave!`)
                        return
                    }

                    if (req.paymentTypeId === 6) {
                        if (!pibmb) {
                            toast.error(`Morate popuniti PIB/MB!`)
                            return
                        }
                        req.note += ` PIB/MB: ${pibmb}`
                    }

                    props.onProcessStart?.()
                    setIsInProgress(true)
                    webApi
                        .post('/checkout', req)
                        .then((res) => {
                            props.onSuccess?.()
                            toast.success(`Uspešno ste zaključili porudžbinu!`)
                        })
                        .catch((err) => {
                            setIsInProgress(false)
                            props.onFail?.()
                            handleApiError(err)
                        })
                        .finally(() => {
                            props.onProcessEnd?.()
                        })
                }}
            >
                Zaključi porudžbinu
            </Button>
        </Stack>
    )
}

export default OrderConclusion
