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
    Chip,
    Typography,
    FormControlLabel,
    Checkbox,
    Divider,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers/DatePicker'
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider'
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFnsV3'
import { srLatn } from 'date-fns/locale/sr-Latn'
import { format } from 'date-fns'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '@/constants'
import { ODSUSTVO_CONSTANTS } from '@/constants/odsustvo/odsustvoConstants'
import { toast } from 'react-toastify'

const formatDate = (dateString) => {
    if (!dateString) return ''
    return format(new Date(dateString), 'dd.MM.yyyy HH:mm')
}

export const OdsustvoDialog = ({
    open,
    onClose,
    onSave,
    odsustvo,
    initialDate,
    tipoviOdsustva,
    canEditAll,
    canApprove,
}) => {
    const [tipOdsustvaId, setTipOdsustvaId] = useState('')
    const [datumOd, setDatumOd] = useState(null)
    const [datumDo, setDatumDo] = useState(null)
    const [komentar, setKomentar] = useState('')
    const [saving, setSaving] = useState(false)
    const [deleting, setDeleting] = useState(false)
    const [approving, setApproving] = useState(false)
    const [realizovanoKorisnik, setRealizovanoKorisnik] = useState(false)
    const [realizovanoOdobravac, setRealizovanoOdobravac] = useState(false)

    useEffect(() => {
        if (open) {
            if (odsustvo) {
                setTipOdsustvaId(odsustvo.tipOdsustvaId)
                setDatumOd(new Date(odsustvo.datumOd))
                setDatumDo(new Date(odsustvo.datumDo))
                setKomentar(odsustvo.komentar || '')
                setRealizovanoKorisnik(odsustvo.realizovanoKorisnik || false)
                setRealizovanoOdobravac(odsustvo.realizovanoOdobravac || false)
            } else {
                setTipOdsustvaId(tipoviOdsustva[0]?.id || '')
                setDatumOd(initialDate || new Date())
                setDatumDo(initialDate || new Date())
                setKomentar('')
                setRealizovanoKorisnik(false)
                setRealizovanoOdobravac(false)
            }
        }
    }, [open, odsustvo, initialDate, tipoviOdsustva])

    const handleSave = async () => {
        if (!tipOdsustvaId || !datumOd || !datumDo) {
            toast.error('Molimo popunite sva obavezna polja')
            return
        }

        if (datumDo < datumOd) {
            toast.error('Datum do mora biti veci ili jednak datumu od')
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
            toast.success(odsustvo ? 'Odsustvo azurirano' : 'Odsustvo dodato')
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

    const handleApprove = async () => {
        if (!odsustvo?.id) return

        setApproving(true)
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.ODSUSTVO.APPROVE(odsustvo.id))
            toast.success('Odsustvo odobreno')
            onSave()
        } catch (err) {
            handleApiError(err)
        } finally {
            setApproving(false)
        }
    }

    const handleRealizovanoKorisnikChange = async (e) => {
        const newValue = e.target.checked
        setRealizovanoKorisnik(newValue)
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.ODSUSTVO.REALIZOVANO(odsustvo.id), {
                realizovanoKorisnik: newValue,
            })
        } catch (err) {
            handleApiError(err)
            setRealizovanoKorisnik(!newValue)
        }
    }

    const handleRealizovanoOdobravacChange = async (e) => {
        const newValue = e.target.checked
        setRealizovanoOdobravac(newValue)
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.ODSUSTVO.REALIZOVANO(odsustvo.id), {
                realizovanoOdobravac: newValue,
            })
        } catch (err) {
            handleApiError(err)
            setRealizovanoOdobravac(!newValue)
        }
    }

    const isEditing = !!odsustvo
    const isApproved = odsustvo?.status === ODSUSTVO_CONSTANTS.STATUS.ODOBRENO
    const isPending = odsustvo?.status === ODSUSTVO_CONSTANTS.STATUS.CEKA_ODOBRENJE
    const canEditFields = !isEditing || !isApproved || canEditAll

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
                    {isEditing && (
                        <>
                            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flexWrap: 'wrap' }}>
                                <Typography variant="body2" color="text.secondary">
                                    Status:
                                </Typography>
                                <Chip
                                    label={ODSUSTVO_CONSTANTS.STATUS_LABELS[odsustvo.status]}
                                    size="small"
                                    sx={{
                                        bgcolor: ODSUSTVO_CONSTANTS.STATUS_COLORS[odsustvo.status],
                                        color: 'white',
                                    }}
                                />
                            </Box>
                            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                                <Typography variant="caption" color="text.secondary">
                                    Kreirano: {formatDate(odsustvo.createdAt)}
                                </Typography>
                                {isApproved && odsustvo.odobrenoAt && (
                                    <Typography variant="caption" color="text.secondary">
                                        Odobreno: {formatDate(odsustvo.odobrenoAt)}
                                        {odsustvo.odobrenoByNickname && ` (${odsustvo.odobrenoByNickname})`}
                                    </Typography>
                                )}
                            </Box>
                            <Divider />
                        </>
                    )}

                    <FormControl fullWidth>
                        <InputLabel>Tip odsustva</InputLabel>
                        <Select
                            value={tipOdsustvaId}
                            label="Tip odsustva"
                            onChange={(e) => setTipOdsustvaId(e.target.value)}
                            disabled={!canEditFields}
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
                            disabled={!canEditFields}
                            slotProps={{
                                textField: { fullWidth: true },
                            }}
                        />

                        <DatePicker
                            label="Datum do"
                            value={datumDo}
                            onChange={(newValue) => setDatumDo(newValue)}
                            disabled={!canEditFields}
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
                        disabled={!canEditFields}
                    />

                    {isEditing && (
                        <>
                            <Divider />
                            <Typography variant="subtitle2" color="text.secondary">
                                Realizovano
                            </Typography>
                            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            checked={realizovanoKorisnik}
                                            onChange={handleRealizovanoKorisnikChange}
                                            disabled={saving || deleting || approving}
                                        />
                                    }
                                    label="Realizovano (korisnik)"
                                />
                                <FormControlLabel
                                    control={
                                        <Checkbox
                                            checked={realizovanoOdobravac}
                                            onChange={handleRealizovanoOdobravacChange}
                                            disabled={!canApprove || saving || deleting || approving}
                                        />
                                    }
                                    label="Realizovano (odobravac)"
                                />
                            </Box>
                        </>
                    )}
                </Box>
            </DialogContent>
            <DialogActions sx={{ px: 3, pb: 2 }}>
                {isEditing && (
                    <Button
                        onClick={handleDelete}
                        color="error"
                        disabled={deleting || saving || approving}
                        sx={{ mr: 'auto' }}
                    >
                        {deleting ? <CircularProgress size={20} /> : 'Obrisi'}
                    </Button>
                )}
                {isEditing && canApprove && isPending && (
                    <Button
                        onClick={handleApprove}
                        color="success"
                        variant="contained"
                        disabled={approving || saving || deleting}
                    >
                        {approving ? <CircularProgress size={20} /> : 'Odobri'}
                    </Button>
                )}
                <Button onClick={onClose} disabled={saving || deleting || approving}>
                    Odustani
                </Button>
                {canEditFields && (
                    <Button
                        onClick={handleSave}
                        variant="contained"
                        disabled={saving || deleting || approving}
                    >
                        {saving ? <CircularProgress size={20} /> : 'Sacuvaj'}
                    </Button>
                )}
            </DialogActions>
        </Dialog>
    )
}
