import { DataGrid } from '@mui/x-data-grid'
import { useState } from 'react'
import { Box, IconButton } from '@mui/material'
import { ArrowCircleRight } from '@mui/icons-material'
import { useRouter } from 'next/router'

export const ProracunTable = () => {
    const router = useRouter()

    const [columns, setColumns] = useState([
        {
            field: 'id',
            headerName: 'Broj',
            type: 'number',
            width: 110,
        },
        {
            field: 'datum',
            headerName: 'Datum',
            type: 'string',
            width: 110,
        },
        {
            field: 'vrednostBezPdv',
            headerName: 'Vrednost bez PDV',
            type: 'number',
            width: 150,
        },
        {
            field: 'vrednostSaPdv',
            headerName: 'Vrednost sa PDV',
            type: 'number',
            width: 150,
        },
        {
            field: 'referent',
            headerName: 'Referent',
            type: 'string',
            width: 150,
        },
        {
            field: 'komercijalnoDokument',
            headerName: 'Komercijalno dokument',
            type: 'string',
            width: 100,
        },
        {
            field: 'actions',
            headerName: 'Akcije',
            width: 200,
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
    const [rows, setRows] = useState([
        {
            id: 1,
            datum: '07.08.1999',
            vrednostBezPdv: 1000000,
            vrednostSaPdv: 1200000,
            referent: 'Marko Markovic',
            komercijalnoDokument: '32 - 1658',
        },
        {
            id: 2,
            datum: '07.08.1999',
            vrednostBezPdv: 1000,
            vrednostSaPdv: 1200,
            referent: 'Marko Markovic',
            komercijalnoDokument: '32 - 1658',
        },
    ])

    const handleRowDoubleClick = (row) => {
        router.push(`/proracun/${row.id}`)
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
                rows={rows}
                checkboxSelection={false}
            />
        </Box>
    )
}
