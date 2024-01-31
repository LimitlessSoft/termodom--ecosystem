import { formatNumber } from "@/app/helpers/numberHelpers"
import { mainTheme } from "@/app/theme"
import { LinearProgress, TableCell, TableRow, Typography, styled } from "@mui/material"
import { toast } from "react-toastify"
import { IPorudzbinaRowProps } from "../models/IPorudzbinaRowProps"
import moment from 'moment'

const PorudzbinaRowStyled = styled(TableRow)(
    ({ theme }) => `
        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.grey[100]};
        }
    `
)

export const PorudzbinaRow = (props: IPorudzbinaRowProps): JSX.Element => {

    console.log(props.porudzbina.createdAt)
    const formattedDate = moment(props.porudzbina.createdAt).format(`DD.MM.YYYY. HH:mm`)

    return (
        props.porudzbina == null ?
        <LinearProgress /> :
        <PorudzbinaRowStyled
            onClick={() => {
                toast.error(`Ova funkcionalnost još uvek nije implementirana`)
            }}>
            <TableCell>{props.porudzbina.oneTimeHash}</TableCell>
            <TableCell>{formattedDate}</TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: mainTheme.palette.warning.main,
                        fontWeight: `600`
                    }}>
                    {props.porudzbina.status}
                </Typography>
            </TableCell>
            <TableCell>{props.porudzbina.user}</TableCell>
            <TableCell>{formatNumber(props.porudzbina.valueWithVAT)}</TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: mainTheme.palette.success.main,
                        fontWeight: `600`
                    }}>
                    {formatNumber(props.porudzbina.discountValue)}
                </Typography>
            </TableCell>
        </PorudzbinaRowStyled>
    )
}