import { useState, useMemo, useCallback, useEffect } from 'react'
import NextLink from 'next/link'
import { useRouter } from 'next/router'
import {
    Box,
    Button,
    Chip,
    Container,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    IconButton,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Typography,
    CircularProgress,
    LinearProgress,
    Menu,
    MenuItem,
} from '@mui/material'
import AddIcon from '@mui/icons-material/Add'
import DeleteIcon from '@mui/icons-material/Delete'
import LockIcon from '@mui/icons-material/Lock'
import LockOpenIcon from '@mui/icons-material/LockOpen'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import SaveIcon from '@mui/icons-material/Save'
import moment from 'moment'
import { officeApi } from '../../../apis/officeApi'
import { asUtcString } from '../../../helpers/dateHelpers'
import { handleApiError } from '../../../apis/officeApi'
import { toast } from 'react-toastify'
import { PERMISSIONS_CONSTANTS } from '../../../constants'
import { usePermissions } from '../../../hooks/usePermissionsHook'
import { hasPermission } from '../../../helpers/permissionsHelpers'
import { ConfirmDialog } from '../../../widgets'

// Helper to map status to label and color
const getStatusMeta = (status) => {
    switch (status) {
        case 0:
            return { label: 'Otvoren', color: 'green' }
        case 1:
            return { label: 'Zatvoren', color: 'red' }
        case 2:
            return { label: 'Storniran', color: 'purple' }
        default:
            return { label: status, color: 'default' }
    }
}

// Helper to map numeric type (PopisDokumentType) to human-readable label
const getTypeLabel = (type) => {
    switch (type) {
        case 0:
            return 'Vanredni popis'
        case 1:
            return 'Popis za nabavku'
        default:
            return String(type ?? '')
    }
}

