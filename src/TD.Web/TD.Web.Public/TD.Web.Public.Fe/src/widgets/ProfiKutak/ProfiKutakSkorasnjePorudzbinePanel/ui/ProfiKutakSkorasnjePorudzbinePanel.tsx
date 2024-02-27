import { LinearProgress, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material"
import { ProfiKutakPanelBase } from "../../ProfiKutakSkorasnjePorudzbinePanelBase"
import { formatNumber } from "@/app/helpers/numberHelpers"
import { mainTheme } from "@/app/theme"
import { useEffect, useState } from "react"
import { ApiBase, fetchApi } from "@/app/api"
import moment from "moment"
import { ResponsiveTypography } from "@/widgets/Responsive"

export const ProfiKutakSkorasnjePorudzbinePanel = (): JSX.Element => {

    const [orders, setOrders] = useState<any | null>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/orders?status=1&status=2&status=3&status=4&status=5&SortColumn=Date&SortDirection=1`)
        .then((res) => {
            setOrders(res)
        })
    }, [])

    return (
        orders == null ?
            <LinearProgress /> :
            <ProfiKutakPanelBase title={`Skorašnje porudžbine`}>
                <TableContainer component={Paper}>
                    <Table sx={{ width: `100%` }} aria-label='Korpa'>
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
                            {
                                orders.map((order: any) => {
                                    return(
                                        <TableRow key={order.oneTimeHash}>
                                            <TableCell>
                                                <ResponsiveTypography>
                                                    {order.oneTimeHash.substring(0, 8)}
                                                </ResponsiveTypography>
                                            </TableCell>
                                            <TableCell>
                                                <ResponsiveTypography>
                                                    {moment(order.oneTimeHash.date).format("D.MM.yyyy.")}
                                                </ResponsiveTypography>
                                            </TableCell>
                                            <TableCell>
                                                <ResponsiveTypography
                                                    sx={{
                                                        color: ["Čeka obradu", "Obrađuje se"].indexOf(order.status) >= 0 ?
                                                            mainTheme.palette.warning.main :
                                                            mainTheme.palette.success.main,
                                                        fontWeight: `600`
                                                    }}>
                                                    {order.status}
                                                </ResponsiveTypography>
                                            </TableCell>
                                            <TableCell>
                                                    <ResponsiveTypography
                                                        sx={{
                                                            textAlign: `right`
                                                        }}>
                                                        {formatNumber(order.valueWithVAT)}
                                                    </ResponsiveTypography>
                                                </TableCell>
                                            <TableCell>
                                                <ResponsiveTypography
                                                    sx={{
                                                        color: mainTheme.palette.success.main,
                                                        fontWeight: `600`,
                                                        textAlign: `right`
                                                    }}>
                                                    {formatNumber(order.discountValue)}
                                                </ResponsiveTypography>
                                            </TableCell>
                                        </TableRow>
                                    )
                                })
                            }
                        </TableBody>
                    </Table>
                </TableContainer>
            </ProfiKutakPanelBase>
    )
}