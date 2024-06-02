import { Grid, Table, TableBody, TableCell, TableHead, TableRow } from "@mui/material"

export const NalogZaPrevozTable = (): JSX.Element => {
    return (
        <Grid item sm={12}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Column 1</TableCell>
                        <TableCell>Column 2</TableCell>
                        <TableCell>Column 3</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    <TableRow>
                        <TableCell>Row 1, Cell 1</TableCell>
                        <TableCell>Row 1, Cell 2</TableCell>
                        <TableCell>Row 1, Cell 3</TableCell>
                    </TableRow>
                    <TableRow>
                        <TableCell>Row 2, Cell 1</TableCell>
                        <TableCell>Row 2, Cell 2</TableCell>
                        <TableCell>Row 2, Cell 3</TableCell>
                    </TableRow>
                    {/* Add more rows as needed */}
                </TableBody>
            </Table>
        </Grid>
    )
}