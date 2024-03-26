import { formatNumber } from "@/app/helpers/numberHelpers"
import { mainTheme } from "@/app/theme"
import { LinearProgress, TableCell, TableRow, Typography, styled } from "@mui/material"
import { toast } from "react-toastify"
import { IPorudzbinaRowProps } from "../models/IPorudzbinaRowProps"
import moment from 'moment'
import { useRouter } from "next/router"

const PorudzbinaRowStyled = styled(TableRow)<{ checkedoutat?: Date }>(
    ({ theme, checkedoutat }) => `
        background-color: ${
            checkedoutat == null || moment(checkedoutat) < moment().add(-1, 'days').set({hour:0,minute:0,second:0,millisecond:0}) ?
            'initial' :
            theme.palette.info.light
        };
        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.grey[200]};
        }
    `
)

export const PorudzbinaRow = (props: IPorudzbinaRowProps): JSX.Element => {

    const router = useRouter()

    return (
        props.porudzbina == null ?
        <LinearProgress /> :
        <PorudzbinaRowStyled
            checkedoutat={new Date(props.porudzbina.checkedOutAt)}
            onClick={() => {
                router.push(`/porudzbine/${props.porudzbina.oneTimeHash}`)
            }}>
            <TableCell>{props.porudzbina.oneTimeHash}</TableCell>
            <TableCell>{ props.porudzbina.checkedOutAt == null ? "" :  moment(props.porudzbina.checkedOutAt).format(`DD.MM.YYYY. HH:mm`)}</TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: mainTheme.palette.warning.main,
                        fontWeight: `600`
                    }}>
                    {props.porudzbina.status}
                </Typography>
            </TableCell>
            <TableCell>{props.porudzbina.userInformation.name}</TableCell>
            <TableCell>{formatNumber(props.porudzbina.summary.valueWithVAT)}</TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: mainTheme.palette.success.main,
                        fontWeight: `600`
                    }}>
                    {formatNumber(props.porudzbina.summary.discountValue)}
                </Typography>
            </TableCell>
        </PorudzbinaRowStyled>
    )
}