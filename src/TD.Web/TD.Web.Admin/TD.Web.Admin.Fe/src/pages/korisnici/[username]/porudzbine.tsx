import { PorudzbinaRow } from '@/widgets/Porudzbine/PorudzbinaRow/ui/PorudzbinaRow'
import {
    CircularProgress,
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material'
import { useRouter } from 'next/router'
import { useEffect, useState } from 'react'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { LSBackButton } from 'ls-core-next'

const KorisnikPorudzbine = (): JSX.Element => {
    const [userId, setUserId] = useState<number | undefined>(undefined)
    const [orders, setOrders] = useState<any[] | undefined>(undefined)
    const router = useRouter()

    useEffect(() => {
        if (!router) return

        const userId = router.query.userId
        if (userId) {
            setUserId(Number(userId))
        }
    }, [router])

    useEffect(() => {
        if (!userId) return

        adminApi
            .get(
                `/orders?userId=${userId}&pageSize=50&currentPage=1&SortColumn=Date&SortDirection=Descending&Status=1&Status=2&Status=3&Status=4&Status=5`
            )
            .then((response) => {
                setOrders(response.data.payload)
            })
            .catch((err) => handleApiError(err))
    }, [userId])

    return (
        <Grid m={2}>
            <LSBackButton />
            {!orders && <CircularProgress />}
            {orders && orders.length === 0 && (
                <Typography>Nema porudžbina</Typography>
            )}
            {orders && orders.length > 0 && (
                <TableContainer component={Paper}>
                    <Table sx={{ width: `100%` }} aria-label="Korpa">
                        <TableHead>
                            <TableRow>
                                <TableCell>Broj</TableCell>
                                <TableCell>Datum i vreme</TableCell>
                                <TableCell>Status</TableCell>
                                <TableCell>Korisnik</TableCell>
                                <TableCell>Vrednost sa PDV</TableCell>
                                <TableCell>Ušteda</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {orders.map((porudzbina: any) => (
                                <PorudzbinaRow
                                    key={porudzbina.id}
                                    porudzbina={porudzbina}
                                />
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            )}
        </Grid>
    )
}

export default KorisnikPorudzbine
