import { PorudzbinaRow } from '@/widgets/Porudzbine/PorudzbinaRow/ui/PorudzbinaRow'
import {
    Button,
    CircularProgress,
    Grid,
    Paper,
    Stack,
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
import { adminApi } from '@/apis/adminApi'

const KorisnikPorudzbine = (): JSX.Element => {
    const [userId, setUserId] = useState<number | undefined>(undefined)
    const [orders, setOrders] = useState<any[] | undefined>(undefined)
    const router = useRouter()

    useEffect(() => {
        if (router == null) return

        const userId = router.query.userId
        if (userId) {
            setUserId(Number(userId))
        }
    }, [router])

    useEffect(() => {
        if (userId == null) return

        adminApi
            .get(
                `/orders?userId=${userId}&pageSize=50&currentPage=1&SortColumn=Date&SortDirection=Descending`
            )
            .then((response) => {
                setOrders(response.data)
            })
    }, [userId])

    return (
        <Grid m={2}>
            <Stack direction={`row`} m={2}>
                <Button
                    variant={`contained`}
                    onClick={() => {
                        router.back()
                    }}
                >
                    Nazad
                </Button>
            </Stack>

            {orders === undefined && <CircularProgress />}
            {orders !== undefined && orders.length === 0 && (
                <Typography>Nema porudžbina</Typography>
            )}
            {orders !== undefined && orders.length > 0 && (
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
