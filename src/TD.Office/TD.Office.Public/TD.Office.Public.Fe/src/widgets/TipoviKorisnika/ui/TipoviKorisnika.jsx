import { useState, useEffect, useCallback } from 'react'
import {
    Box,
    Button,
    Paper,
    Typography,
    CircularProgress,
    IconButton,
    TextField,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@mui/material'
import { Add, Edit, Delete } from '@mui/icons-material'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { toast } from 'react-toastify'

export const TipoviKorisnika = () => {
    const [tipoviKorisnika, setTipoviKorisnika] = useState([])
    const [loading, setLoading] = useState(true)
    const [dialogOpen, setDialogOpen] = useState(false)
    const [selectedTip, setSelectedTip] = useState(null)
    const [naziv, setNaziv] = useState('')
    const [boja, setBoja] = useState('#4CAF50')
    const [saving, setSaving] = useState(false)

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.TIP_KORISNIKA
    )

    const canWrite = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TIP_KORISNIKA.WRITE
    )

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

    const handleOpenDialog = (tip = null) => {
        if (tip) {
            setSelectedTip(tip)
            setNaziv(tip.naziv)
            setBoja(tip.boja)
        } else {
            setSelectedTip(null)
            setNaziv('')
            setBoja('#4CAF50')
        }
        setDialogOpen(true)
    }

    const handleCloseDialog = () => {
        setDialogOpen(false)
        setSelectedTip(null)
        setNaziv('')
        setBoja('#4CAF50')
    }

    const handleSave = async () => {
        if (!naziv.trim()) {
            toast.error('Naziv je obavezno polje')
            return
        }

        if (!/^#[0-9A-Fa-f]{6}$/.test(boja)) {
            toast.error('Boja mora biti u hex formatu (#RRGGBB)')
            return
        }

        setSaving(true)
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.TIP_KORISNIKA.SAVE, {
                id: selectedTip?.id || null,
                naziv: naziv.trim(),
                boja,
            })
            toast.success(selectedTip ? 'Tip korisnika ažuriran' : 'Tip korisnika dodat')
            handleCloseDialog()
            await fetchTipoviKorisnika()
        } catch (err) {
            handleApiError(err)
        } finally {
            setSaving(false)
        }
    }

    const handleDelete = async (id) => {
        if (!window.confirm('Da li ste sigurni da želite da obrišete ovaj tip korisnika?')) {
            return
        }

        try {
            await officeApi.delete(ENDPOINTS_CONSTANTS.TIP_KORISNIKA.DELETE(id))
            toast.success('Tip korisnika obrisan')
            await fetchTipoviKorisnika()
        } catch (err) {
            handleApiError(err)
        }
    }

    return (
        <Box sx={{ p: 2 }}>
            <Paper sx={{ p: 2 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h5">Tipovi korisnika</Typography>
                    {canWrite && (
                        <Button
                            variant="contained"
                            startIcon={<Add />}
                            onClick={() => handleOpenDialog()}
                        >
                            Novi tip
                        </Button>
                    )}
                </Box>

                {loading ? (
                    <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
                        <CircularProgress />
                    </Box>
                ) : (
                    <TableContainer>
                        <Table>
                            <TableHead>
                                <TableRow>
                                    <TableCell>Naziv</TableCell>
                                    <TableCell>Boja</TableCell>
                                    {canWrite && <TableCell align="right">Akcije</TableCell>}
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {tipoviKorisnika.map((tip) => (
                                    <TableRow key={tip.id}>
                                        <TableCell>{tip.naziv}</TableCell>
                                        <TableCell>
                                            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                                                <Box
                                                    sx={{
                                                        width: 24,
                                                        height: 24,
                                                        backgroundColor: tip.boja,
                                                        borderRadius: 1,
                                                        border: '1px solid #ccc',
                                                    }}
                                                />
                                                {tip.boja}
                                            </Box>
                                        </TableCell>
                                        {canWrite && (
                                            <TableCell align="right">
                                                <IconButton
                                                    size="small"
                                                    onClick={() => handleOpenDialog(tip)}
                                                >
                                                    <Edit />
                                                </IconButton>
                                                <IconButton
                                                    size="small"
                                                    color="error"
                                                    onClick={() => handleDelete(tip.id)}
                                                >
                                                    <Delete />
                                                </IconButton>
                                            </TableCell>
                                        )}
                                    </TableRow>
                                ))}
                                {tipoviKorisnika.length === 0 && (
                                    <TableRow>
                                        <TableCell colSpan={canWrite ? 3 : 2} align="center">
                                            Nema tipova korisnika
                                        </TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>
                )}
            </Paper>

            <Dialog open={dialogOpen} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
                <DialogTitle>
                    {selectedTip ? 'Izmeni tip korisnika' : 'Novi tip korisnika'}
                </DialogTitle>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
                        <TextField
                            label="Naziv"
                            value={naziv}
                            onChange={(e) => setNaziv(e.target.value)}
                            fullWidth
                            required
                        />
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                            <TextField
                                label="Boja (hex)"
                                value={boja}
                                onChange={(e) => setBoja(e.target.value)}
                                fullWidth
                                placeholder="#RRGGBB"
                            />
                            <input
                                type="color"
                                value={boja}
                                onChange={(e) => setBoja(e.target.value)}
                                style={{ width: 50, height: 40, cursor: 'pointer', border: 'none' }}
                            />
                        </Box>
                    </Box>
                </DialogContent>
                <DialogActions sx={{ px: 3, pb: 2 }}>
                    <Button onClick={handleCloseDialog} disabled={saving}>
                        Odustani
                    </Button>
                    <Button
                        onClick={handleSave}
                        variant="contained"
                        disabled={saving}
                    >
                        {saving ? <CircularProgress size={20} /> : 'Sačuvaj'}
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    )
}
