import { useState, useEffect } from 'react'
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Box,
    CircularProgress,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers/DatePicker'
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider'
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFnsV3'
import { srLatn } from 'date-fns/locale/sr-Latn'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '@/constants'
import { toast } from 'react-toastify'

export const OdsustvoDialog = ({
    open,
    onClose,
    onSave,
    odsustvo,
    initialDate,
    tipoviOdsustva,
    canEditAll,
}) => {
    const [tipOdsustvaId, setTipOdsustvaId] = useState('')
    const [datumOd, setDatumOd] = useState(null)
    const [datumDo, setDatumDo] = useState(null)
    const [komentar, setKomentar] = useState('')
    const [saving, setSaving] = useState(false)
    const [deleting, setDeleting] = useState(false)

    useEffect(() => {
        if (open) {
            if (odsustvo) {
                setTipOdsustvaId(odsustvo.tipOdsustvaId)
                setDatumOd(new Date(odsustvo.datumOd))
                setDatumDo(new Date(odsustvo.datumDo))
                setKomentar(odsustvo.komentar || '')
            } else {
                setTipOdsustvaId(tipoviOdsustva[0]?.id || '')
                setDatumOd(initialDate || new Date())
                setDatumDo(initialDate || new Date())
                setKomentar('')
            }
        }
    }, [open, odsustvo, initialDate, tipoviOdsustva])

    const handleSave = async () => {
        if (!tipOdsustvaId || !datumOd || !datumDo) {
            toast.error('Molimo popunite sva obavezna polja')
            return
        }

        if (datumDo < datumOd) {
            toast.error('Datum do mora biti veći ili jednak datumu od')
            return
        }

        setSaving(true)
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.ODSUSTVO.SAVE, {
                id: odsustvo?.id || null,
                tipOdsustvaId,
                datumOd: datumOd.toISOString(),
                datumDo: datumDo.toISOString(),
                komentar: komentar || null,
                userId: odsustvo?.userId || null,
            })
            toast.success(odsustvo ? 'Odsustvo ažurirano' : 'Odsustvo dodato')
            onSave()
        } catch (err) {
            handleApiError(err)
        } finally {
            setSaving(false)
        }
    }

    const handleDelete = async () => {
        if (!odsustvo?.id) return

        setDeleting(true)
        try {
            await officeApi.delete(ENDPOINTS_CONSTANTS.ODSUSTVO.DELETE(odsustvo.id))
            toast.success('Odsustvo obrisano')
            onSave()
        } catch (err) {
            handleApiError(err)
        } finally {
            setDeleting(false)
        }
    }

    const isEditing = !!odsustvo

    return (
        <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
            <DialogTitle>
                {isEditing ? 'Izmeni odsustvo' : 'Novo odsustvo'}
                {isEditing && odsustvo.userNickname && (
                    <Box component="span" sx={{ fontWeight: 'normal', ml: 1 }}>
                        - {odsustvo.userNickname}
                    </Box>
                )}
            </DialogTitle>
            <DialogContent>
                <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
                    <FormControl fullWidth>
                        <InputLabel>Tip odsustva</InputLabel>
                        <Select
                            value={tipOdsustvaId}
                            label="Tip odsustva"
                            onChange={(e) => setTipOdsustvaId(e.target.value)}
                        >
                            {tipoviOdsustva.map((tip) => (
                                <MenuItem key={tip.id} value={tip.id}>
                                    {tip.naziv}
                                </MenuItem>
                            ))}
                        </Select>
                    </FormControl>

                    <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={srLatn}>
                        <DatePicker
                            label="Datum od"
                            value={datumOd}
                            onChange={(newValue) => setDatumOd(newValue)}
                            slotProps={{
                                textField: { fullWidth: true },
                            }}
                        />

                        <DatePicker
                            label="Datum do"
                            value={datumDo}
                            onChange={(newValue) => setDatumDo(newValue)}
                            slotProps={{
                                textField: { fullWidth: true },
                            }}
                        />
                    </LocalizationProvider>

                    <TextField
                        label="Komentar"
                        value={komentar}
                        onChange={(e) => setKomentar(e.target.value)}
                        multiline
                        rows={3}
                        fullWidth
                    />
                </Box>
            </DialogContent>
            <DialogActions sx={{ px: 3, pb: 2 }}>
                {isEditing && (
                    <Button
                        onClick={handleDelete}
                        color="error"
                        disabled={deleting || saving}
                        sx={{ mr: 'auto' }}
                    >
                        {deleting ? <CircularProgress size={20} /> : 'Obriši'}
                    </Button>
                )}
                <Button onClick={onClose} disabled={saving || deleting}>
                    Odustani
                </Button>
                <Button
                    onClick={handleSave}
                    variant="contained"
                    disabled={saving || deleting}
                >
                    {saving ? <CircularProgress size={20} /> : 'Sačuvaj'}
                </Button>
            </DialogActions>
        </Dialog>
    )
}