// Header section with basic document info
const PopisHeader = ({
    document,
    actions,
    disabled,
    isMutating,
    onToggleLock,
    isStatusMutating,
}) => {
    const {
        id,
        date,
        type,
        status,
        komercijalnoPopisBrDok,
        popisDate,
        komercijalnoNarudzbenicaBrDok,
        narudzbenicaDate,
    } = document

    const statusMeta = getStatusMeta(status)

    const formattedDate = useMemo(() => {
        if (!date) return '—'
        const m = moment(asUtcString(date))
        if (!m.isValid()) return '—'
        return m.format('DD.MM.YYYY HH:mm')
    }, [date])

    const formattedPopisDate = useMemo(() => {
        if (!popisDate) return null
        // If it already looks like an ISO string with TZ, don't use asUtcString
        const dateToParse =
            popisDate.toString().includes('T') &&
            (popisDate.toString().includes('Z') ||
                popisDate.toString().includes('+'))
                ? popisDate
                : asUtcString(popisDate)
        const m = moment(dateToParse)
        if (!m.isValid()) return null
        return m.format('DD.MM.YYYY')
    }, [popisDate])

    const formattedNarudzbenicaDate = useMemo(() => {
        if (!narudzbenicaDate) return null
        const dateToParse =
            narudzbenicaDate.toString().includes('T') &&
            (narudzbenicaDate.toString().includes('Z') ||
                narudzbenicaDate.toString().includes('+'))
                ? narudzbenicaDate
                : asUtcString(narudzbenicaDate)
        const m = moment(dateToParse)
        if (!m.isValid()) return null
        return m.format('DD.MM.YYYY')
    }, [narudzbenicaDate])

    const typeLabel = useMemo(() => getTypeLabel(type), [type])

    const isLocked = status === 1

    const [menuAnchorEl, setMenuAnchorEl] = useState(null)
    const isMenuOpen = Boolean(menuAnchorEl)

    const handleMenuOpen = (event) => {
        setMenuAnchorEl(event.currentTarget)
    }

    const handleMenuClose = () => {
        setMenuAnchorEl(null)
    }

    const handleMenuActionClick = (action) => {
        handleMenuClose()
        if (action?.onClick && !action.disabled && !disabled) {
            action.onClick()
        }
    }

    // Menu icon should only be disabled when there is a global mutation in progress,
    // not just because the document is locked (status === 1).
    // "disabled" coming from parent already includes both lock (status 1/2)
    // and global mutating flags; we want to allow menu open in locked state,
    // so we derive a lighter flag for the icon.
    const isMenuIconDisabled = isMutating || isStatusMutating

    // Permissions for locking/unlocking
    const robaPopisPermissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.ROBA_POPIS
    )
    const canLock = hasPermission(
        robaPopisPermissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.ROBA_POPIS.LOCK
    )
    const canUnlock = hasPermission(
        robaPopisPermissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.ROBA_POPIS.UNLOCK
    )
    const canStorniraj = hasPermission(
        robaPopisPermissions,
        PERMISSIONS_CONSTANTS.USER_PERMISSIONS.ROBA_POPIS.STORNIRAJ
    )

    return (
        <Paper
            elevation={1}
            sx={{
                p: 2,
                mb: 3,
                borderLeft: 6,
                borderColor: statusMeta.color,
                position: 'relative',
                overflow: 'hidden',
            }}
        >
            {isMutating && (
                <Box
                    sx={{
                        position: 'absolute',
                        inset: 0,
                        bgcolor: 'rgba(255,255,255,0.6)',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center',
                        zIndex: 1,
                    }}
                >
                    <CircularProgress size={32} />
                </Box>
            )}
            <Stack
                direction={{ xs: 'column', sm: 'row' }}
                justifyContent="space-between"
                alignItems={{ xs: 'flex-start', sm: 'center' }}
                spacing={1.5}
            >
                <Box>
                    <Typography variant="h5" fontWeight={600} gutterBottom>
                        Popis #{id}
                    </Typography>
                    {komercijalnoPopisBrDok != null && (
                        <Typography
                            variant="body2"
                            color="text.secondary"
                            sx={{ mb: 0.5 }}
                        >
                            Komercijalno POPIS br. dok: {komercijalnoPopisBrDok}
                            {formattedPopisDate && ` (${formattedPopisDate})`}
                        </Typography>
                    )}
                    {komercijalnoNarudzbenicaBrDok != null && (
                        <Typography
                            variant="body2"
                            color="text.secondary"
                            sx={{ mb: 0.5 }}
                        >
                            Komercijalno NARUDZBENICA br. dok:{' '}
                            {komercijalnoNarudzbenicaBrDok}
                            {formattedNarudzbenicaDate &&
                                ` (${formattedNarudzbenicaDate})`}
                        </Typography>
                    )}
                    <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
                        <Typography variant="body2" color="text.secondary">
                            Datum: {formattedDate}
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            Tip: {typeLabel}
                        </Typography>
                    </Stack>
                </Box>
                <Stack direction="row" spacing={1} alignItems="center">
                    <Chip
                        label={statusMeta.label}
                        variant="filled"
                        size="medium"
                        sx={{
                            backgroundColor: statusMeta.color,
                            color: '#fff',
                        }}
                    />
                    {/* Lock / Unlock button (only for status 0 or 1) */}
                    {(status === 0 || status === 1) && (
                        <IconButton
                            size="small"
                            sx={{
                                border: '1px solid',
                                borderColor: isLocked ? 'red' : 'green',
                                color: isLocked ? 'red' : 'green',
                            }}
                            onClick={onToggleLock}
                            disabled={
                                disabled ||
                                isStatusMutating ||
                                (isLocked ? !canUnlock : !canLock)
                            }
                        >
                            {isStatusMutating ? (
                                <CircularProgress size={18} />
                            ) : isLocked ? (
                                <LockIcon fontSize="small" />
                            ) : (
                                <LockOpenIcon fontSize="small" />
                            )}
                        </IconButton>
                    )}
                    {/* Three dots menu for header actions (e.g., Storniraj) */}
                    {actions && actions.length > 0 && (
                        <>
                            <IconButton
                                size="small"
                                onClick={handleMenuOpen}
                                disabled={isMenuIconDisabled}
                            >
                                <MoreVertIcon fontSize="small" />
                            </IconButton>
                            <Menu
                                anchorEl={menuAnchorEl}
                                open={isMenuOpen}
                                onClose={handleMenuClose}
                                anchorOrigin={{
                                    vertical: 'bottom',
                                    horizontal: 'right',
                                }}
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                            >
                                {actions.map((action) => {
                                    const isStornirajAction =
                                        action.key === 'storniraj'
                                    // Disable Storniraj when document is locked (status 1 or 2),
                                    // but keep other actions enabled unless a global mutation is running.
                                    const actionDisabled =
                                        isMenuIconDisabled ||
                                        (isStornirajAction &&
                                            (status === 1 ||
                                                status === 2 ||
                                                !canStorniraj)) ||
                                        action.disabled

                                    return (
                                        <MenuItem
                                            key={action.key}
                                            disabled={actionDisabled}
                                            onClick={() =>
                                                handleMenuActionClick(action)
                                            }
                                        >
                                            {action.label}
                                        </MenuItem>
                                    )
                                })}
                            </Menu>
                        </>
                    )}
                </Stack>
            </Stack>
        </Paper>
    )
}

