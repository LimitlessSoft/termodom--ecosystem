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
            headerClassName: 'sticky-header-left',
            cellClassName: 'sticky-cell-left',
        },
        {
            field: PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS.NAZIV.toLowerCase(),
            headerName:
                PARTNERI_FINANSIJSKO_I_KOMERCIJALNO_CONSTANTS.TABLE_HEAD_FIELDS
                    .NAZIV,
            width: 150,
            renderCell,
            hideable: false,
            headerClassName: 'sticky-header-left-second',
            cellClassName: 'sticky-cell-left-second',
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
            sx={{
                '& .MuiDataGrid-columnHeaders': {
                    position: 'sticky',
                    top: 0,
                    zIndex: 1000,
                    backgroundColor: 'white',
                },
                '& .sticky-header-left': {
                    position: 'sticky',
                    left: 0,
                    zIndex: 1001,
                    backgroundColor: 'white',
                    borderRight: '1px solid #ddd',
                },
                '& .sticky-header-left-second': {
                    position: 'sticky',
                    left: 150,
                    zIndex: 1001,
                    backgroundColor: 'white',
                    borderRight: '1px solid #ddd',
                },
                '& .sticky-cell-left': {
                    position: 'sticky',
                    left: 0,
                    zIndex: 1000,
                    backgroundColor: 'white',
                },
                '& .sticky-cell-left-second': {
                    position: 'sticky',
                    left: 150,
                    zIndex: 1000,
                    backgroundColor: 'white',
                },
                '& .MuiDataGrid-cell': {
                    padding: '10px',
                },
            }}
            getRowHeight={() => 'auto'}
        />
    )
}

export default PartneriKomercijalnoIFinansijskoTable
