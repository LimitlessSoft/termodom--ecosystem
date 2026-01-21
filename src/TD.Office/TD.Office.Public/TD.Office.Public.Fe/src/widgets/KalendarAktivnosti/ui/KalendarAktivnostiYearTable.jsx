import { useMemo } from 'react'
import { Box, Chip, Paper, Typography } from '@mui/material'
import { StripedDataGrid } from '@/widgets/StripedDataGrid'
import { ODSUSTVO_CONSTANTS } from '@/constants/odsustvo/odsustvoConstants'
import { format } from 'date-fns'

const STATUS_COLORS = {
    PENDING: '#ffc107',
    APPROVED: '#4caf50',
    REALIZED: '#2196f3',
}

const getOdsustvoColor = (odsustvo) => {
    const isPending = odsustvo.status === ODSUSTVO_CONSTANTS.STATUS.CEKA_ODOBRENJE
    const isRealized = odsustvo.realizovanoKorisnik && odsustvo.realizovanoOdobravac

    if (isPending) {
        return STATUS_COLORS.PENDING
    }
    if (isRealized) {
        return STATUS_COLORS.REALIZED
    }
    return STATUS_COLORS.APPROVED
}

const getStatusLabel = (odsustvo) => {
    const isPending = odsustvo.status === ODSUSTVO_CONSTANTS.STATUS.CEKA_ODOBRENJE
    const isRealized = odsustvo.realizovanoKorisnik && odsustvo.realizovanoOdobravac

    if (isPending) {
        return 'Ceka odobrenje'
    }
    if (isRealized) {
        return 'Realizovano'
    }
    return 'Odobreno'
}

const formatDate = (dateString) => {
    if (!dateString) return ''
    return format(new Date(dateString), 'dd.MM.yyyy')
}

const calculateDays = (datumOd, datumDo) => {
    if (!datumOd || !datumDo) return 0

    const startDate = new Date(datumOd)
    const endDate = new Date(datumDo)
    startDate.setHours(0, 0, 0, 0)
    endDate.setHours(0, 0, 0, 0)

    let count = 0
    const currentDate = new Date(startDate)

    while (currentDate <= endDate) {
        if (currentDate.getDay() !== 0) {
            count++
        }
        currentDate.setDate(currentDate.getDate() + 1)
    }

    return count
}

export const KalendarAktivnostiYearTable = ({ data, loading, onRowClick, userName }) => {
    const summary = useMemo(() => {
        if (!data || data.length === 0) return { byType: {}, total: 0 }

        const byType = {}
        let total = 0

        data.forEach((item) => {
            const days = calculateDays(item.datumOd, item.datumDo)
            const tipNaziv = item.tipOdsustvaNaziv || 'Nepoznato'

            if (!byType[tipNaziv]) {
                byType[tipNaziv] = 0
            }
            byType[tipNaziv] += days
            total += days
        })

        return { byType, total }
    }, [data])

    const columns = [
        {
            field: 'tipOdsustvaNaziv',
            headerName: 'Tip odsustva',
            width: 200,
            hideable: false,
            filterable: false,
        },
        {
            field: 'datumOd',
            headerName: 'Datum od',
            width: 120,
            hideable: false,
            filterable: false,
            renderCell: (params) => formatDate(params.value),
        },
        {
            field: 'datumDo',
            headerName: 'Datum do',
            width: 120,
            hideable: false,
            filterable: false,
            renderCell: (params) => formatDate(params.value),
        },
        {
            field: 'brojDana',
            headerName: 'Broj dana',
            width: 100,
            hideable: false,
            filterable: false,
            renderCell: (params) => calculateDays(params.row.datumOd, params.row.datumDo),
        },
        {
            field: 'status',
            headerName: 'Status',
            width: 150,
            hideable: false,
            filterable: false,
            renderCell: (params) => {
                const color = getOdsustvoColor(params.row)
                const label = getStatusLabel(params.row)
                return (
                    <Chip
                        label={label}
                        size="small"
                        sx={{
                            bgcolor: color,
                            color: 'white',
                        }}
                    />
                )
            },
        },
        {
            field: 'komentar',
            headerName: 'Komentar',
            flex: 1,
            minWidth: 200,
            hideable: false,
            filterable: false,
        },
    ]

    return (
        <Box sx={{ mt: 3 }}>
            <Typography variant="h6" sx={{ mb: 2 }}>
                Godisnji pregled za: {userName}
            </Typography>
            <StripedDataGrid
                autoHeight
                loading={loading}
                noRowsMessage="Nema odsustva za izabranog korisnika u ovoj godini"
                getRowId={(row) => row.id}
                hideFooterPagination
                disableColumnSelector
                columns={columns}
                rows={data || []}
                checkboxSelection={false}
                onRowClick={(params) => onRowClick && onRowClick(params.row)}
                sx={{
                    '& .MuiDataGrid-row': {
                        cursor: 'pointer',
                    },
                }}
            />

            {data && data.length > 0 && (
                <Paper sx={{ mt: 2, p: 2, bgcolor: 'grey.50' }}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 'bold', mb: 1 }}>
                        Sumarna statistika
                    </Typography>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 3 }}>
                        {Object.entries(summary.byType).map(([tip, days]) => (
                            <Box key={tip}>
                                <Typography variant="body2" color="text.secondary">
                                    {tip}
                                </Typography>
                                <Typography variant="h6">
                                    {days} {days === 1 ? 'dan' : 'dana'}
                                </Typography>
                            </Box>
                        ))}
                        <Box sx={{ borderLeft: '2px solid', borderColor: 'primary.main', pl: 2 }}>
                            <Typography variant="body2" color="text.secondary">
                                Ukupno
                            </Typography>
                            <Typography variant="h6" color="primary.main" sx={{ fontWeight: 'bold' }}>
                                {summary.total} {summary.total === 1 ? 'dan' : 'dana'}
                            </Typography>
                        </Box>
                    </Box>
                </Paper>
            )}
        </Box>
    )
}
