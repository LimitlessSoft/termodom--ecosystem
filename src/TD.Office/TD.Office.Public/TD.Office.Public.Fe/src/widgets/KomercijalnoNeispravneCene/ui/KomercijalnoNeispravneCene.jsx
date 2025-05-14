import { Box, LinearProgress } from '@mui/material'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { DataGrid } from '@mui/x-data-grid'

export const KomercijalnoNeispravneCene = () => {
    const columns = [
        { field: 'baza', headerName: 'Baza' },
        { field: 'magacinId', headerName: 'Magacin ID' },
        { field: 'robaId', headerName: 'Roba ID' },
        { field: 'opis', headerName: 'Opis', width: 500 },
    ]
    const [data, setData] = useState()
    const [pagination, setPagination] = useState({
        pageSize: 10,
    })

    useEffect(() => {
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.IZVESTAJI
                    .GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA
            )
            .then((res) => setData(res.data.items))
            .catch(handleApiError)
    }, [])

    if (!data) return <LinearProgress />

    return (
        <Box>
            <DataGrid
                initialState={{
                    pagination: {
                        paginationModel: pagination,
                    },
                }}
                pagination={pagination}
                onPaginationModelChange={setPagination}
                columns={columns}
                rows={data}
                getRowId={() => {
                    return Math.random()
                }}
            />
        </Box>
    )
}
