import { IAzuriranjeCenaTableRowProps } from "../models/IAzuriranjeCenaTableRowProps"
import { AzuriranjeCenaUslovFormiranjaCell } from "./AzuriranjeCenaUslovFormiranjaCell"
import { CircularProgress, TableCell, TableRow, styled } from "@mui/material"
import { AzuriranjeCenaPovezanCell } from "./AzuriranjeCenaPovezanCell"
import { ReactNode, useState } from "react"

interface ICellProperties {
    children: number | string | ReactNode,
    error?: boolean
}

const Cell = (props: ICellProperties): JSX.Element => {
    
    const CellStyled = styled(TableCell)(
        ({ theme }) => `
            text-align: center;

            ${
                props.error ? `
                    background-color: ${theme.palette.error.main};
                    color: ${theme.palette.error.contrastText};
                ` : null
            }
        `)

    return <CellStyled>
        { typeof(props.children) == 'number' ? props.children.toFixed(2) : props.children }</CellStyled>
}

export const AzuriranjeCenaTableRow = (props: IAzuriranjeCenaTableRowProps): JSX.Element => {

    const [data, setData] = useState(props.data)
    const [isDataLoading, setIsDataLoading] = useState(false)

    const isMinOsnovaError = () => {
        if(data.uslovFormiranjaWebCeneType != 0)
            return false

        return data.minWebOsnova.toFixed(2) != (data.nabavnaCenaKomercijalno * (100 + data.uslovFormiranjaWebCeneModifikator) / 100).toFixed(2)
    }

    const isMaxOsnovaError = () => {
        if(data.uslovFormiranjaWebCeneType != 0)
            return false

        return data.maxWebOsnova.toFixed(2) != data.prodajnaCenaKomercijalno.toFixed(2)
    }

    const reloadRowData = () => {
        setIsDataLoading(true)
        fetchApi(ApiBase.Main, '/web-azuriranje-cena?id=' + data.id)
            .then((response) => {
                setData(response[0])
            })
            .finally(() => {
                setIsDataLoading(false)
            })
    }

    return (
        <TableRow key={data.naziv}>
            <Cell>{data.naziv}</Cell>
            <Cell error={isMinOsnovaError()}>{isDataLoading ? <CircularProgress size={`1em`} /> : data.minWebOsnova}</Cell>
            <Cell error={isMaxOsnovaError()}>{isDataLoading ? <CircularProgress size={`1em`} /> : data.maxWebOsnova}</Cell>
            <Cell>{isDataLoading ? <CircularProgress size={`1em`} /> : data.nabavnaCenaKomercijalno}</Cell>
            <Cell>{isDataLoading ? <CircularProgress size={`1em`} /> : data.prodajnaCenaKomercijalno}</Cell>
            <Cell>
                <AzuriranjeCenaUslovFormiranjaCell
                    disabled={isDataLoading}
                    data={data}
                    onSuccessUpdate={() => {
                        reloadRowData()
                    }}
                    onErrorUpdate={() => {
                        
                    }} />
            </Cell>
            <Cell>{data.platinumCena}</Cell>
            <Cell>{data.goldCena}</Cell>
            <Cell>{data.silverCena}</Cell>
            <Cell>{data.ironCena}</Cell>
            <Cell>
                <AzuriranjeCenaPovezanCell
                    disabled={isDataLoading}
                    data={data}
                    onSuccessUpdate={() => {
                    reloadRowData()
                }}
                onErrorUpdate={() => {
                    
                }} />
            </Cell>
        </TableRow>
    )
}