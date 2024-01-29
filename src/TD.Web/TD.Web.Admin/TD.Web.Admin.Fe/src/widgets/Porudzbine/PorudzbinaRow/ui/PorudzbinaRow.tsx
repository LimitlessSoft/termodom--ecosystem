import { formatNumber } from "@/app/helpers/numberHelpers"
import { mainTheme } from "@/app/theme"
import { TableCell, TableRow, Typography, styled } from "@mui/material"
import { toast } from "react-toastify"

const PorudzbinaRowStyled = styled(TableRow)(
    ({ theme }) => `
        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.grey[100]};
        }
    `
)

export const PorudzbinaRow = (): JSX.Element => {
    return (
        <PorudzbinaRowStyled
            onClick={() => {
                toast.error(`Ova funkcionalnost još uvek nije implementirana`)
            }}>
            <TableCell>7CBBAC32</TableCell>
            <TableCell>01.02.2024.</TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: mainTheme.palette.warning.main,
                        fontWeight: `600`
                    }}>
                    Obrađuje se
                </Typography>
            </TableCell>
            <TableCell>Aleksa Ristic</TableCell>
            <TableCell>{formatNumber(27859.57)}</TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: mainTheme.palette.success.main,
                        fontWeight: `600`
                    }}>
                    {formatNumber(2859.24)}
                </Typography>
            </TableCell>
        </PorudzbinaRowStyled>
    )
}