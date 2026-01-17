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

export const TipoviOdsustva = () => {
    const [tipoviOdsustva, setTipoviOdsustva] = useState([])
    const [loading, setLoading] = useState(true)
    const [dialogOpen, setDialogOpen] = useState(false)
    const [selectedTip, setSelectedTip] = useState(null)
    const [naziv, setNaziv] = useState('')
    const [redosled, setRedosled] = useState(0)
    const [saving, setSaving] = useState(false)

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.TIP_ODSUSTVA
    )

    const canWrite = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TIP_ODSUSTVA.WRITE
    )

    const fetchTipoviOdsustva = useCallback(async () => {
        setLoading(true)
        try {
            const response = await officeApi.get(
                ENDPOINTS_CONSTANTS.TIP_ODSUSTVA.GET_MULTIPLE
            )
            setTipoviOdsustva(response.data)
        } catch (err) {
            handleApiError(err)
        } finally {
            setLoading(false)
        }
    }, [])

    useEffect(() => {
        fetchTipoviOdsustva()
    }, [fetchTipoviOdsustva])

    const handleOpenDialog = (tip = null) => {
        if (tip) {
            setSelectedTip(tip)
            setNaziv(tip.naziv)
            setRedosled(tip.redosled)
        } else {
            setSelectedTip(null)
            setNaziv('')
            setRedosled(tipoviOdsustva.length)
        }
        setDialogOpen(true)
    }

    const handleCloseDialog = () => {
        setDialogOpen(false)
        setSelectedTip(null)
        setNaziv('')
        setRedosled(0)
    }

    const handleSave = async () => {
        if (!naziv.trim()) {
            toast.error('Naziv je obavezno polje')
            return
        }

        setSaving(true)
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.TIP_ODSUSTVA.SAVE, {
                id: selectedTip?.id || null,
                naziv: naziv.trim(),
                redosled,
            })
            toast.success(selectedTip ? 'Tip odsustva ažuriran' : 'Tip odsustva dodat')
            handleCloseDialog()
            await fetchTipoviOdsustva()
        } catch (err) {
            handleApiError(err)
        } finally {
            setSaving(false)
        }
    }

    const handleDelete = async (id) => {
        if (!window.confirm('Da li ste sigurni da želite da obrišete ovaj tip odsustva?')) {
            return
        }

        try {
            await officeApi.delete(ENDPOINTS_CONSTANTS.TIP_ODSUSTVA.DELETE(id))
            toast.success('Tip odsustva obrisan')
            await fetchTipoviOdsustva()
        } catch (err) {
            handleApiError(err)
        }
    }

    return (
        <Box sx={{ p: 2 }}>
            <Paper sx={{ p: 2 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h5">Tipovi odsustva</Typography>
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
                                    <TableCell>Redosled</TableCell>
                                    {canWrite && <TableCell align="right">Akcije</TableCell>}
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {tipoviOdsustva.map((tip) => (
                                    <TableRow key={tip.id}>
                                        <TableCell>{tip.naziv}</TableCell>
                                        <TableCell>{tip.redosled}</TableCell>
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
                                {tipoviOdsustva.length === 0 && (
                                    <TableRow>
                                        <TableCell colSpan={canWrite ? 3 : 2} align="center">
                                            Nema tipova odsustva
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
                    {selectedTip ? 'Izmeni tip odsustva' : 'Novi tip odsustva'}
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
                        <TextField
                            label="Redosled"
                            type="number"
                            value={redosled}
                            onChange={(e) => setRedosled(parseInt(e.target.value) || 0)}
                            fullWidth
                        />
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
