import {
    LinearProgress,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TablePagination,
    TableRow,
} from '@mui/material'
import { ProfiKutakPanelBase } from '../../ProfiKutakSkorasnjePorudzbinePanelBase'
import { formatNumber } from '@/app/helpers/numberHelpers'
import { mainTheme } from '@/app/theme'
import { useEffect, useState } from 'react'
import moment from 'moment'
import { ResponsiveTypography } from '@/widgets/Responsive'
import { useRouter } from 'next/router'
import { handleApiError, webApi } from '@/api/webApi'

export const ProfiKutakSkorasnjePorudzbinePanel = () => {
    const [orders, setOrders] = useState(null)
    const [page, setPage] = useState(0)
    const [rowsPerPage, setRowsPerPage] = useState(
        mainTheme.defaultPagination?.default ?? 10
    )
    const [totalCount, setTotalCount] = useState(null)
    const router = useRouter()

    useEffect(() => {
        webApi
            .get(
                `/orders?status=1&status=2&status=3&status=4&status=5&pageSize=${rowsPerPage}&currentPage=${
                    page + 1
                }&SortColumn=Date&SortDirection=1`
            )
            .then((res) => {
                setOrders(res.data.payload)
                const pagination =
                    res.data.pagination || res.data?.meta?.pagination
                if (pagination && typeof pagination.totalCount === 'number') {
                    setTotalCount(pagination.totalCount)
                } else {
                    setTotalCount(null)
                }
            })
            .catch((err) => handleApiError(err))
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [page, rowsPerPage])

    return !orders ? (
        <LinearProgress />
    ) : (
        <ProfiKutakPanelBase title={`Skorašnje porudžbine`}>
            <TableContainer component={Paper}>
                <Table sx={{ width: `100%` }} aria-label="Korpa">
                    <TableHead>
                        <TableRow>
                            <TableCell>Broj</TableCell>
                            <TableCell>Datum</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Vrednost sa PDV</TableCell>
                            <TableCell>Ušteda</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {orders.map((order) => {
                            return (
                                <TableRow
                                    key={order.oneTimeHash}
                                    onClick={() => {
                                        router.push(
                                            `/porudzbine/${order.oneTimeHash}`
                                        )
                                    }}
                                >
                                    <TableCell>
                                        <ResponsiveTypography>
                                            {order.oneTimeHash.substring(0, 8)}
                                        </ResponsiveTypography>
                                    </TableCell>
                                    <TableCell>
                                        <ResponsiveTypography>
                                            {moment(
                                                order.oneTimeHash.date
                                            ).format('D.MM.yyyy.')}
                                        </ResponsiveTypography>
                                    </TableCell>
                                    <TableCell>
                                        <ResponsiveTypography
                                            sx={{
                                                color:
                                                    [
                                                        'Čeka obradu',
                                                        'Obrađuje se',
                                                    ].indexOf(order.status) >= 0
                                                        ? mainTheme.palette
                                                              .warning.main
                                                        : mainTheme.palette
                                                              .success.main,
                                                fontWeight: `600`,
                                            }}
                                        >
                                            {order.status}
                                        </ResponsiveTypography>
                                    </TableCell>
                                    <TableCell>
                                        <ResponsiveTypography
                                            sx={{
                                                textAlign: `right`,
                                            }}
                                        >
                                            {formatNumber(order.valueWithVAT)}
                                        </ResponsiveTypography>
                                    </TableCell>
                                    <TableCell>
                                        <ResponsiveTypography
                                            sx={{
                                                color: mainTheme.palette.success
                                                    .main,
                                                fontWeight: `600`,
                                                textAlign: `right`,
                                            }}
                                        >
                                            {formatNumber(order.discountValue)}
                                        </ResponsiveTypography>
                                    </TableCell>
                                </TableRow>
                            )
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
            <TablePagination
                component="div"
                rowsPerPageOptions={mainTheme.defaultPagination.options}
                count={totalCount ?? -1}
                rowsPerPage={rowsPerPage}
                page={page}
                onPageChange={(_, newPage) => setPage(newPage)}
                onRowsPerPageChange={(e) => {
                    const newSize = parseInt(e.target.value, 10)
                    setRowsPerPage(newSize)
                    setPage(0)
                }}
            />
        </ProfiKutakPanelBase>
    )
}
