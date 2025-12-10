import { formatNumber } from '@/app/helpers/numberHelpers'
import { mainTheme } from '@/app/theme'
import {
    LinearProgress,
    styled,
    TableCell,
    TableRow,
    Typography,
} from '@mui/material'
import moment from 'moment'
import { useRouter } from 'next/router'
import dateHelpers from '@/app/helpers/dateHelpers'

const PorudzbinaRowStyled = styled(TableRow)(
    ({ theme }) => `
        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.grey[100]};
        }
    `
)

export const PorudzbinaRow = (props) => {
    const router = useRouter()

    return props.porudzbina == null ? (
        <LinearProgress />
    ) : (
        <PorudzbinaRowStyled
            onClick={() => {
                router.push(`/porudzbine/${props.porudzbina.oneTimeHash}`)
            }}
        >
            <TableCell>{props.porudzbina.oneTimeHash}</TableCell>
            <TableCell>
                {props.porudzbina.checkedOutAt == null
                    ? ''
                    : moment(
                          dateHelpers.asUtcString(props.porudzbina.checkedOutAt)
                      ).format(`DD.MM.YYYY. HH:mm`)}
            </TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: mainTheme.palette.warning.main,
                        fontWeight: `600`,
                    }}
                >
                    {props.porudzbina.status}
                </Typography>
            </TableCell>
            <TableCell>{props.porudzbina.userInformation.name}</TableCell>
            <TableCell>
                {formatNumber(props.porudzbina.summary.valueWithVAT)}
            </TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: mainTheme.palette.success.main,
                        fontWeight: `600`,
                    }}
                >
                    {formatNumber(props.porudzbina.summary.discountValue)}
                </Typography>
            </TableCell>
        </PorudzbinaRowStyled>
    )
}
