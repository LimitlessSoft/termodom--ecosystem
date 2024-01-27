import { Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material"
import { IKorpaContentProps } from "../interfaces/IKorpaContentProps"
import { KorpaRow } from "./KorpaRow"

export const KorpaContent = (props: IKorpaContentProps): JSX.Element => {
    return (
        <Grid
            container
            py={`1rem`}>
                <TableContainer component={Paper}>
                    <Table sx={{ width: `100%` }} aria-label='Korpa'>
                        <TableHead>
                            <TableRow>
                                <TableCell>Proizvod</TableCell>
                                <TableCell>Količina</TableCell>
                                <TableCell>Cena sa PDV</TableCell>
                                <TableCell>Vrednost sa PDV</TableCell>
                                <TableCell>{/* Action buttons */}</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {
                                props.cart.items.map((item: any) => 
                                    <KorpaRow key={item.id} item={item} />
                                )
                            }
                        </TableBody>
                    </Table>
                </TableContainer>
        </Grid>
    )
}