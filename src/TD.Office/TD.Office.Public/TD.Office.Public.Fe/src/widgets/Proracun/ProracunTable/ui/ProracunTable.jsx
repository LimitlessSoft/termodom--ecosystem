import { DataGrid } from '@mui/x-data-grid'
import { useEffect, useState } from 'react'
import { Box, CircularProgress, IconButton, Typography } from '@mui/material'
import { ArrowCircleRight, Block, Lock, LockOpen } from '@mui/icons-material'
import { useRouter } from 'next/router'
import moment from 'moment'
import { useZMagacini } from '@/zStore'
import { formatNumber } from '@/helpers/numberHelpers'

export const ProracunTable = ({
    data,
    magacini,
    pagination,
    setPagination,
}) => {
    const router = useRouter()

    const [columns, setColumns] = useState([
        {
            field: 'state',
            headerName: 'Stanje',
            type: 'string',
            width: 50,
            valueGetter: (params) => {
                return ''
            },
            renderCell: (row) => {
                return row.row.state === 0 ? (
                    <LockOpen color={`success`} />
                ) : (
                    <Lock color={`error`} />
                )
            },
        },
        {
            field: 'id',
            headerName: 'Broj',
            type: 'number',
            width: 110,
        },
        {
            field: 'createdAt',
            headerName: 'Datum',
            type: 'string',
            width: 150,
            valueGetter: (params) => {
                return moment(params.value + 'Z').format('DD.MM.YYYY')
            },
        },
        {
            field: 'magacinId',
            headerName: 'Magacin',
            type: 'number',
            flex: 1,
            minWidth: 150,
            renderCell: (row) => {
                return magacini?.find((m) => m.id === row.row.magacinId)?.name
            },
        },
        {
            field: 'type',
            headerName: 'Tip',
            type: 'string',
            width: 50,
        },
        {
            field: 'ukupnoBezPdv',
            headerName: 'Vrednost bez PDV',
            type: 'number',
            width: 150,
            valueGetter: (params) => {
                return formatNumber(params.value)
            },
        },
        // {
        //     field: 'vrednostSaPdv',
        //     headerName: 'Vrednost sa PDV',
        //     type: 'number',
        //     width: 150,
        // },
        {
            field: 'referent',
            headerName: 'Referent',
            type: 'string',
            width: 150,
        },
        {
            field: 'komercijalnoDokument',
            headerName: 'Komercijalno dokument',
            width: 130,
            renderCell: (row) => {
                return (
                    <Box textAlign={`center`} width={`100%`}>
                        {row.row.komercijalnoDokument === '' ? (
                            <Block />
                        ) : (
                            <Typography>
                                {row.row.komercijalnoDokument}
                            </Typography>
                        )}
                    </Box>
                )
            },
        },
        {
            field: 'actions',
            headerName: 'Akcije',
            width: 80,
            renderCell: (row) => {
                return (
                    <IconButton
                        color={`secondary`}
                        onClick={() => {
                            handleRowDoubleClick(row.row)
                        }}
                    >
                        <ArrowCircleRight />
                    </IconButton>
                )
            },
        },
    ])

    const handleRowDoubleClick = (row) => {
        router.push(`/proracun/${row.id}`)
    }

    if (data === undefined || columns === undefined) {
        return <CircularProgress />
    }

    return (
        <Box>
            <DataGrid
                sx={{
                    m: 2,
                    '& .MuiDataGrid-columnHeaderTitle': {
                        whiteSpace: 'normal',
                        lineHeight: 'normal',
                    },
                    '& .MuiDataGrid-columnHeader': {
                        // Forced to use important since overriding inline styles
                        height: 'unset !important',
                    },
                    '& .MuiDataGrid-columnHeaders': {
                        // Forced to use important since overriding inline styles
                        maxHeight: '168px !important',
                    },
                }}
                onRowDoubleClick={handleRowDoubleClick}
                disableRowSelectionOnClick={true}
                disableColumnSelector={true}
                columns={columns}
                rows={data.payload}
                initialState={{
                    pagination: {
                        paginationModel: pagination,
                    },
                }}
                paginationMode={`server`}
                pagination={pagination}
                onPaginationModelChange={setPagination}
                pageSizeOptions={[10, 50, 100]}
                checkboxSelection={false}
                rowCount={data.pagination.totalCount}
            />
        </Box>
    )
}
