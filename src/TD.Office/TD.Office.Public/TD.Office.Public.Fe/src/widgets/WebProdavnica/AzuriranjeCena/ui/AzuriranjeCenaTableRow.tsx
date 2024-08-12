import { IAzuriranjeCenaTableRowProps } from '../models/IAzuriranjeCenaTableRowProps'
import { AzuriranjeCenaUslovFormiranjaCell } from './AzuriranjeCenaUslovFormiranjaCell'
import { CircularProgress, TableCell, TableRow, styled } from '@mui/material'
import { AzuriranjeCenaPovezanCell } from './AzuriranjeCenaPovezanCell'
import { ReactNode, useState } from 'react'
import { officeApi } from '@/apis/officeApi'

interface ICellProperties {
    children: number | string | ReactNode
    error?: boolean
}

const Cell = (props: ICellProperties) => {
    const CellStyled = styled(TableCell)(
        ({ theme }) => `
            text-align: center;

            ${
                props.error
                    ? `
                    background-color: ${theme.palette.error.main};
                    color: ${theme.palette.error.contrastText};
                `
                    : null
            }
        `
    )

    return (
        <CellStyled>
            {typeof props.children == 'number'
                ? props.children.toFixed(2)
                : props.children}
        </CellStyled>
    )
}

export const AzuriranjeCenaTableRow = (
    props: IAzuriranjeCenaTableRowProps
): JSX.Element => {
    const [data, setData] = useState(props.data)
    const [isDataLoading, setIsDataLoading] = useState(false)

    const isMinOsnovaError = () => {
        if (data.uslovFormiranjaWebCeneType == 0)
            return (
                data.minWebOsnova.toFixed(2) !=
                (
                    (data.nabavnaCenaKomercijalno *
                        (100 + data.uslovFormiranjaWebCeneModifikator)) /
                    100
                ).toFixed(2)
            )

        if (data.uslovFormiranjaWebCeneType == 1) {
            return (
                data.minWebOsnova.toFixed(2) !=
                (
                    (data.prodajnaCenaKomercijalno *
                        (100 - data.uslovFormiranjaWebCeneModifikator)) /
                    100
                ).toFixed(2)
            )
        }

        if (data.uslovFormiranjaWebCeneType == 2) return data.minWebOsnova != 0

        return true
    }

    const isMaxOsnovaError = () => {
        console.log(data)
        if (data.uslovFormiranjaWebCeneType != 2)
            return (
                data.maxWebOsnova.toFixed(2) !=
                data.prodajnaCenaKomercijalno.toFixed(2)
            )

        if (data.uslovFormiranjaWebCeneType == 2) return data.maxWebOsnova != 0

        return true
    }

    const reloadRowData = () => {
        setIsDataLoading(true)

        officeApi
            .get(`/web-azuriranje-cena?id=${data.id}`)
            .then((response: any) => {
                setData(response.data[0])
            })
            .finally(() => {
                setIsDataLoading(false)
            })
    }

    return (
        <TableRow key={data.naziv}>
            <Cell>{data.naziv}</Cell>
            <Cell error={isMinOsnovaError()}>
                {isDataLoading ? (
                    <CircularProgress size={`1em`} />
                ) : (
                    data.minWebOsnova
                )}
            </Cell>
            <Cell error={isMaxOsnovaError()}>
                {isDataLoading ? (
                    <CircularProgress size={`1em`} />
                ) : (
                    data.maxWebOsnova
                )}
            </Cell>
            <Cell>
                {isDataLoading ? (
                    <CircularProgress size={`1em`} />
                ) : (
                    data.nabavnaCenaKomercijalno
                )}
            </Cell>
            <Cell>
                {isDataLoading ? (
                    <CircularProgress size={`1em`} />
                ) : (
                    data.prodajnaCenaKomercijalno
                )}
            </Cell>
            <Cell>
                <AzuriranjeCenaUslovFormiranjaCell
                    disabled={isDataLoading}
                    data={data}
                    onSuccessUpdate={() => {
                        reloadRowData()
                    }}
                    onErrorUpdate={() => {}}
                />
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
                    onErrorUpdate={() => {}}
                />
            </Cell>
        </TableRow>
    )
}
