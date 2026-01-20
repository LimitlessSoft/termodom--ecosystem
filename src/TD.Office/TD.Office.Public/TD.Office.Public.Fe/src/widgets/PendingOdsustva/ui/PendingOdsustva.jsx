import { useEffect, useState } from 'react'
import {
    CircularProgress,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
    Button,
} from '@mui/material'
import { useRouter } from 'next/router'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS_CONSTANTS, URL_CONSTANTS } from '@/constants'
import { mainTheme } from '@/themes'
import { format } from 'date-fns'

const formatDate = (dateString) => {
    if (!dateString) return ''
    return format(new Date(dateString), 'dd.MM.yyyy')
}

export const PendingOdsustva = () => {
    const router = useRouter()
    const [data, setData] = useState(undefined)

    useEffect(() => {
        officeApi
            .get(ENDPOINTS_CONSTANTS.ODSUSTVO.PENDING)
            .then((r) => {
                setData(r.data)
            })
            .catch(handleApiError)
    }, [])

    const handleGoToCalendar = () => {
        router.push(URL_CONSTANTS.KORISNICI.KALENDAR_AKTIVNOSTI)
    }

    return (
        <Paper sx={{ p: 2 }}>
            <Stack>
                <Typography variant="h6">Zahtevi za odsustvo na cekanju</Typography>
                {data === undefined && <CircularProgress />}
                {data !== undefined && data.length === 0 && (
                    <Typography color={mainTheme.palette.success.main}>
                        Nema zahteva za odsustvo na cekanju
                    </Typography>
                )}
                {data !== undefined && data.length > 0 && (
                    <>
                        <TableContainer component={Paper} sx={{ mt: 1 }}>
                            <Table size="small">
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Korisnik</TableCell>
                                        <TableCell>Tip odsustva</TableCell>
                                        <TableCell>Od</TableCell>
                                        <TableCell>Do</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {data.map((row) => (
                                        <TableRow key={row.id}>
                                            <TableCell>{row.userNickname}</TableCell>
                                            <TableCell>{row.tipOdsustvaNaziv}</TableCell>
                                            <TableCell>{formatDate(row.datumOd)}</TableCell>
                                            <TableCell>{formatDate(row.datumDo)}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                        <Button
                            variant="text"
                            size="small"
                            onClick={handleGoToCalendar}
                            sx={{ mt: 1, alignSelf: 'flex-end' }}
                        >
                            Idi na kalendar aktivnosti
                        </Button>
                    </>
                )}
            </Stack>
        </Paper>
    )
}
