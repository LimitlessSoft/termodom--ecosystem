import { TableCell, TableRow, styled } from "@mui/material"
import { IAzuriranjeCenaTableRowProps } from "../models/IAzuriranjeCenaTableRowProps"
import { AzuriranjeCenaUslovFormiranjaCell } from "./AzuriranjeCenaUslovFormiranjaCell"
import { AzuriranjeCenaPovezanCell } from "./AzuriranjeCenaPovezanCell"
import { ReactNode } from "react"

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

    const data = props.data

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

    return (
        <TableRow key={data.naziv}>
            <Cell>{data.naziv}</Cell>
            <Cell error={isMinOsnovaError()}>{data.minWebOsnova}</Cell>
            <Cell error={isMaxOsnovaError()}>{data.maxWebOsnova}</Cell>
            <Cell>{data.nabavnaCenaKomercijalno}</Cell>
            <Cell>{data.prodajnaCenaKomercijalno}</Cell>
            <Cell>
                <AzuriranjeCenaUslovFormiranjaCell data={data} reloadData={props.reloadData} />
            </Cell>
            <Cell>{data.platinumCena}</Cell>
            <Cell>{data.goldCena}</Cell>
            <Cell>{data.silverCena}</Cell>
            <Cell>{data.ironCena}</Cell>
            <Cell>
                <AzuriranjeCenaPovezanCell data={data} reloadData={props.reloadData} />
            </Cell>
        </TableRow>
    )
}