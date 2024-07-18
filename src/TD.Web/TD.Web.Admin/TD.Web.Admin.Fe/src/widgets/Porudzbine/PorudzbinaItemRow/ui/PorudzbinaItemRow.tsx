import { IPorudzbinaItemRowProps } from '../models/IPorudzbinaItemRowProps'
import { TableCell, TableRow, styled } from '@mui/material'
import { formatNumber } from '@/helpers/numberHelpers'
import { toast } from 'react-toastify'

const PorudzbinaItemRowStyled = styled(TableRow)(
    ({ theme }) => `
        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.grey[100]};
        }
    `
)

export const PorudzbinaItemRow = (
    props: IPorudzbinaItemRowProps
): JSX.Element => {
    return (
        <PorudzbinaItemRowStyled
            onClick={() => {
                toast.success(`Not implemented yet!`)
            }}
        >
            <TableCell>{props.item.name}</TableCell>
            <TableCell>{formatNumber(props.item.quantity)}</TableCell>
            <TableCell>{formatNumber(props.item.priceWithVAT)}</TableCell>
            <TableCell>{formatNumber(props.item.valueWithVAT)}</TableCell>
            <TableCell>{formatNumber(props.item.discount)}%</TableCell>
        </PorudzbinaItemRowStyled>
    )
}
