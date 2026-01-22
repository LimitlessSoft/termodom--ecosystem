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
    Chip,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
} from '@mui/material'
import { Add, Edit, Delete, Visibility } from '@mui/icons-material'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '@/constants'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { toast } from 'react-toastify'

const TICKET_TYPES = {
    0: { label: 'Bug', color: 'error' },
    1: { label: 'Feature', color: 'primary' },
}

const TICKET_STATUSES = {
    0: { label: 'Novi', color: 'default' },
    1: { label: 'U toku', color: 'info' },
    2: { label: 'Rešen', color: 'success' },
    3: { label: 'Odbijen', color: 'error' },
}

const TICKET_PRIORITIES = {
    0: { label: 'Nizak', color: 'default' },
    1: { label: 'Srednji', color: 'warning' },
    2: { label: 'Visok', color: 'error' },
    3: { label: 'Kritičan', color: 'error' },
}

export const Tickets = () => {
    const [tickets, setTickets] = useState([])
    const [loading, setLoading] = useState(true)
    const [dialogOpen, setDialogOpen] = useState(false)
    const [detailsDialogOpen, setDetailsDialogOpen] = useState(false)
    const [selectedTicket, setSelectedTicket] = useState(null)
    const [title, setTitle] = useState('')
    const [description, setDescription] = useState('')
    const [type, setType] = useState(0)
    const [saving, setSaving] = useState(false)

    // Filters
    const [filterType, setFilterType] = useState('')
    const [filterStatus, setFilterStatus] = useState('')
    const [filterPriority, setFilterPriority] = useState('')

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.TICKETS
    )

    const canCreateBug = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TICKETS.CREATE_BUG
    )

    const canCreateFeature = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TICKETS.CREATE_FEATURE
    )

    const canManagePriority = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TICKETS.MANAGE_PRIORITY
    )

    const canManageStatus = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TICKETS.MANAGE_STATUS
    )

    const canDeveloperNotes = hasPermission(
        permissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.TICKETS.DEVELOPER_NOTES
    )

    const canCreate = canCreateBug || canCreateFeature

    const fetchTickets = useCallback(async () => {
        setLoading(true)
        try {
            let url = ENDPOINTS_CONSTANTS.TICKETS.GET_MULTIPLE
            const params = new URLSearchParams()
            if (filterType !== '') params.append('type', filterType)
            if (filterStatus !== '') params.append('status', filterStatus)
            if (filterPriority !== '') params.append('priority', filterPriority)
            if (params.toString()) url += '?' + params.toString()

            const response = await officeApi.get(url)
            setTickets(response.data)
        } catch (err) {
            handleApiError(err)
        } finally {
            setLoading(false)
        }
    }, [filterType, filterStatus, filterPriority])

    useEffect(() => {
        fetchTickets()
    }, [fetchTickets])

    const handleOpenDialog = (ticket = null) => {
        if (ticket) {
            setSelectedTicket(ticket)
            setTitle(ticket.title)
            setDescription(ticket.description)
            setType(ticket.type)
        } else {
            setSelectedTicket(null)
            setTitle('')
            setDescription('')
            setType(canCreateBug ? 0 : 1)
        }
        setDialogOpen(true)
    }

    const handleCloseDialog = () => {
        setDialogOpen(false)
        setSelectedTicket(null)
        setTitle('')
        setDescription('')
        setType(0)
    }

    const handleViewDetails = async (id) => {
        try {
            const response = await officeApi.get(ENDPOINTS_CONSTANTS.TICKETS.GET(id))
            setSelectedTicket(response.data)
            setDetailsDialogOpen(true)
        } catch (err) {
            handleApiError(err)
        }
    }

    const handleSave = async () => {
        if (!title.trim()) {
            toast.error('Naslov je obavezno polje')
            return
        }
        if (!description.trim()) {
            toast.error('Opis je obavezno polje')
            return
        }

        setSaving(true)
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.TICKETS.SAVE, {
                id: selectedTicket?.id || null,
                title: title.trim(),
                description: description.trim(),
                type,
            })
            toast.success(selectedTicket ? 'Tiket ažuriran' : 'Tiket kreiran')
            handleCloseDialog()
            await fetchTickets()
        } catch (err) {
            handleApiError(err)
        } finally {
            setSaving(false)
        }
    }

    const handleDelete = async (id) => {
        if (!window.confirm('Da li ste sigurni da želite da obrišete ovaj tiket?')) {
            return
        }

        try {
            await officeApi.delete(ENDPOINTS_CONSTANTS.TICKETS.DELETE(id))
            toast.success('Tiket obrisan')
            await fetchTickets()
        } catch (err) {
            handleApiError(err)
        }
    }

    const handleUpdatePriority = async (id, priority) => {
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.TICKETS.UPDATE_PRIORITY(id), { priority })
            toast.success('Prioritet ažuriran')
            await fetchTickets()
        } catch (err) {
            handleApiError(err)
        }
    }

    const handleUpdateStatus = async (id, status) => {
        try {
            await officeApi.put(ENDPOINTS_CONSTANTS.TICKETS.UPDATE_STATUS(id), { status })
            toast.success('Status ažuriran')
            await fetchTickets()
            if (detailsDialogOpen) {
                const response = await officeApi.get(ENDPOINTS_CONSTANTS.TICKETS.GET(id))
                setSelectedTicket(response.data)
            }
        } catch (err) {
            handleApiError(err)
        }
    }

    const formatDate = (dateString) => {
        if (!dateString) return '-'
        return new Date(dateString).toLocaleDateString('sr-Latn-RS', {
            day: '2-digit',
            month: '2-digit',
            year: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        })
    }

    return (
        <Box sx={{ p: 2 }}>
            <Paper sx={{ p: 2 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h5">Tiketi</Typography>
                    {canCreate && (
                        <Button
                            variant="contained"
                            startIcon={<Add />}
                            onClick={() => handleOpenDialog()}
                        >
                            Novi tiket
                        </Button>
                    )}
                </Box>

                {/* Filters */}
                <Box sx={{ display: 'flex', gap: 2, mb: 2, flexWrap: 'wrap' }}>
                    <FormControl size="small" sx={{ minWidth: 120 }}>
                        <InputLabel>Tip</InputLabel>
                        <Select
                            value={filterType}
                            label="Tip"
                            onChange={(e) => setFilterType(e.target.value)}
                        >
                            <MenuItem value="">Svi</MenuItem>
                            <MenuItem value={0}>Bug</MenuItem>
                            <MenuItem value={1}>Feature</MenuItem>
                        </Select>
                    </FormControl>
                    <FormControl size="small" sx={{ minWidth: 120 }}>
                        <InputLabel>Status</InputLabel>
                        <Select
                            value={filterStatus}
                            label="Status"
                            onChange={(e) => setFilterStatus(e.target.value)}
                        >
                            <MenuItem value="">Svi</MenuItem>
                            <MenuItem value={0}>Novi</MenuItem>
                            <MenuItem value={1}>U toku</MenuItem>
                            <MenuItem value={2}>Rešen</MenuItem>
                            <MenuItem value={3}>Odbijen</MenuItem>
                        </Select>
                    </FormControl>
                    <FormControl size="small" sx={{ minWidth: 120 }}>
                        <InputLabel>Prioritet</InputLabel>
                        <Select
                            value={filterPriority}
                            label="Prioritet"
                            onChange={(e) => setFilterPriority(e.target.value)}
                        >
                            <MenuItem value="">Svi</MenuItem>
                            <MenuItem value={0}>Nizak</MenuItem>
                            <MenuItem value={1}>Srednji</MenuItem>
                            <MenuItem value={2}>Visok</MenuItem>
                            <MenuItem value={3}>Kritičan</MenuItem>
                        </Select>
                    </FormControl>
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
                                    <TableCell>Naslov</TableCell>
                                    <TableCell>Tip</TableCell>
                                    <TableCell>Status</TableCell>
                                    <TableCell>Prioritet</TableCell>
                                    <TableCell>Kreirao</TableCell>
                                    <TableCell>Datum</TableCell>
                                    <TableCell align="right">Akcije</TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {tickets.map((ticket) => (
                                    <TableRow key={ticket.id}>
                                        <TableCell>{ticket.title}</TableCell>
                                        <TableCell>
                                            <Chip
                                                label={TICKET_TYPES[ticket.type]?.label}
                                                color={TICKET_TYPES[ticket.type]?.color}
                                                size="small"
                                            />
                                        </TableCell>
                                        <TableCell>
                                            {canManageStatus ? (
                                                <Select
                                                    value={ticket.status}
                                                    size="small"
                                                    onChange={(e) => handleUpdateStatus(ticket.id, e.target.value)}
                                                >
                                                    <MenuItem value={0}>Novi</MenuItem>
                                                    <MenuItem value={1}>U toku</MenuItem>
                                                    <MenuItem value={2}>Rešen</MenuItem>
                                                    <MenuItem value={3}>Odbijen</MenuItem>
                                                </Select>
                                            ) : (
                                                <Chip
                                                    label={TICKET_STATUSES[ticket.status]?.label}
                                                    color={TICKET_STATUSES[ticket.status]?.color}
                                                    size="small"
                                                />
                                            )}
                                        </TableCell>
                                        <TableCell>
                                            {canManagePriority ? (
                                                <Select
                                                    value={ticket.priority}
                                                    size="small"
                                                    onChange={(e) => handleUpdatePriority(ticket.id, e.target.value)}
                                                >
                                                    <MenuItem value={0}>Nizak</MenuItem>
                                                    <MenuItem value={1}>Srednji</MenuItem>
                                                    <MenuItem value={2}>Visok</MenuItem>
                                                    <MenuItem value={3}>Kritičan</MenuItem>
                                                </Select>
                                            ) : (
                                                <Chip
                                                    label={TICKET_PRIORITIES[ticket.priority]?.label}
                                                    color={TICKET_PRIORITIES[ticket.priority]?.color}
                                                    size="small"
                                                    variant="outlined"
                                                />
                                            )}
                                        </TableCell>
                                        <TableCell>{ticket.submittedByUserNickname}</TableCell>
                                        <TableCell>{formatDate(ticket.createdAt)}</TableCell>
                                        <TableCell align="right">
                                            <IconButton
                                                size="small"
                                                onClick={() => handleViewDetails(ticket.id)}
                                            >
                                                <Visibility />
                                            </IconButton>
                                            {canCreate && (
                                                <>
                                                    <IconButton
                                                        size="small"
                                                        onClick={() => handleOpenDialog(ticket)}
                                                    >
                                                        <Edit />
                                                    </IconButton>
                                                    <IconButton
                                                        size="small"
                                                        color="error"
                                                        onClick={() => handleDelete(ticket.id)}
                                                    >
                                                        <Delete />
                                                    </IconButton>
                                                </>
                                            )}
                                        </TableCell>
                                    </TableRow>
                                ))}
                                {tickets.length === 0 && (
                                    <TableRow>
                                        <TableCell colSpan={7} align="center">
                                            Nema tiketa
                                        </TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </TableContainer>
                )}
            </Paper>

            {/* Create/Edit Dialog */}
            <Dialog open={dialogOpen} onClose={handleCloseDialog} maxWidth="md" fullWidth>
                <DialogTitle>
                    {selectedTicket ? 'Izmeni tiket' : 'Novi tiket'}
                </DialogTitle>
                <DialogContent>
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
                        <TextField
                            label="Naslov"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            fullWidth
                            required
                        />
                        <FormControl fullWidth>
                            <InputLabel>Tip</InputLabel>
                            <Select
                                value={type}
                                label="Tip"
                                onChange={(e) => setType(e.target.value)}
                                disabled={selectedTicket !== null}
                            >
                                {canCreateBug && <MenuItem value={0}>Bug</MenuItem>}
                                {canCreateFeature && <MenuItem value={1}>Feature</MenuItem>}
                            </Select>
                        </FormControl>
                        <TextField
                            label="Opis"
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                            fullWidth
                            required
                            multiline
                            rows={6}
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

            {/* Details Dialog */}
            <Dialog open={detailsDialogOpen} onClose={() => setDetailsDialogOpen(false)} maxWidth="md" fullWidth>
                <DialogTitle>Detalji tiketa</DialogTitle>
                <DialogContent>
                    {selectedTicket && (
                        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
                            <Typography variant="h6">{selectedTicket.title}</Typography>
                            <Box sx={{ display: 'flex', gap: 1 }}>
                                <Chip
                                    label={TICKET_TYPES[selectedTicket.type]?.label}
                                    color={TICKET_TYPES[selectedTicket.type]?.color}
                                    size="small"
                                />
                                <Chip
                                    label={TICKET_STATUSES[selectedTicket.status]?.label}
                                    color={TICKET_STATUSES[selectedTicket.status]?.color}
                                    size="small"
                                />
                                <Chip
                                    label={TICKET_PRIORITIES[selectedTicket.priority]?.label}
                                    color={TICKET_PRIORITIES[selectedTicket.priority]?.color}
                                    size="small"
                                    variant="outlined"
                                />
                            </Box>
                            <Typography variant="body2" color="text.secondary">
                                Kreirao: {selectedTicket.submittedByUserNickname} | {formatDate(selectedTicket.createdAt)}
                            </Typography>
                            <Paper variant="outlined" sx={{ p: 2 }}>
                                <Typography variant="subtitle2" gutterBottom>Opis:</Typography>
                                <Typography variant="body2" sx={{ whiteSpace: 'pre-wrap' }}>
                                    {selectedTicket.description}
                                </Typography>
                            </Paper>
                            {selectedTicket.developerNotes && (
                                <Paper variant="outlined" sx={{ p: 2, bgcolor: 'action.hover' }}>
                                    <Typography variant="subtitle2" gutterBottom>Developer napomene:</Typography>
                                    <Typography variant="body2" sx={{ whiteSpace: 'pre-wrap' }}>
                                        {selectedTicket.developerNotes}
                                    </Typography>
                                </Paper>
                            )}
                            {selectedTicket.resolvedAt && (
                                <Paper variant="outlined" sx={{ p: 2 }}>
                                    <Typography variant="subtitle2" gutterBottom>Rešenje:</Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        Rešio: {selectedTicket.resolvedByUserNickname} | {formatDate(selectedTicket.resolvedAt)}
                                    </Typography>
                                    {selectedTicket.resolutionNotes && (
                                        <Typography variant="body2" sx={{ whiteSpace: 'pre-wrap', mt: 1 }}>
                                            {selectedTicket.resolutionNotes}
                                        </Typography>
                                    )}
                                </Paper>
                            )}
                        </Box>
                    )}
                </DialogContent>
                <DialogActions sx={{ px: 3, pb: 2 }}>
                    <Button onClick={() => setDetailsDialogOpen(false)}>
                        Zatvori
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    )
}
