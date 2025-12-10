import {
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@mui/material'
import { PorudzbinaItemRow } from '../../PorudzbinaItemRow'

export const PorudzbinaItems = ({ items, stockTypes, isDelivery }) => {
    return (
        <Grid
            sx={{
                m: 2,
            }}
        >
            <TableContainer component={Paper}>
                <Table sx={{ width: `100%` }} aria-label="Korpa">
                    <TableHead>
                        <TableRow>
                            <TableCell>Proizvod</TableCell>
                            <TableCell>Količina</TableCell>
                            <TableCell>Cena sa PDV</TableCell>
                            <TableCell>Vrednost sa PDV</TableCell>
                            <TableCell>Rabat</TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {items.map((item) => {
                            return (
                                <PorudzbinaItemRow
                                    key={item.productId}
                                    item={item}
                                    stockTypes={stockTypes}
                                    isDelivery={isDelivery}
                                />
                            )
                        })}
                    </TableBody>
                </Table>
            </TableContainer>
        </Grid>
    )
}
