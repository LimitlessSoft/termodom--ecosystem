import { PorudzbinaRow } from "@/widgets/Porudzbine/PorudzbinaRow"
import { Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material"

const Porudzbine = (): JSX.Element => {
    return (
        <Grid sx={{
            m: 2
        }}>
            <Typography
                variant={`h5`}
                my={4}>
                Porudžbine
            </Typography>
            <TableContainer component={Paper}>
                <Table sx={{ width: `100%` }} aria-label='Korpa'>
                    <TableHead>
                        <TableRow>
                            <TableCell>Broj</TableCell>
                            <TableCell>Datum i vreme</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Korisnik</TableCell>
                            <TableCell>Vrednost sa PDV</TableCell>
                            <TableCell>Ušteda</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <PorudzbinaRow />
                    </TableBody>
                </Table>
            </TableContainer>
        </Grid>
    )
}

export default Porudzbine