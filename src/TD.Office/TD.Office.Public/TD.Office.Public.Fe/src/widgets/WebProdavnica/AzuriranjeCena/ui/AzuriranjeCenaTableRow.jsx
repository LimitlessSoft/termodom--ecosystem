import { AzuriranjeCenaUslovFormiranjaCell } from './AzuriranjeCenaUslovFormiranjaCell'
import {
    CircularProgress,
    styled,
    TableCell,
    TableRow,
    Typography,
} from '@mui/material'
import { AzuriranjeCenaPovezanCell } from './AzuriranjeCenaPovezanCell'
import { useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { formatNumber } from '@/helpers/numberHelpers'

const Cell = (props) => {
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

export const AzuriranjeCenaTableRow = (props) => {
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
    const getDiscount = (price, discountedPrice) => {
        return formatNumber(((price - discountedPrice) / price) * 100)
    }
    const isMaxOsnovaError = () => {
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
            .then((response) => {
                setData(response.data[0])
            })
            .catch((err) => handleApiError(err))
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
            <Cell>
                <Typography>{formatNumber(data.platinumCena)}</Typography>
                <DiscountLabel
                    discount={getDiscount(
                        data.prodajnaCenaKomercijalno,
                        data.platinumCena
                    )}
                />
            </Cell>
            <Cell>
                <Typography>{formatNumber(data.goldCena)}</Typography>
                <DiscountLabel
                    discount={getDiscount(
                        data.prodajnaCenaKomercijalno,
                        data.goldCena
                    )}
                />
            </Cell>
            <Cell>
                <Typography>{formatNumber(data.silverCena)}</Typography>
                <DiscountLabel
                    discount={getDiscount(
                        data.prodajnaCenaKomercijalno,
                        data.silverCena
                    )}
                />
            </Cell>
            <Cell>
                <Typography>{formatNumber(data.ironCena)}</Typography>
                <DiscountLabel
                    discount={getDiscount(
                        data.prodajnaCenaKomercijalno,
                        data.ironCena
                    )}
                />
            </Cell>
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

const DiscountLabel = ({ discount }) => {
    return (
        <Typography
            sx={{
                color: discount - 19.98 > 0 ? `red` : `black`,
            }}
        >
            -{discount}%
        </Typography>
    )
}
