import { useState, useEffect, useCallback } from 'react'
import {
    Box,
    Button,
    CircularProgress,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Typography,
} from '@mui/material'
import { Save } from '@mui/icons-material'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '@/constants'
import { toast } from 'react-toastify'

export const KorisniciSingularTipKorisnika = ({ userId, currentTipKorisnikaId, onUpdate }) => {
    const [tipoviKorisnika, setTipoviKorisnika] = useState([])
    const [selectedTipId, setSelectedTipId] = useState(currentTipKorisnikaId || '')
    const [loading, setLoading] = useState(true)
    const [saving, setSaving] = useState(false)

    const hasChanged = selectedTipId !== (currentTipKorisnikaId || '')

    const fetchTipoviKorisnika = useCallback(async () => {
        setLoading(true)
        try {
            const response = await officeApi.get(
                ENDPOINTS_CONSTANTS.TIP_KORISNIKA.GET_MULTIPLE
            )
            setTipoviKorisnika(response.data)
        } catch (err) {
            handleApiError(err)
        } finally {
            setLoading(false)
        }
    }, [])

    useEffect(() => {
        fetchTipoviKorisnika()
    }, [fetchTipoviKorisnika])

    useEffect(() => {
        setSelectedTipId(currentTipKorisnikaId || '')
    }, [currentTipKorisnikaId])

    const handleSave = async () => {
        setSaving(true)
        try {
            await officeApi.put(
                ENDPOINTS_CONSTANTS.USERS.UPDATE_TIP_KORISNIKA_ID(userId),
                {
                    id: userId,
                    tipKorisnikaId: selectedTipId || null,
                }
            )
            toast.success('Tip korisnika uspešno promenjen')
            if (onUpdate) {
                const selectedTip = tipoviKorisnika.find(t => t.id === selectedTipId)
                onUpdate(selectedTipId || null, selectedTip?.naziv || null, selectedTip?.boja || null)
            }
        } catch (err) {
            handleApiError(err)
            setSelectedTipId(currentTipKorisnikaId || '')
        } finally {
            setSaving(false)
        }
    }

    if (loading) {
        return (
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <Typography>Tip korisnika:</Typography>
                <CircularProgress size={20} />
            </Box>
        )
    }

    return (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, flexWrap: 'wrap' }}>
            <Typography>Tip korisnika:</Typography>
            <FormControl size="small" sx={{ minWidth: 180 }}>
                <Select
                    value={selectedTipId}
                    onChange={(e) => setSelectedTipId(e.target.value)}
                    disabled={saving}
                    displayEmpty
                >
                    <MenuItem value="">Bez tipa</MenuItem>
                    {tipoviKorisnika.map((tip) => (
                        <MenuItem key={tip.id} value={tip.id}>
                            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                <Box
                                    sx={{
                                        width: 16,
                                        height: 16,
                                        backgroundColor: tip.boja,
                                        borderRadius: 0.5,
                                        border: '1px solid #ccc',
                                    }}
                                />
                                {tip.naziv}
                            </Box>
                        </MenuItem>
                    ))}
                </Select>
            </FormControl>
            {hasChanged && (
                <Button
                    variant="contained"
                    size="small"
                    startIcon={saving ? <CircularProgress size={16} /> : <Save />}
                    onClick={handleSave}
                    disabled={saving}
                >
                    Sačuvaj
                </Button>
            )}
        </Box>
    )
}
