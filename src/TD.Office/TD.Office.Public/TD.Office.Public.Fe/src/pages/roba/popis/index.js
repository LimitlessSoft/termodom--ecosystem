import React, { useEffect, useState } from 'react'
import {
    Box,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    FormControl,
    FormHelperText,
    InputLabel,
    LinearProgress,
    MenuItem,
    Paper,
    Select,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TablePagination,
    TableRow,
    Typography,
} from '@mui/material'
import { DatePicker } from '@mui/x-date-pickers'
import dayjs from 'dayjs'
import { useRouter } from 'next/router'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { MagaciniDropdown } from '../../../widgets'
import { useUser } from '../../../hooks/useUserHook'

const PopisRobePage = () => {
    const currentUser = useUser()
    const router = useRouter()

    // Rows loaded from backend
    const [rows, setRows] = useState([])

    // Server-side pagination state
    const [page, setPage] = useState(0) // zero-based page index
    const [rowsPerPage, setRowsPerPage] = useState(25)
    const [total, setTotal] = useState(0)

    const [loading, setLoading] = useState(false)

    // Date filters stored as local Date objects (start/end of day)
    const [fromLocal, setFromLocal] = useState(
        dayjs(new Date()).startOf('day').toDate()
    )
    const [toLocal, setToLocal] = useState(
        dayjs(new Date()).endOf('day').toDate()
    )

    // Magacin filter
    // undefined => dropdown not initialized yet (avoid fetching for admins until ready)
    // null => "svi magacini" (all warehouses)
    const [selectedMagacinId, setSelectedMagacinId] = useState(undefined)

    // Dialog state for creating new popis
    const [createOpen, setCreateOpen] = useState(false)
    const [createType, setCreateType] = useState('')
    const [createSubmitting, setCreateSubmitting] = useState(false)
    const [createError, setCreateError] = useState('')

    const mockedMagacinName = 'Magacin: Centralni lager' // mocked current magacin label

    const isBusy = loading || createSubmitting

    const toUtcStartOfDayIso = (localDate) => {
        if (!localDate) return null
        const d = dayjs(localDate).utc().startOf('day').toISOString()
        return d
    }

    const toUtcEndOfDayIso = (localDate) => {
        if (!localDate) return null
        const d = dayjs(localDate).utc().endOf('day').toISOString()
        return d
    }

    const fetchPopisi = async (pageIndex, pageSize) => {
        try {
            setLoading(true)

            const params = {
                currentPage: pageIndex + 1,
                pageSize,
                FromDate: fromLocal,
                ToDate: toLocal,
                MagacinId: selectedMagacinId,
            }

            const response = await officeApi.get('/popisi', {
                params,
            })

            const data = response.data

            // API shape: { payload: PopisDto[], pagination: { totalCount, ... } }
            const items = data.payload ?? []

            const mappedRows = items.map((item) => ({
                id: item.id ?? item.Id,
                brojDokumenta: item.brojDokumenta ?? item.BrojDokumenta,
                // Datum is DateTime on BE; expect ISO string and display date part
                datum: (item.datum ?? item.Datum ?? '')
                    .toString()
                    .substring(0, 10),
                magacin: item.magacin ?? item.Magacin,
                status: item.status ?? item.Status ?? null,
            }))

            setRows(mappedRows)
            setTotal(
                data.pagination?.totalCount ??
                    data.totalCount ??
                    data.total ??
                    mappedRows.length
            )
        } catch (error) {
            handleApiError(error)
            setRows([])
            setTotal(0)
        } finally {
            setLoading(false)
        }
    }

    const isAdmin = !!currentUser?.data?.isAdmin
    const userLoaded = currentUser?.data != null

    useEffect(() => {
        // Wait until user data is loaded to avoid pre-user (non-admin) fetch
        if (!userLoaded) return
        // If admin, wait for MagaciniDropdown to initialize and set selectedMagacinId
        if (isAdmin && selectedMagacinId === undefined) return

        fetchPopisi(page, rowsPerPage)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [page, rowsPerPage, fromLocal, toLocal, selectedMagacinId, isAdmin, userLoaded])

    const handleChangePage = (event, newPage) => {
        setPage(newPage)
    }

    const handleChangeRowsPerPage = (event) => {
        const newRowsPerPage = parseInt(event.target.value, 10)
        setRowsPerPage(newRowsPerPage)
        setPage(0)
    }

    const handleFromDateChange = (value) => {
        setFromLocal(
            dayjs(value ?? new Date())
                .startOf('day')
                .toDate()
        )
        setPage(0)
    }

    const handleToDateChange = (value) => {
        setToLocal(
            dayjs(value ?? new Date())
                .endOf('day')
                .toDate()
        )
        setPage(0)
    }

    const handleOpenCreate = () => {
        setCreateError('')
        setCreateType('')
        setCreateOpen(true)
    }

    const handleCloseCreate = () => {
        if (createSubmitting) return
        setCreateOpen(false)
    }

    const handleCreateConfirm = async () => {
        if (createType === '' || createType === null) {
            setCreateError('Tip je obavezan')
            return
        }

        try {
            setCreateSubmitting(true)
            setCreateError('')

            await officeApi.post('/popisi', {
                type: Number(createType),
            })

            setCreateOpen(false)
            // Refresh current page after creating new popis
            fetchPopisi(page, rowsPerPage)
        } catch (error) {
            handleApiError(error)
        } finally {
            setCreateSubmitting(false)
        }
    }

    const getRowBorderColor = (status) => {
        switch (status) {
            case 0:
                return 'success.main' // green
            case 1:
                return 'error.main' // red
            case 2:
                return 'purple' // purple
            default:
                return 'transparent'
        }
    }

    return (
        <Box p={2}>
            <Paper elevation={1}>
                <Box
                    p={2}
                    display="flex"
                    justifyContent="space-between"
                    alignItems="center"
                >
                    <Typography variant="h6" gutterBottom>
                        Popis robe
                    </Typography>
                    <Button
                        variant="contained"
                        color="primary"
                        onClick={handleOpenCreate}
                        disabled={isBusy}
                    >
                        +
                    </Button>
                </Box>

                {/* Filters */}
                <Box px={2} pb={1} display="flex" gap={2} flexWrap="wrap">
                    <DatePicker
                        label="Datum od"
                        value={dayjs(fromLocal)}
                        onChange={handleFromDateChange}
                        disabled={loading}
                    />
                    <DatePicker
                        label="Datum do"
                        value={dayjs(toLocal)}
                        onChange={handleToDateChange}
                        disabled={loading}
                    />
                    {currentUser?.data?.isAdmin && (
                        <MagaciniDropdown
                            allowSviMagaciniFilter
                            excluteContainingStar
                            types={2}
                            onChange={(e) => {
                                setSelectedMagacinId(e)
                                setPage(0)
                            }}
                            disabled={loading}
                        />
                    )}
                </Box>

                <TableContainer>
                    {loading && <LinearProgress />}
                    <Table size="small">
                        <TableHead>
                            <TableRow>
                                <TableCell>Broj Dokumenta</TableCell>
                                <TableCell>Datum</TableCell>
                                <TableCell>Magacin</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {rows.map((row) => (
                                <TableRow
                                    key={row.id}
                                    hover
                                    onClick={() => {
                                        if (loading) return
                                        router.push(
                                            `/roba/popis/${row.brojDokumenta}`
                                        )
                                    }}
                                    sx={{
                                        borderLeft: '4px solid',
                                        borderLeftColor: getRowBorderColor(
                                            row.status
                                        ),
                                    }}
                                    style={{
                                        textDecoration: 'none',
                                        color: 'inherit',
                                        cursor: loading ? 'default' : 'pointer',
                                    }}
                                >
                                    <TableCell>{row.brojDokumenta}</TableCell>
                                    <TableCell>{row.datum}</TableCell>
                                    <TableCell>{row.magacin}</TableCell>
                                </TableRow>
                            ))}
                            {!loading && rows.length === 0 && (
                                <TableRow>
                                    <TableCell colSpan={3} align="center">
                                        Nema podataka
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>

                <TablePagination
                    component="div"
                    rowsPerPageOptions={[5, 10, 25]}
                    count={total}
                    rowsPerPage={rowsPerPage}
                    page={page}
                    labelRowsPerPage="Redova po strani"
                    onPageChange={(event, newPage) => {
                        if (loading) return
                        handleChangePage(event, newPage)
                    }}
                    onRowsPerPageChange={(event) => {
                        if (loading) return
                        handleChangeRowsPerPage(event)
                    }}
                />
            </Paper>

            <Dialog
                open={createOpen}
                onClose={handleCloseCreate}
                fullWidth
                maxWidth="xs"
            >
                <DialogTitle>Novi popis</DialogTitle>
                <DialogContent>
                    <Box mb={2}>
                        <Typography variant="body2" color="textSecondary">
                            {mockedMagacinName}
                        </Typography>
                    </Box>
                    <FormControl
                        fullWidth
                        margin="normal"
                        error={!!createError}
                    >
                        <InputLabel id="popis-type-label">
                            Tip popisa
                        </InputLabel>
                        <Select
                            labelId="popis-type-label"
                            value={createType}
                            label="Tip popisa"
                            onChange={(e) => setCreateType(e.target.value)}
                            disabled={createSubmitting}
                            variant="outlined"
                        >
                            <MenuItem value={0}>Vanredni</MenuItem>
                            <MenuItem value={1}>Za nabavku</MenuItem>
                        </Select>
                        {createError && (
                            <FormHelperText>{createError}</FormHelperText>
                        )}
                    </FormControl>
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={handleCloseCreate}
                        disabled={createSubmitting}
                    >
                        Otkaži
                    </Button>
                    <Button
                        onClick={handleCreateConfirm}
                        variant="contained"
                        color="primary"
                        disabled={createSubmitting}
                    >
                        Potvrdi
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    )
}

export default PopisRobePage
