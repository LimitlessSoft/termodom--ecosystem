import {
    Dialog,
    DialogContent,
    DialogTitle,
    Stack,
    Typography,
    TextField,
    IconButton,
    Button,
} from '@mui/material'
import { Add, Delete, Save } from '@mui/icons-material'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '../../../../apis/officeApi'
import { toast } from 'react-toastify'

export const AzurirajCeneUpravljajKoeficijentimaDialog = (props) => {
    const [koeficijenti, setKoeficijenti] = useState([])
    const [loading, setLoading] = useState(false)

    const [noviKoeficijent, setNoviKoeficijent] = useState({
        naziv: '',
        vrednost: '',
    })

    // Load existing coefficients
    useEffect(() => {
        if (!props.isOpen) return

        setLoading(true)
        officeApi
            .get('/web-komercijalno-price-koeficijenti')
            .then((response) => {
                // C# dto: KomercijalnoPriceKoeficijentiDto { List<Item> Items }
                const items = response.data.items ?? response.data.Items ?? []
                setKoeficijenti(
                    items.map((i) => ({
                        id: i.id ?? i.Id,
                        naziv: i.naziv ?? i.Naziv,
                        // keep as-is to avoid losing precision or formatting; parse only when sending to API
                        vrednost: i.vrednost ?? i.Vrednost,
                        isDirty: false,
                        isSaving: false,
                    }))
                )
            })
            .catch(handleApiError)
            .finally(() => setLoading(false))
    }, [props.isOpen])

    const handleAddKoeficijent = () => {
        const trimmedNaziv = noviKoeficijent.naziv.trim()
        const trimmedVrednost = `${noviKoeficijent.vrednost}`.trim()

        if (!trimmedNaziv) {
            toast.warn('Unesite naziv koeficijenta pre čuvanja.', {
                autoClose: 4000,
            })
            return
        }

        if (!trimmedVrednost) {
            toast.warn('Unesite vrednost koeficijenta pre čuvanja.', {
                autoClose: 4000,
            })
            return
        }

        const payload = {
            id: null,
            naziv: trimmedNaziv,
            vrednost: Number(trimmedVrednost),
        }

        officeApi
            .put('/web-komercijalno-price-koeficijenti', payload)
            .then(() => {
                // Ne oslanjamo se na odgovor backend-a; koristimo podatke sa klijenta
                const newItem = {
                    id: Date.now(),
                    naziv: trimmedNaziv,
                    vrednost: trimmedVrednost,
                    isDirty: false,
                    isSaving: false,
                }

                setKoeficijenti((prev) => [...prev, newItem])
                setNoviKoeficijent({ naziv: '', vrednost: '' })
            })
            .catch(handleApiError)
    }

    const handleFieldChange = (id, field, value) => {
        const newValue =
            field === 'vrednost' && value !== '' ? Number(value) : value

        setKoeficijenti((prev) =>
            prev.map((k) =>
                k.id === id
                    ? {
                          ...k,
                          [field]: newValue,
                          isDirty: true,
                      }
                    : k
            )
        )
    }

    const handleSaveKoeficijent = (id) => {
        const item = koeficijenti.find((k) => k.id === id)
        if (!item) return

        // mark row as saving
        setKoeficijenti((prev) =>
            prev.map((k) =>
                k.id === id
                    ? {
                          ...k,
                          isSaving: true,
                      }
                    : k
            )
        )

        const payload = {
            id: item.id,
            naziv: item.naziv,
            vrednost: Number(item.vrednost),
        }

        officeApi
            .put('/web-komercijalno-price-koeficijenti', payload)
            .then(() => {
                setKoeficijenti((prev) =>
                    prev.map((k) =>
                        k.id === id
                            ? {
                                  ...k,
                                  isDirty: false,
                                  isSaving: false,
                              }
                            : k
                    )
                )
            })
            .catch((err) => {
                handleApiError(err)
                setKoeficijenti((prev) =>
                    prev.map((k) =>
                        k.id === id
                            ? {
                                  ...k,
                                  isSaving: false,
                              }
                            : k
                    )
                )
            })
    }

    const handleDeleteKoeficijent = (id) => {
        // API za brisanje još uvek nije implementiran
        toast.info(
            'Brisanje koeficijenata još uvek nije dostupno. Obratite se administratoru ili pokušajte kasnije.',
            {
                autoClose: 5000,
            }
        )

        // Kada implementiraš API za brisanje, ukloni toast iznad
        // i otkomentariši sledeći deo, zajedno sa ažuriranjem lokalnog stanja.
        //
        // setKoeficijenti((prev) => prev.filter((k) => k.id !== id))
        // officeApi
        //     .delete(`/web-komercijalno-price-koeficijenti/${id}`)
        //     .catch(handleApiError)
    }

    const handleClose = () => {
        props.handleClose(false)
    }

    return (
        <Dialog
            open={props.isOpen}
            onClose={handleClose}
            fullWidth
            maxWidth={`md`}
        >
            <DialogTitle>
                <Typography variant={`h6`}>
                    Upravljanje koeficijentima za ažuriranje cena
                </Typography>
            </DialogTitle>
            <DialogContent>
                <Stack spacing={2}>
                    <Typography>
                        Ova funkcionalnost omogućava upravljanje koeficijentima
                        koji se koriste prilikom ažuriranja cena u web
                        prodavnici. Definišite nazive i vrednosti koeficijenata
                        koji će biti dodatno matematički primenjeni uz formulu
                        definisanu za ažuriranje cena proizvoda.
                    </Typography>

                    {loading ? (
                        <Typography>Učitavanje...</Typography>
                    ) : (
                        <Stack spacing={1} mt={2}>
                            {koeficijenti.map((koeficijent) => (
                                <Stack
                                    key={koeficijent.id}
                                    direction={`row`}
                                    spacing={2}
                                    alignItems={`center`}
                                >
                                    <TextField
                                        label={`Naziv`}
                                        variant={`outlined`}
                                        size={`small`}
                                        fullWidth
                                        value={koeficijent.naziv}
                                        disabled={koeficijent.isSaving}
                                        onChange={(e) =>
                                            handleFieldChange(
                                                koeficijent.id,
                                                'naziv',
                                                e.target.value
                                            )
                                        }
                                    />
                                    <TextField
                                        label={`Vrednost`}
                                        variant={`outlined`}
                                        size={`small`}
                                        type={`number`}
                                        sx={{ width: 150 }}
                                        value={koeficijent.vrednost}
                                        disabled={koeficijent.isSaving}
                                        onChange={(e) =>
                                            handleFieldChange(
                                                koeficijent.id,
                                                'vrednost',
                                                e.target.value
                                            )
                                        }
                                    />
                                    <IconButton
                                        color={`primary`}
                                        disabled={
                                            !koeficijent.isDirty ||
                                            koeficijent.isSaving
                                        }
                                        onClick={() =>
                                            handleSaveKoeficijent(
                                                koeficijent.id
                                            )
                                        }
                                    >
                                        <Save />
                                    </IconButton>
                                    <IconButton
                                        color={`error`}
                                        disabled={koeficijent.isSaving}
                                        onClick={() =>
                                            handleDeleteKoeficijent(
                                                koeficijent.id
                                            )
                                        }
                                    >
                                        <Delete />
                                    </IconButton>
                                </Stack>
                            ))}
                        </Stack>
                    )}

                    <Stack
                        direction={`row`}
                        spacing={2}
                        alignItems={`center`}
                        mt={3}
                    >
                        <TextField
                            label={`Naziv novog koeficijenta`}
                            variant={`outlined`}
                            size={`small`}
                            fullWidth
                            value={noviKoeficijent.naziv}
                            onChange={(e) =>
                                setNoviKoeficijent((prev) => ({
                                    ...prev,
                                    naziv: e.target.value,
                                }))
                            }
                        />
                        <TextField
                            label={`Vrednost`}
                            variant={`outlined`}
                            size={`small`}
                            type={`number`}
                            sx={{ width: 150 }}
                            value={noviKoeficijent.vrednost}
                            onChange={(e) =>
                                setNoviKoeficijent((prev) => ({
                                    ...prev,
                                    vrednost: e.target.value,
                                }))
                            }
                        />
                        <IconButton
                            color={`primary`}
                            onClick={handleAddKoeficijent}
                        >
                            <Add />
                        </IconButton>
                    </Stack>

                    <Stack direction={`row`} justifyContent={`flex-end`} mt={3}>
                        <Button onClick={handleClose}>Zatvori</Button>
                    </Stack>
                </Stack>
            </DialogContent>
        </Dialog>
    )
}
