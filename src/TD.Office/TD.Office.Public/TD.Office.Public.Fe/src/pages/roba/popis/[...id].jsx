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
import { DataGrid } from '@mui/x-data-grid'
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
import { ConfirmDialog, PopupBox } from '../../../widgets'
import { IzborRobeWidget } from '../../../widgets/IzborRobe/IzborRobeWidget'

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
        userName,
        magacinName,
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
                        {magacinName && (
                            <Typography variant="body2" color="text.secondary">
                                Magacin: {magacinName}
                            </Typography>
                        )}
                        {userName && (
                            <Typography variant="body2" color="text.secondary">
                                Operater: {userName}
                            </Typography>
                        )}
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
    const [paginationModel, setPaginationModel] = useState({
        pageSize: 25,
        page: 0,
    })
    const [filterText, setFilterText] = useState('')

    const filteredItems = useMemo(() => {
        if (!filterText) return items
        const lowerFilter = filterText.toLowerCase()
        return items.filter(
            (item) =>
                (item.id &&
                    String(item.id).toLowerCase().includes(lowerFilter)) ||
                (item.name && item.name.toLowerCase().includes(lowerFilter))
        )
    }, [items, filterText])

    const handlePopisanaChange = useCallback(
        (id, value) => {
            if (disabled) return
            let normalized = value.replace(',', '.')
            normalized = normalized.replace(/^0+(?=\d)/, '')
            if (
                normalized !== '' &&
                normalized !== '.' &&
                (Number.isNaN(Number(normalized)) || Number(normalized) < 0)
            )
                return
            setEditedPopisana((prev) => ({ ...prev, [id]: normalized }))

            const parsed = Number(normalized)
            if (!Number.isNaN(parsed)) {
                onEditPopisanaKolicina(id, parsed)
            }
        },
        [disabled, onEditPopisanaKolicina]
    )

    const handleNarucenaChange = useCallback(
        (id, value) => {
            if (disabled) return
            let normalized = value.replace(',', '.')
            normalized = normalized.replace(/^0+(?=\d)/, '')
            if (
                normalized !== '' &&
                normalized !== '.' &&
                (Number.isNaN(Number(normalized)) || Number(normalized) < 0)
            )
                return
            setEditedNarucena((prev) => ({ ...prev, [id]: normalized }))

            const parsed = Number(normalized)
            if (!Number.isNaN(parsed)) {
                onEditNarucenaKolicina(id, parsed)
            }
        },
        [disabled, onEditNarucenaKolicina]
    )

    const handleSavePopisana = useCallback(
        async (itemId) => {
            if (!documentId) return
            const rawValue = editedPopisana[itemId]
            if (rawValue == null) return
            const valueToSave = Number(rawValue)
            if (Number.isNaN(valueToSave)) return
            setSavingPopisana((prev) => ({ ...prev, [itemId]: true }))
            try {
                await officeApi.put(
                    `/popisi/${documentId}/items/${itemId}/popisana-kolicina`,
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
        },
        [documentId, editedPopisana]
    )

    const handleSaveNarucena = useCallback(
        async (itemId) => {
            if (!documentId) return
            const rawValue = editedNarucena[itemId]
            if (rawValue == null) return
            const valueToSave = Number(rawValue)
            if (Number.isNaN(valueToSave)) return
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
        },
        [documentId, editedNarucena]
    )

    const hasEditedPopisana = useCallback(
        (itemId) =>
            Object.prototype.hasOwnProperty.call(editedPopisana, itemId),
        [editedPopisana]
    )
    const hasEditedNarucena = useCallback(
        (itemId) =>
            Object.prototype.hasOwnProperty.call(editedNarucena, itemId),
        [editedNarucena]
    )

    const handleDeleteItem = useCallback(
        async (itemId) => {
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
        },
        [documentId, disabled, onRemoveItem]
    )

    const columns = useMemo(() => {
        const cols = [
            { field: 'id', headerName: 'ID', width: 100 },
            { field: 'name', headerName: 'Naziv', flex: 1, minWidth: 200 },
            { field: 'unit', headerName: 'JM', width: 80 },
            {
                field: 'popisanaKolicina',
                headerName: 'Popisana količina',
                width: 200,
                sortable: false,
                renderCell: (params) => {
                    const item = params.row
                    const editedPopisanaValue = editedPopisana[item.id]
                    const showSavePopisana = hasEditedPopisana(item.id)
                    const isSavePopisanaDisabled =
                        disabled || savingPopisana[item.id]
                    const isRowSaving =
                        Boolean(savingPopisana[item.id]) ||
                        Boolean(savingNarucena[item.id]) ||
                        Boolean(deletingItem[item.id])

                    return (
                        <Stack
                            direction="row"
                            spacing={1}
                            alignItems="center"
                            sx={{ width: '100%', height: '100%' }}
                        >
                            <TextField
                                size="small"
                                value={
                                    showSavePopisana
                                        ? editedPopisanaValue
                                        : (item.popisanaKolicina ?? 0)
                                }
                                onChange={(e) =>
                                    handlePopisanaChange(
                                        item.id,
                                        e.target.value
                                    )
                                }
                                inputProps={{ min: 0 }}
                                disabled={disabled || isRowSaving}
                                onKeyDown={(e) => e.stopPropagation()}
                            />
                            {showSavePopisana && (
                                <IconButton
                                    color="primary"
                                    size="small"
                                    onClick={(e) => {
                                        e.stopPropagation()
                                        handleSavePopisana(item.id)
                                    }}
                                    disabled={
                                        isSavePopisanaDisabled || isRowSaving
                                    }
                                >
                                    {savingPopisana[item.id] ? (
                                        <CircularProgress size={18} />
                                    ) : (
                                        <SaveIcon fontSize="small" />
                                    )}
                                </IconButton>
                            )}
                        </Stack>
                    )
                },
            },
        ]

        if (showNarucenaColumn) {
            cols.push({
                field: 'narucenaKolicina',
                headerName: 'Naručena količina',
                width: 200,
                sortable: false,
                renderCell: (params) => {
                    const item = params.row
                    const editedNarucenaValue = editedNarucena[item.id]
                    const showSaveNarucena = hasEditedNarucena(item.id)
                    const isSaveNarucenaDisabled =
                        disabled || savingNarucena[item.id]
                    const isRowSaving =
                        Boolean(savingPopisana[item.id]) ||
                        Boolean(savingNarucena[item.id]) ||
                        Boolean(deletingItem[item.id])

                    return (
                        <Stack
                            direction="row"
                            spacing={1}
                            alignItems="center"
                            sx={{ width: '100%', height: '100%' }}
                        >
                            <TextField
                                size="small"
                                value={
                                    showSaveNarucena
                                        ? editedNarucenaValue
                                        : (item.narucenaKolicina ?? 0)
                                }
                                onChange={(e) =>
                                    handleNarucenaChange(
                                        item.id,
                                        e.target.value
                                    )
                                }
                                inputProps={{ min: 0 }}
                                disabled={disabled || isRowSaving}
                                onKeyDown={(e) => e.stopPropagation()}
                            />
                            {showSaveNarucena && (
                                <IconButton
                                    color="primary"
                                    size="small"
                                    onClick={(e) => {
                                        e.stopPropagation()
                                        handleSaveNarucena(item.id)
                                    }}
                                    disabled={
                                        isSaveNarucenaDisabled || isRowSaving
                                    }
                                >
                                    {savingNarucena[item.id] ? (
                                        <CircularProgress size={18} />
                                    ) : (
                                        <SaveIcon fontSize="small" />
                                    )}
                                </IconButton>
                            )}
                        </Stack>
                    )
                },
            })
        }

        cols.push({
            field: 'actions',
            headerName: 'Akcije',
            width: 100,
            align: 'center',
            headerAlign: 'center',
            sortable: false,
            renderCell: (params) => {
                const item = params.row
                const isRowSaving =
                    Boolean(savingPopisana[item.id]) ||
                    Boolean(savingNarucena[item.id]) ||
                    Boolean(deletingItem[item.id])
                return (
                    <IconButton
                        color="error"
                        size="small"
                        onClick={(e) => {
                            e.stopPropagation()
                            handleDeleteItem(item.id)
                        }}
                        disabled={disabled || isRowSaving}
                    >
                        {deletingItem[item.id] ? (
                            <CircularProgress size={18} />
                        ) : (
                            <DeleteIcon fontSize="small" />
                        )}
                    </IconButton>
                )
            },
        })

        return cols
    }, [
        editedPopisana,
        editedNarucena,
        savingPopisana,
        savingNarucena,
        deletingItem,
        disabled,
        showNarucenaColumn,
        handlePopisanaChange,
        handleNarucenaChange,
        handleSavePopisana,
        handleSaveNarucena,
        handleDeleteItem,
        hasEditedPopisana,
        hasEditedNarucena,
    ])

    return (
        <Paper elevation={1} sx={{ width: '100%' }}>
            <Box sx={{ p: 2 }}>
                <TextField
                    fullWidth
                    size="small"
                    label="Pretraži (ID, Naziv)"
                    variant="outlined"
                    value={filterText}
                    onChange={(e) => setFilterText(e.target.value)}
                />
            </Box>
            <DataGrid
                rows={filteredItems}
                columns={columns}
                density="compact"
                disableRowSelectionOnClick
                getRowHeight={() => 'auto'}
                autoHeight
                paginationModel={paginationModel}
                onPaginationModelChange={setPaginationModel}
                pageSizeOptions={[10, 25, 50, 100]}
                sx={{
                    border: 0,
                    '& .MuiDataGrid-cell': {
                        py: 1,
                    },
                }}
            />
        </Paper>
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
    const [isAddItemPopupOpen, setIsAddItemPopupOpen] = useState(false)

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
                    userName: data.userName,
                    magacinName: data.magacinName,
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
                        userName: data.userName,
                        magacinName: data.magacinName,
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
        setIsAddItemPopupOpen((prev) => !prev)
    }, [])

    const handleSelectRoba = useCallback(
        (robaId, kolicina) => {
            if (!document.id) return
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
                                popisanaKolicina: created.popisanaKolicina,
                                narucenaKolicina: created.narucenaKolicina ?? 0,
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
        },
        [document.id]
    )

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

            {isAddItemPopupOpen && (
                <PopupBox
                    onClose={() => setIsAddItemPopupOpen(false)}
                    initialHeight={1}
                >
                    <IzborRobeWidget onSelectRoba={handleSelectRoba} />
                </PopupBox>
            )}
        </Container>
    )
}

export default PopisDetailsPage
