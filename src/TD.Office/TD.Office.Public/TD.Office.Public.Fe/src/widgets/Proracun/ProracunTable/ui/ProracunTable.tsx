import { DataGrid } from '@mui/x-data-grid'
import { useState } from 'react'
import {
    Autocomplete,
    Box,
    CircularProgress,
    IconButton,
    Stack,
    TextField,
} from '@mui/material'
import { ArrowCircleRight } from '@mui/icons-material'
import { toast } from 'react-toastify'
import { DatePicker } from '@mui/x-date-pickers'
import { useRouter } from 'next/router'
import dayjs from 'dayjs'

export const ProracunTable = () => {
    const router = useRouter()

    const [columns, setColumns] = useState<any>([
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
            renderCell: (row: any) => {
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
    const [rows, setRows] = useState<any>([
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

    const handleRowDoubleClick = (row: any) => {
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
