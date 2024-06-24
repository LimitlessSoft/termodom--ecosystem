import { useEffect, useState } from "react"
import { DashboardPanel } from "./DashboardPanel"
import { ApiBase, fetchApi } from "@/app/api"
import { CircularProgress, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material"

export const DashboardProductsViewsPanel = (): JSX.Element => {

    const [data, setData] = useState<any[] | undefined>(undefined)
    useEffect(() => {

        const dateFrom = new Date()
        dateFrom.setDate(dateFrom.getDate() - 7)

        fetchApi(ApiBase.Main, `/products-statistics?DateFromUtc=${dateFrom.toISOString()}&DateToUtc=${new Date().toISOString()}`)
        .then((response: any) => {
            response.json().then((response: any) => {
                setData(response.views.items.toSorted((x: any, y: any) => y.views - x.views).slice(0, 10))
            })
        })
    }, [])

    return (
        <DashboardPanel
            title={`Najposećeniji proizvodi ove nedelje`}>
                {
                    data == null ? <CircularProgress /> :
                    <Table>
                        <TableContainer>
                            <TableHead>
                                <TableCell>Proizvod</TableCell>
                                <TableCell>Poseta</TableCell>
                            </TableHead>
                            <TableBody>
                                {
                                    data.map((item: any, index: number) => (
                                        <TableRow key={index}>
                                            <TableCell sx={{ p: 0 }}>{item.name}</TableCell>
                                            <TableCell sx={{ p: 0, textAlign: `center` }}>{item.views}</TableCell>
                                        </TableRow>
                                    ))
                                }
                            </TableBody>
                        </TableContainer>
                    </Table>
                }
        </DashboardPanel>
    )
}