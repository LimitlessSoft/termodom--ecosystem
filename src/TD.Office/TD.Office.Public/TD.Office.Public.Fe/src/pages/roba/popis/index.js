import React, { useEffect, useState } from 'react'
import {
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TablePagination,
    Typography,
    Box,
    LinearProgress,
    Button,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    FormHelperText,
} from '@mui/material'
import { useRouter } from 'next/router'
import { officeApi, handleApiError } from '@/apis/officeApi'

const PopisRobePage = () => {
    const router = useRouter()

    // Rows loaded from backend
    const [rows, setRows] = useState([])

    // Server-side pagination state
    const [page, setPage] = useState(0) // zero-based page index
    const [rowsPerPage, setRowsPerPage] = useState(25)
    const [total, setTotal] = useState(0)

    const [loading, setLoading] = useState(false)

    // Dialog state for creating new popis
    const [createOpen, setCreateOpen] = useState(false)
    const [createType, setCreateType] = useState('')
    const [createSubmitting, setCreateSubmitting] = useState(false)
    const [createError, setCreateError] = useState('')

    const mockedMagacinName = 'Magacin: Centralni lager' // mocked current magacin label

    const fetchPopisi = async (pageIndex, pageSize) => {
        try {
            setLoading(true)

            // Assuming BE expects 1-based page index; adjust if it's 0-based
            const response = await officeApi.get('/popisi', {
                params: {
                    currentPage: pageIndex + 1,
                    pageSize,
                },
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

    useEffect(() => {
        fetchPopisi(page, rowsPerPage)
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [page, rowsPerPage])

    const handleChangePage = (event, newPage) => {
        setPage(newPage)
    }

    const handleChangeRowsPerPage = (event) => {
        const newRowsPerPage = parseInt(event.target.value, 10)
        setRowsPerPage(newRowsPerPage)
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
                    >
                        +
                    </Button>
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
                                    onClick={() =>
                                        router.push(
                                            `/roba/popis/${row.brojDokumenta}`
                                        )
                                    }
                                    sx={{
                                        borderLeft: '4px solid',
                                        borderLeftColor: getRowBorderColor(
                                            row.status
                                        ),
                                    }}
                                    style={{
                                        textDecoration: 'none',
                                        color: 'inherit',
                                        cursor: 'pointer',
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
                    onPageChange={handleChangePage}
                    onRowsPerPageChange={handleChangeRowsPerPage}
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