// Table for displaying and editing items
const PopisItemsTable = ({
    items,
    onEditPopisanaKolicina,
    onEditNarucenaKolicina,
    onRemoveItem,
    disabled,
    documentId,
    showNarucenaColumn = true,
}) => {
    const [editedPopisana, setEditedPopisana] = useState({})
    const [editedNarucena, setEditedNarucena] = useState({})
    const [savingPopisana, setSavingPopisana] = useState({})
    const [savingNarucena, setSavingNarucena] = useState({})
    const [deletingItem, setDeletingItem] = useState({})

    const handlePopisanaChange = (id, value) => {
        if (disabled) return
        const parsed = Number(value)
        if (Number.isNaN(parsed) || parsed < 0) return
        setEditedPopisana((prev) => ({ ...prev, [id]: parsed }))
        onEditPopisanaKolicina(id, parsed)
    }

    const handleNarucenaChange = (id, value) => {
        if (disabled) return
        const parsed = Number(value)
        if (Number.isNaN(parsed) || parsed < 0) return
        setEditedNarucena((prev) => ({ ...prev, [id]: parsed }))
        onEditNarucenaKolicina(id, parsed)
    }

    const handleSavePopisana = async (itemId) => {
        if (!documentId) return
        const valueToSave = editedPopisana[itemId]
        if (valueToSave == null) return
        setSavingPopisana((prev) => ({ ...prev, [itemId]: true }))
        try {
            await officeApi.put(
                `/popisi/${documentId}/items${itemId}/popisana-kolicina`,
                valueToSave,
                { headers: { 'Content-Type': 'application/json' } }
            )
            toast.success('Popisana količina sačuvana')
            setEditedPopisana((prev) => {
                const next = { ...prev }
                delete next[itemId]
                return next
            })
        } catch (err) {
            handleApiError(err)
        } finally {
            setSavingPopisana((prev) => ({ ...prev, [itemId]: false }))
        }
    }

    const handleSaveNarucena = async (itemId) => {
        if (!documentId) return
        const valueToSave = editedNarucena[itemId]
        if (valueToSave == null) return
        setSavingNarucena((prev) => ({ ...prev, [itemId]: true }))
        try {
            await officeApi.put(
                `/popisi/${documentId}/items/${itemId}/narucena-kolicina`,
                valueToSave,
                { headers: { 'Content-Type': 'application/json' } }
            )
            toast.success('Naručena količina sačuvana')
            setEditedNarucena((prev) => {
                const next = { ...prev }
                delete next[itemId]
                return next
            })
        } catch (err) {
            handleApiError(err)
        } finally {
            setSavingNarucena((prev) => ({ ...prev, [itemId]: false }))
        }
    }

    const hasEditedPopisana = (itemId) =>
        Object.prototype.hasOwnProperty.call(editedPopisana, itemId)
    const hasEditedNarucena = (itemId) =>
        Object.prototype.hasOwnProperty.call(editedNarucena, itemId)

    const handleDeleteItem = async (itemId) => {
        if (!documentId || disabled) return
        setDeletingItem((prev) => ({ ...prev, [itemId]: true }))
        try {
            await officeApi.delete(`/popisi/${documentId}/items/${itemId}`)
            onRemoveItem(itemId)
            toast.success('Stavka je obrisana')
        } catch (err) {
            handleApiError(err)
        } finally {
            setDeletingItem((prev) => ({ ...prev, [itemId]: false }))
        }
    }

    return (
        <TableContainer component={Paper} elevation={1}>
            <Table size="small">
                <TableHead>
                    <TableRow>
                        <TableCell>ID</TableCell>
                        <TableCell>Naziv</TableCell>
                        <TableCell>JM</TableCell>
                        <TableCell>Popisana količina</TableCell>
                        {showNarucenaColumn && (
                            <TableCell>Naručena količina</TableCell>
                        )}
                        <TableCell align="center">Akcije</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {items.map((item) => {
                        const editedPopisanaValue = editedPopisana[item.id]
                        const editedNarucenaValue = editedNarucena[item.id]
                        const showSavePopisana = hasEditedPopisana(item.id)
                        const showSaveNarucena = hasEditedNarucena(item.id)

                        const isSavePopisanaDisabled =
                            disabled || savingPopisana[item.id]
                        const isSaveNarucenaDisabled =
                            disabled || savingNarucena[item.id]

                        // When saving or deleting this item, disable fields & actions
                        const isRowSaving =
                            Boolean(savingPopisana[item.id]) ||
                            Boolean(savingNarucena[item.id]) ||
                            Boolean(deletingItem[item.id])

                        return (
                            <TableRow key={item.id} hover>
                                <TableCell>{item.id}</TableCell>
                                <TableCell>{item.name}</TableCell>
                                <TableCell>{item.unit}</TableCell>
                                <TableCell>
                                    <Stack
                                        direction="row"
                                        spacing={1}
                                        alignItems="center"
                                    >
                                        <TextField
                                            size="small"
                                            value={
                                                showSavePopisana
                                                    ? Number(
                                                          editedPopisanaValue
                                                      )
                                                    : Number(
                                                          item.popisanaKolicina ??
                                                              0
                                                      )
                                            }
                                            onChange={(e) =>
                                                handlePopisanaChange(
                                                    item.id,
                                                    e.target.value
                                                )
                                            }
                                            inputProps={{ min: 0 }}
                                            disabled={disabled || isRowSaving}
                                        />
                                        {showSavePopisana && (
                                            <IconButton
                                                color="primary"
                                                size="small"
                                                onClick={() =>
                                                    handleSavePopisana(item.id)
                                                }
                                                disabled={
                                                    isSavePopisanaDisabled ||
                                                    isRowSaving
                                                }
                                            >
                                                {savingPopisana[item.id] ? (
                                                    <CircularProgress
                                                        size={18}
                                                    />
                                                ) : (
                                                    <SaveIcon fontSize="small" />
                                                )}
                                            </IconButton>
                                        )}
                                    </Stack>
                                </TableCell>
                                {showNarucenaColumn && (
                                    <TableCell sx={{ minWidth: 180 }}>
                                        <Stack
                                            direction="row"
                                            spacing={1}
                                            alignItems="center"
                                        >
                                            <TextField
                                                type="number"
                                                size="small"
                                                value={
                                                    showSaveNarucena
                                                        ? Number(
                                                              editedNarucenaValue
                                                          )
                                                        : Number(
                                                              item.narucenaKolicina ??
                                                                  0
                                                          )
                                                }
                                                onChange={(e) =>
                                                    handleNarucenaChange(
                                                        item.id,
                                                        e.target.value
                                                    )
                                                }
                                                inputProps={{ min: 0 }}
                                                disabled={
                                                    disabled || isRowSaving
                                                }
                                            />
                                            {showSaveNarucena && (
                                                <IconButton
                                                    color="primary"
                                                    size="small"
                                                    onClick={() =>
                                                        handleSaveNarucena(
                                                            item.id
                                                        )
                                                    }
                                                    disabled={
                                                        isSaveNarucenaDisabled ||
                                                        isRowSaving
                                                    }
                                                >
                                                    {savingNarucena[item.id] ? (
                                                        <CircularProgress
                                                            size={18}
                                                        />
                                                    ) : (
                                                        <SaveIcon fontSize="small" />
                                                    )}
                                                </IconButton>
                                            )}
                                        </Stack>
                                    </TableCell>
                                )}
                                <TableCell align="center">
                                    <IconButton
                                        color="error"
                                        size="small"
                                        onClick={() =>
                                            handleDeleteItem(item.id)
                                        }
                                        disabled={disabled || isRowSaving}
                                    >
                                        {deletingItem[item.id] ? (
                                            <CircularProgress size={18} />
                                        ) : (
                                            <DeleteIcon fontSize="small" />
                                        )}
                                    </IconButton>
                                </TableCell>
                            </TableRow>
                        )
                    })}
                    {items.length === 0 && (
                        <TableRow>
                            <TableCell colSpan={6} align="center">
                                <Typography
                                    variant="body2"
                                    color="text.secondary"
                                >
                                    Nema stavki na popisu.
                                </Typography>
                            </TableCell>
                        </TableRow>
                    )}
                </TableBody>
            </Table>
        </TableContainer>
    )
}

