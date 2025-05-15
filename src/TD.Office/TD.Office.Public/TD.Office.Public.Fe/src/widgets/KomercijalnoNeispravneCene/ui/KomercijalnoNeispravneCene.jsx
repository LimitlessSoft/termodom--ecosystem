import {
    Alert,
    Box,
    Button,
    CircularProgress,
    LinearProgress,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../constants'
import { DataGrid } from '@mui/x-data-grid'
import { Refresh } from '@mui/icons-material'
import Grid2 from '@mui/material/Unstable_Grid2'
import dayjs from 'dayjs'

export const KomercijalnoNeispravneCene = () => {
    const columns = [
        { field: 'baza', headerName: 'Baza' },
        { field: 'magacinId', headerName: 'Magacin ID' },
        { field: 'robaId', headerName: 'Roba ID' },
        { field: 'opis', headerName: 'Opis', width: 500 },
    ]
    const [data, setData] = useState()
    const [status, setStatus] = useState()
    const [lastRun, setLastRun] = useState()
    const [pagination, setPagination] = useState({
        pageSize: 10,
    })

    const reloadData = () => {
        reloadLastRun()
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.IZVESTAJI
                    .GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA
            )
            .then((res) => setData(res.data.items))
            .catch(handleApiError)
    }

    const reloadStatus = () => {
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.IZVESTAJI
                    .GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA_STATUS
            )
            .then((res) => setStatus(res.data))
            .catch(handleApiError)
    }

    const reloadLastRun = () => {
        officeApi
            .get(
                ENDPOINTS_CONSTANTS.IZVESTAJI
                    .GET_IZVESTAJ_NEISPRAVNIH_CENA_U_MAGACINIMA_LAST_RUN
            )
            .then((res) => setLastRun(res.data))
            .catch(handleApiError)
    }

    useEffect(() => {
        reloadStatus()
    }, [])

    useEffect(() => {
        if (status !== 'Idle') return
        reloadData()
    }, [status])

    if (!status) return <LinearProgress />
    if (status !== 'Idle') {
        return (
            <Alert severity={`info`} variant={`filled`}>
                Generisanje izveštaja je u toku. Molimo Vas sačekajte.
            </Alert>
        )
    }

    if (!data) return <LinearProgress />

    return (
        <Box>
            <Grid2 container p={1} spacing={2} alignItems={`center`}>
                <Grid2 xs={9}>
                    <Alert severity={`warning`} variant={`filled`}>
                        U tabeli su iskazane cene bez poreza!
                    </Alert>
                </Grid2>
                <Grid2 xs={3}>
                    <Button
                        endIcon={<Refresh />}
                        variant={`contained`}
                        onClick={() => {
                            setData(null)
                            setStatus('InProgress')
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
                </Grid2>
                <Grid2>
                    {lastRun && (
                        <Typography variant={`h6`}>
                            Izvestaj od:{' '}
                            {dayjs(lastRun + 'Z').format('DD.MM.YYYY HH:mm:ss')}
                        </Typography>
                    )}
                    {!lastRun && <CircularProgress />}
                </Grid2>
            </Grid2>
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
