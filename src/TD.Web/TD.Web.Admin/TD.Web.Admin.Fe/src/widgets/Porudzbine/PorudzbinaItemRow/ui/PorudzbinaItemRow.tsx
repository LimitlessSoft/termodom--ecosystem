import { formatNumber } from "@/app/helpers/numberHelpers"
import { TableCell, TableRow, styled } from "@mui/material"
import { toast } from "react-toastify"

const PorudzbinaItemRowStyled = styled(TableRow)(
    ({ theme }) => `
        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.grey[100]};
        }
    `
)

export const PorudzbinaItemRow = (): JSX.Element => {

    return (
        <PorudzbinaItemRowStyled
            onClick={() => {
                toast.success(`Not implemented yet!`)
            }}>
            <TableCell>Proizvod name</TableCell>
            <TableCell>{formatNumber(321)}</TableCell>
            <TableCell>{formatNumber(1221)}</TableCell>
            <TableCell>{formatNumber(1234124)}</TableCell>
            <TableCell>{formatNumber(12)}%</TableCell>
        </PorudzbinaItemRowStyled>
    )
}