const PopisDetailsPage = () => {
    const router = useRouter()
    const { id: routeId } = router.query

    const initialId = useMemo(() => {
        if (!routeId) return '—'
        if (Array.isArray(routeId)) return routeId[0]
        return routeId
    }, [routeId])

    const [document, setDocument] = useState({
        id: initialId,
        date: null,
        type: '',
        status: '',
        items: [],
        komercijalnoPopisBrDok: '',
        popisDate: null,
        komercijalnoNarudzbenicaBrDok: '',
        narudzbenicaDate: null,
    })

    const [loading, setLoading] = useState(true)
    const [isStornirajLoading, setIsStornirajLoading] = useState(false)
    const [isStatusMutating, setIsStatusMutating] = useState(false)
    const [isBulkDialogOpen, setIsBulkDialogOpen] = useState(false)
    const [isBulkActionLoading, setIsBulkActionLoading] = useState(false)

    // Helper to reload popis data (used after bulk add)
    const reloadPopis = useCallback(async () => {
        if (!document.id) return
        await officeApi
            .get(`/popisi/${document.id}`)
            .then((response) => {
                const data = response.data
                setDocument({
                    id: data.id,
                    date: data.date,
                    type: data.type,
                    status: data.status,
                    items: (data.items || []).map((item) => ({
                        id: item.id,
                        name: item.naziv,
                        unit: item.unit,
                        popisanaKolicina: item.popisanaKolicina,
                        narucenaKolicina: item.narucenaKolicina ?? 0,
                    })),
                    komercijalnoPopisBrDok: data.komercijalnoPopisBrDok,
                    popisDate: data.popisDate,
                    komercijalnoNarudzbenicaBrDok:
                        data.komercijalnoNarudzbenicaBrDok,
                    narudzbenicaDate: data.narudzbenicaDate,
                })
            })
            .catch((err) => {
                handleApiError(err)
            })
    }, [document.id])

    const handleBulkAddPromet = useCallback(async () => {
        if (!document.id) return
        try {
            setIsBulkActionLoading(true)
            await officeApi.post(`/popisi/${document.id}/masovno-dodavanje`, {
                actionType: 0,
            })
            toast.success('Dodavanje stavki sa prometom uspešno.')
            setIsBulkDialogOpen(false)
            await reloadPopis()
        } catch (err) {
            handleApiError(err)
        } finally {
            setIsBulkActionLoading(false)
        }
    }, [document.id, reloadPopis])

    const handleBulkAddPocetno = useCallback(async () => {
        if (!document.id) return
        try {
            setIsBulkActionLoading(true)
            await officeApi.post(`/popisi/${document.id}/masovno-dodavanje`, {
                actionType: 1,
            })
            toast.success(
                'Dodavanje robe početnog stanja (količina > 0) uspešno.'
            )
            setIsBulkDialogOpen(false)
            await reloadPopis()
        } catch (err) {
            handleApiError(err)
        } finally {
            setIsBulkActionLoading(false)
        }
    }, [document.id, reloadPopis])

    useEffect(() => {
        if (!initialId || initialId === '—') {
            return
        }

        const abortController = new AbortController()

        const fetchPopis = async () => {
            setLoading(true)
            await officeApi
                .get(`/popisi/${initialId}`, {
                    signal: abortController.signal,
                })
                .then((response) => {
                    const data = response.data

                    setDocument({
                        id: data.id,
                        date: data.date,
                        type: data.type,
                        status: data.status,
                        items: (data.items || []).map((item) => ({
                            id: item.id,
                            name: item.naziv,
                            unit: item.unit,
                            popisanaKolicina: item.popisanaKolicina,
                            narucenaKolicina: item.narucenaKolicina ?? 0,
                        })),
                        komercijalnoPopisBrDok: data.komercijalnoPopisBrDok,
                        popisDate: data.popisDate,
                        komercijalnoNarudzbenicaBrDok:
                            data.komercijalnoNarudzbenicaBrDok,
                        narudzbenicaDate: data.narudzbenicaDate,
                    })
                })
                .catch((err) => {
                    if (
                        err?.name === 'CanceledError' ||
                        err?.name === 'AbortError'
                    ) {
                        return
                    }
                    handleApiError(err)
                })
                .finally(() => {
                    setLoading(false)
                })
        }

        fetchPopis()

        return () => {
            abortController.abort()
        }
    }, [initialId])

    const isDisabled = useMemo(
        () => document.status === 1 || document.status === 2,
        [document.status]
    )

    const handleEditPopisanaKolicina = useCallback((itemId, newQuantity) => {
        setDocument((prev) => ({
            ...prev,
            items: prev.items.map((item) =>
                item.id === itemId
                    ? { ...item, popisanaKolicina: newQuantity }
                    : item
            ),
        }))
    }, [])

    const handleEditNarucenaKolicina = useCallback((itemId, newQuantity) => {
        setDocument((prev) => ({
            ...prev,
            items: prev.items.map((item) =>
                item.id === itemId
                    ? { ...item, narucenaKolicina: newQuantity }
                    : item
            ),
        }))
    }, [])

    const handleRemoveItem = useCallback((itemId) => {
        setDocument((prev) => ({
            ...prev,
            items: prev.items.filter((item) => item.id !== itemId),
        }))
    }, [])

    const handleAddItem = useCallback(() => {
        if (!document.id) return

        const channelName = `host-popis-new-item-` + Date.now()
        const addWindow = window.open(
            `/izbor-robe?channel=${channelName}&noLayout=true`,
            `newWindow`,
            `popup,width=800,height=600`
        )

        if (!addWindow) {
            toast.error(`Nije moguće otvoriti novi prozor`)
            return
        }

        const channel = new BroadcastChannel(channelName)
        channel.onmessage = (event) => {
            switch (event.data.type) {
                case 'select-roba': {
                    const { robaId, kolicina } = event.data.payload
                    setIsStatusMutating(true)
                    officeApi
                        .post(`/popisi/${document.id}/items`, {
                            RobaId: robaId,
                            Kolicina: kolicina,
                        })
                        .then((response) => {
                            const created = response?.data
                            if (!created) {
                                return
                            }

                            setDocument((prev) => ({
                                ...prev,
                                items: [
                                    ...prev.items,
                                    {
                                        id: created.id,
                                        name: created.naziv,
                                        unit: created.unit,
                                        popisanaKolicina:
                                            created.popisanaKolicina,
                                        narucenaKolicina:
                                            created.narucenaKolicina ?? 0,
                                    },
                                ],
                            }))
                        })
                        .catch((err) => {
                            handleApiError(err)
                        })
                        .finally(() => {
                            setIsStatusMutating(false)
                        })
                    break
                }
                default:
                    toast.error(`Nepoznata akcija`)
                    break
            }
        }
    }, [document.id])

    const handleStorniraj = useCallback(async () => {
        if (!document.id) return
        setIsStornirajLoading(true)

        await officeApi
            .post(`/popisi/${document.id}/storniraj`)
            .then(() => {
                // After successful storniraj, update status to 2 (Otkazan)
                setDocument((prev) => ({ ...prev, status: 2 }))
            })
            .catch((err) => {
                handleApiError(err)
            })
            .finally(() => {
                setIsStornirajLoading(false)
            })
    }, [document.id])

    const handleToggleStatus = useCallback(async () => {
        if (!document.id) return
        if (document.status !== 0 && document.status !== 1) return

        const nextStatus = document.status === 1 ? 0 : 1

        setIsStatusMutating(true)

        await officeApi
            .put(`/popisi/${document.id}/status`, {
                Status: nextStatus,
            })
            .then(() => {
                setDocument((prev) => ({ ...prev, status: nextStatus }))
            })
            .catch((err) => {
                handleApiError(err)
            })
            .finally(() => {
                setIsStatusMutating(false)
            })
    }, [document.id, document.status])

    // Confirm "storniraj" dialog state
    const [isStornirajDialogOpen, setIsStornirajDialogOpen] = useState(false)
    const openStornirajDialog = () => setIsStornirajDialogOpen(true)
    const closeStornirajDialog = () => setIsStornirajDialogOpen(false)

    if (loading) {
        return (
            <Container maxWidth="lg" sx={{ py: 3 }}>
                <Stack spacing={2}>
                    <Typography>Učitavanje...</Typography>
                    <LinearProgress />
                </Stack>
            </Container>
        )
    }

    const isAnyDisabled = isDisabled || isStornirajLoading || isStatusMutating

    const showNarucenaColumn = document.type !== 0

    return (
        <Container maxWidth="lg" sx={{ py: 3 }}>
            <ConfirmDialog
                isOpen={isStornirajDialogOpen}
                onCancel={closeStornirajDialog}
                onConfirm={() => {
                    handleStorniraj()
                    closeStornirajDialog()
                }}
                message={
                    'Da li sigurno zelite stornirati ovaj popis? Storniranjem popis ce biti zadrzan ovde, ali ce sve stavke iz komercijalnog biti obrisane. Nastavi?'
                }
            />
            {(isStornirajLoading || isStatusMutating) && (
                <LinearProgress sx={{ mb: 2 }} />
            )}
            <Box sx={{ mb: 2 }}>
                <Button
                    component={NextLink}
                    href="/roba/popis"
                    variant="contained"
                    // Nazad button must remain enabled even when other actions are disabled
                    disabled={false}
                >
                    Nazad
                </Button>
            </Box>
            <PopisHeader
                document={document}
                disabled={isStornirajLoading || isStatusMutating}
                isMutating={isStornirajLoading}
                onToggleLock={handleToggleStatus}
                isStatusMutating={isStatusMutating}
                actions={[
                    {
                        key: 'storniraj',
                        label: 'Storniraj',
                        color: 'error',
                        variant: 'contained',
                        onClick: openStornirajDialog,
                        disabled: isDisabled,
                    },
                ]}
            />

            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    alignItems: 'center',
                    mb: 1.5,
                }}
            >
                <Typography variant="h6">Stavke popisa</Typography>
                <Stack direction="row" spacing={1}>
                    <Button
                        variant="contained"
                        onClick={() => setIsBulkDialogOpen(true)}
                        disabled={isAnyDisabled}
                    >
                        Masovno dodavanje stavki
                    </Button>
                    <Button
                        variant="contained"
                        startIcon={<AddIcon />}
                        onClick={handleAddItem}
                        disabled={isAnyDisabled}
                    >
                        Dodaj stavku
                    </Button>
                </Stack>
            </Box>

            <PopisItemsTable
                items={document.items}
                onEditPopisanaKolicina={handleEditPopisanaKolicina}
                onEditNarucenaKolicina={handleEditNarucenaKolicina}
                onRemoveItem={handleRemoveItem}
                disabled={isAnyDisabled}
                documentId={document.id}
                showNarucenaColumn={showNarucenaColumn}
            />

            {/* Masovno dodavanje stavki dialog */}
            <Dialog
                open={isBulkDialogOpen}
                onClose={() => setIsBulkDialogOpen(false)}
                fullWidth
                maxWidth="sm"
            >
                <DialogTitle>Masovno dodavanje stavki</DialogTitle>
                <DialogContent>
                    {isBulkActionLoading && <LinearProgress sx={{ mb: 2 }} />}
                    <Stack spacing={2} sx={{ mt: 1 }}>
                        <Button
                            variant="contained"
                            onClick={handleBulkAddPromet}
                            disabled={isAnyDisabled || isBulkActionLoading}
                        >
                            Stavke sa prometom
                        </Button>
                        <Button
                            variant="contained"
                            onClick={handleBulkAddPocetno}
                            disabled={isAnyDisabled || isBulkActionLoading}
                        >
                            Roba početnog stanja sa količinom većom od 0
                        </Button>
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={() => setIsBulkDialogOpen(false)}
                        disabled={isBulkActionLoading}
                    >
                        Zatvori
                    </Button>
                </DialogActions>
            </Dialog>
        </Container>
    )
}

export default PopisDetailsPage
