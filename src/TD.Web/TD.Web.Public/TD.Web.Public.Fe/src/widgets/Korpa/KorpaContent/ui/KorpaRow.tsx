import { TableCell, TableRow } from "@mui/material"
import { IKorpaRowProps } from "../interfaces/IKorpaRowProps"

export const KorpaRow = (props: IKorpaRowProps): JSX.Element => {

    const item = props.item
    
    return (
        <TableRow>
            <TableCell>{item.name}</TableCell>
            <TableCell>{item.quantity}</TableCell>
            <TableCell>{item.price}</TableCell>
            <TableCell>{item.price + (item.price * item.vat / 100)}</TableCell>
            <TableCell>UKLONI</TableCell>
        </TableRow>
    )
}