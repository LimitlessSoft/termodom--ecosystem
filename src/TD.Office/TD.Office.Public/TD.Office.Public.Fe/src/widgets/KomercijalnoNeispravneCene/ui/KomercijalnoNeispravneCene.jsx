import { Box, Button, IconButton, LinearProgress, Stack } from '@mui/material'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { DataGrid } from '@mui/x-data-grid'
import {
    Refresh,
    RefreshOutlined,
    RefreshRounded,
    RefreshSharp,
    RefreshTwoTone,
} from '@mui/icons-material'

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

    const reloadData = () => {
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.IZVESTAJI
                    .GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA
            )
            .then((res) => setData(res.data.items))
            .catch(handleApiError)
    }

    useEffect(() => {
        reloadData()
    }, [])

    if (!data) return <LinearProgress />

    return (
        <Box>
            <Stack direction={`row`} justifyContent={`end`} p={1}>
                <Button
                    endIcon={<Refresh />}
                    variant={`contained`}
                    onClick={() => {
                        setData(null)
                        officeApi
                            .post(
                                ENDPOINTS_CONSTANTS.IZVESTAJI
                                    .OSVEZI_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA
                            )
                            .then((_) => reloadData())
                            .catch(handleApiError)
                    }}
                >
                    Osveži
                </Button>
            </Stack>
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
