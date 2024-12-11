import { PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS } from '@/constants'
import { DataGrid } from '@mui/x-data-grid'
import { renderCell, generateColumns } from '../helpers/PartneriHelpers'
import { Box, MenuItem, TextField } from '@mui/material'
import { toast } from 'react-toastify'
import { PartneriKomercijalnoIFinansijskoTableKomentarCell } from './PartneriKomercijalnoIFinansijskoTableKomentarCell'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '@/constants'
import { useState } from 'react'

export const PartneriKomercijalnoIFinansijskoTable = ({
    partnersData,
    partnersRequest,
    tolerance,
    statuses,
}) => {
    const [isStatusUpdating, setIsStatusUpdating] = useState(false)

    const columns = [
        {
            field: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.PPID.toLowerCase(),
            headerName:
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                    .PPID,
            width: 100,
            renderCell,
            hideable: false,
            filterable: false,
        },
        {
            field: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.STATUS.toLowerCase(),
            headerName:
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                    .STATUS,
            width: 150,
            renderCell: (param) => {
                return (
                    <Box>
                        <TextField
                            select
                            disabled={isStatusUpdating}
                            value={param.value}
                            onChange={() => {
                                setIsStatusUpdating(true)

                                officeApi
                                    .put(
                                        ENDPOINTS_CONSTANTS.PARTNERS.PUT_KOMERCIJALNO_I_FINANSIJSKO_DATA_STATUS(
                                            param.id
                                        ),
                                        {
                                            ppid: param.id,
                                            statusId: param.value,
                                        }
                                    )
                                    .then((response) => {
                                        toast.success('Status uspešno ažuriran')
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setIsStatusUpdating(false)
                                    })
                            }}
                        >
                            {statuses.map((status) => (
                                <MenuItem key={status.id} value={status.id}>
                                    {status.name}
                                </MenuItem>
                            ))}
                        </TextField>
                    </Box>
                )
            },
            hideable: false,
            filterable: false,
        },
        {
            field: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.KOMENTAR.toLowerCase(),
            headerName:
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                    .KOMENTAR,
            width: 100,
            renderCell: (param) => (
                <Box>
                    <PartneriKomercijalnoIFinansijskoTableKomentarCell
                        param={param}
                    />
                </Box>
            ),
            hideable: false,
            filterable: false,
        },
        {
            field: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.NAZIV.toLowerCase(),
            headerName:
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                    .NAZIV,
            width: 150,
            renderCell,
            hideable: false,
            filterable: false,
        },
        ...partnersRequest.years
            .toSorted((a, b) => b - a)
            .map((year) => generateColumns(year, tolerance))
            .flat(),
    ]

    return (
        <DataGrid
            getRowId={(row) => row.ppid}
            hideFooterPagination={true}
            disableColumnSelector
            disableRowSelectionOnClick
            columns={columns}
            rows={partnersData.payload}
            rowCount={partnersData.pagination.totalCount}
            checkboxSelection={false}
            getRowHeight={() => 'auto'}
        />
    )
}
