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
import { toast } from 'react-toastify'
import { useState, useEffect } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { formatNumber } from '../../../../helpers/numberHelpers'

export const AzuriranjeCenaUslovFormiranjaCell = (props) => {
    const isInitialReferentnaCena = props.data.uslovFormiranjaWebCeneType === 2
    const [isUpdating, setIsUpdating] = useState(false)
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [data, setData] = useState(props.data)
    const [request, setRequest] = useState({
        id: props.data.uslovFormiranjaWebCeneId,
        webProductId: props.data.id,
        type: props.data.uslovFormiranjaWebCeneType,
        modifikator: props.data.uslovFormiranjaWebCeneModifikator,
    })

    const uslovLabel = (modifikator) => {
        switch (data.uslovFormiranjaWebCeneType) {
            case 0:
                return `Nabavna cena + ${modifikator}%`
            case 1:
                return `Prodajna cena - ${modifikator}%`
            case 2:
                return `Cena na upit`
            case 3: {
                if (modifikator == null) return 'Koeficijentalno'
                const mod = Number(modifikator)
                if (isNaN(mod)) return 'Koeficijentalno'
                const koeficijentalnoId = mod % 1000
                const koeficijentalnoValue = (mod - koeficijentalnoId) / 1000
                return `Koeficijentalno (id: ${koeficijentalnoId}, multiplikator: ${formatNumber(
                    koeficijentalnoValue
                )})`
            }
            default:
                return ''
        }
    }

    return (
        <Grid>
            <Dialog
                onClose={() => {
                    setIsDialogOpen(false)
                }}
                open={isDialogOpen}
            >
                <DialogTitle>
                    Uslov formiranja Min Web Osnove - {props.data.naziv}
                </DialogTitle>
                <DialogContent>
                    <TextField
                        id={`uslov-formiranja`}
                        select
                        required
                        defaultValue={props.data.uslovFormiranjaWebCeneType}
                        label={`Uslov formiranja cene`}
                        sx={{ minWidth: 350, my: 1 }}
                        onChange={(e) => {
                            setRequest({
                                ...request,
                                type: Number(e.target.value),
                            })
                        }}
                        helperText={`Izaberite uslov formiranja cene`}
                    >
                        <MenuItem value={0}>Nabavna cena +%</MenuItem>
                        <MenuItem value={1}>Prodajna cena -%</MenuItem>
                        <MenuItem value={2}>Cena na upit</MenuItem>
                        <MenuItem value={3}>Koeficijentalno</MenuItem>
                    </TextField>
                    {request.type != 2 && request.type != 3 && (
                        <TextField
                            type={`text`}
                            defaultValue={
                                props.data.uslovFormiranjaWebCeneModifikator
                            }
                            label={`Modifikator`}
                            onChange={(e) => {
                                setRequest({
                                    ...request,
                                    modifikator: Number(e.target.value),
                                })
                            }}
                            helperText={`Modifikator (možete staviti vrednost u minusu)`}
                        ></TextField>
                    )}
                    {request.type === 3 && (
                        <KoeficijentalnoModifikacija
                            request={request}
                            setRequest={setRequest}
                        />
                    )}
                    <Grid container direction={`column`}>
                        <Typography>
                            Buduca platinum cena: to be implemented
                        </Typography>
                        <Typography>
                            Buduca gold cena: to be implemented
                        </Typography>
                        <Typography>
                            Buduca silver cena: to be implemented
                        </Typography>
                        <Typography>
                            Buduca iron cena: to be implemented
                        </Typography>
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            setIsUpdating(true)

                            officeApi
                                .put(
                                    `/web-azuriraj-cene-uslovi-formiranja-min-web-osnova`,
                                    request
                                )
                                .then(() => {
                                    toast.success(
                                        `Uspešno ažuriran uslov formiranja cene!`
                                    )
                                    setData({
                                        ...data,
                                        uslovFormiranjaWebCeneModifikator:
                                            request.modifikator,
                                        uslovFormiranjaWebCeneType:
                                            request.type,
                                    })
                                    props.onSuccessUpdate()
                                    setIsDialogOpen(false)
                                })
                                .catch((err) => {
                                    props.onErrorUpdate()
                                    handleApiError(err)
                                })
                                .finally(() => {
                                    setIsUpdating(false)
                                })
                        }}
                    >
                        Potvrdi
                    </Button>
                    <Button
                        onClick={() => {
                            setIsDialogOpen(false)
                        }}
                    >
                        Odustani
                    </Button>
                </DialogActions>
            </Dialog>
            <Button
                disabled={isUpdating || props.disabled}
                startIcon={
                    isUpdating ? <CircularProgress size={`1em`} /> : null
                }
                color={`info`}
                variant={`contained`}
                onClick={() => {
                    setIsDialogOpen(true)
                }}
            >
                {uslovLabel(data.uslovFormiranjaWebCeneModifikator)}
            </Button>
        </Grid>
    )
}

const KoeficijentalnoModifikacija = ({ request, setRequest }) => {
    const [options, setOptions] = useState([])
    const [isLoading, setIsLoading] = useState(false)
    const [selectedId, setSelectedId] = useState(null)
    const [value, setValue] = useState('')

    // Decode existing modifikator if present: modifikator = value * 1000 + id
    useEffect(() => {
        if (request?.modifikator && request.type === 3) {
            const mod = Number(request.modifikator)
            if (!isNaN(mod)) {
                const id = mod % 1000
                const val = (mod - id) / 1000
                setSelectedId(id)
                setValue(val)
            }
        }
    }, [request])

    useEffect(() => {
        setIsLoading(true)
        officeApi
            .get('/web-komercijalno-price-koeficijenti')
            .then((res) => {
                // Expected shape: { items: [{ id, naziv, vrednost }] }
                setOptions(res.data?.items || [])
            })
            .catch((err) => {
                handleApiError(err)
            })
            .finally(() => {
                setIsLoading(false)
            })
    }, [])

    const updateModifikator = (newId, newValue) => {
        const id = Number(newId)
        const val = Number(newValue)
        if (!id || isNaN(id) || isNaN(val)) {
            return
        }
        const modifikator = val * 1000 + id
        setRequest((prev) => ({
            ...prev,
            modifikator,
        }))
    }

    const handleSelectChange = (e) => {
        const newId = Number(e.target.value)
        setSelectedId(newId)
        updateModifikator(newId, value)
    }

    const handleValueChange = (e) => {
        const newValue = e.target.value
        setValue(newValue)
        updateModifikator(selectedId, newValue)
    }

    return (
        <Grid container direction={`column`} sx={{ my: 1 }}>
            <TextField
                select
                label={`Koeficijent`}
                value={selectedId ?? ''}
                onChange={handleSelectChange}
                sx={{ mb: 1, minWidth: 250 }}
                disabled={isLoading}
                helperText={
                    isLoading
                        ? 'Učitavanje koeficijenata...'
                        : 'Izaberite koeficijent'
                }
            >
                {options.map((opt) => (
                    <MenuItem key={opt.id} value={opt.id}>
                        {opt.naziv} ({opt.vrednost})
                    </MenuItem>
                ))}
            </TextField>
            <TextField
                type="number"
                label={`Multiplikator`}
                value={value}
                onChange={handleValueChange}
                helperText={`Unesite multiplikator koja će se kombinovati sa izabranim koeficijentom`}
            />
        </Grid>
    )
}
