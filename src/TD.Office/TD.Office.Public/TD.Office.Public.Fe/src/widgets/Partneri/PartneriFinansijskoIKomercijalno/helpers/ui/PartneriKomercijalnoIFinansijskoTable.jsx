import { PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS } from '@/constants'
import { DataGrid } from '@mui/x-data-grid'
import { renderCell, generateColumns } from '../PartneriHelpers'

function PartneriKomercijalnoIFinansijskoTable({
    partnersData,
    partnersRequest,
    pagination,
    onPaginationChange,
    tolerance,
}) {
    const columns = [
        {
            field: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.PPID.toLowerCase(),
            headerName:
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                    .PPID,
            width: 150,
            renderCell,
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
        },
        ...partnersRequest.years
            .toSorted((a, b) => b - a)
            .map((year) => generateColumns(year, tolerance))
            .flat(),
    ]

    return (
        <DataGrid
            getRowId={(row) => row.ppid}
            pageSizeOptions={
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.PAGE_SIZE_OPTIONS
            }
            paginationModel={pagination}
            onPaginationModelChange={onPaginationChange}
            disableColumnSelector
            columns={columns}
            rows={partnersData}
            initialState={{
                pagination: {
                    paginationModel: pagination,
                },
            }}
            checkboxSelection={false}
            getRowHeight={() => 'auto'}
        />
    )
}

export default PartneriKomercijalnoIFinansijskoTable
