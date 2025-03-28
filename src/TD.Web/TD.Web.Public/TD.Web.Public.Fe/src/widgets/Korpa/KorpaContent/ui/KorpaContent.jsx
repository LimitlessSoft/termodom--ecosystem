import {
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@mui/material'
import { KorpaRow } from './KorpaRow'
import { korpaConstants } from '../../korpaConstants'

export const KorpaContent = (props) => {
    return (
        <TableContainer component={Paper}>
            <Table sx={{ width: `100%` }} aria-label="Korpa">
                <TableHead>
                    <TableRow>
                        <TableCell
                            sx={{
                                minWidth:
                                    korpaConstants.proizvodiColumnMinWidth,
                            }}
                        >
                            Proizvod
                        </TableCell>
                        <TableCell sx={{ textAlign: `center` }}>
                            Količina
                        </TableCell>
                        <TableCell
                            sx={{
                                whiteSpace: 'nowrap',
                                textAlign: `right`,
                            }}
                        >
                            Cena sa PDV
                        </TableCell>
                        <TableCell
                            sx={{
                                whiteSpace: 'nowrap',
                                textAlign: `right`,
                            }}
                        >
                            Vrednost sa PDV
                        </TableCell>
                        <TableCell>{/* Action buttons */}</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {props.cart.items.map((item) => (
                        <KorpaRow
                            key={item.id}
                            item={item}
                            reloadKorpa={() => {
                                props.reloadKorpa()
                            }}
                        />
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}
