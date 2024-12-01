import { formatNumber } from '@/helpers/numberHelpers'
import { mainTheme } from '@/theme'
import {
    LinearProgress,
    TableCell,
    TableRow,
    Typography,
    styled,
} from '@mui/material'
import { toast } from 'react-toastify'
import { IPorudzbinaRowProps } from '../models/IPorudzbinaRowProps'
import moment from 'moment'
import { useRouter } from 'next/router'
import { asUtcString } from '@/helpers/dateHelpers'

const PorudzbinaRowStyled = styled(TableRow)<{ checkedoutat?: Date }>(
    ({ theme, checkedoutat }) => `
        background-color: ${
            checkedoutat == null ||
            moment(asUtcString(checkedoutat)) <
                moment()
                    .add(-1, 'days')
                    .set({ hour: 0, minute: 0, second: 0, millisecond: 0 })
                ? 'initial'
                : theme.palette.info.light
        };
        &:hover {
            cursor: pointer;
            background-color: ${theme.palette.grey[200]};
        }
    `
)

export const PorudzbinaRow = (props: IPorudzbinaRowProps): JSX.Element => {
    const router = useRouter()

    return props.porudzbina == null ? (
        <LinearProgress />
    ) : (
        <PorudzbinaRowStyled
            checkedoutat={new Date(props.porudzbina.checkedOutAt)}
            onClick={() => {
                router.push(`/porudzbine/${props.porudzbina.oneTimeHash}`)
            }}
        >
            <TableCell>{props.porudzbina.oneTimeHash}</TableCell>
            <TableCell>
                {props.porudzbina.checkedOutAt == null
                    ? ''
                    : moment(asUtcString(props.porudzbina.checkedOutAt)).format(
                          `DD.MM.YYYY. HH:mm`
                      )}
            </TableCell>
            <TableCell>
                <Typography
                    sx={{
                        color: () => {
                            switch (props.porudzbina.statusId) {
                                case 1:
                                    return mainTheme.palette.primary.dark
                                case 2:
                                    return mainTheme.palette.secondary.light
                                case 3:
                                    return mainTheme.palette.success.main
                                default:
                                    return mainTheme.palette.text.primary
                            }
                        },
                        fontWeight: `600`,
                    }}
                >
                    {props.porudzbina.status}
                </Typography>
            </TableCell>
            <TableCell
                sx={{
                    color: () => {
                        switch (props.porudzbina.hasAtLeastOneMaxPriceLevel) {
                            case true:
                                return mainTheme.palette.error.main
                            case false:
                                return mainTheme.palette.text.primary
                            default:
                                return mainTheme.palette.text.primary
                        }
                    },
                }}
            >
                {props.porudzbina.userInformation.name}
            </TableCell>
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
