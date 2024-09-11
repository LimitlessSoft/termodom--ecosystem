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

const Partneri = () => {
    return (
        <Grid container p={2} gap={2}>
            <Grid item xs={12}>
                {/* MUI table */}
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>PPID</TableCell>
                                <TableCell>Naziv</TableCell>
                                <TableCell>PIB</TableCell>
                                <TableCell>MB</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {/*{data.map((row) => (*/}
                            {/*    <TableRow key={row.id}>*/}
                            {/*        <TableCell>{row.ime}</TableCell>*/}
                            {/*        <TableCell>{row.prezime}</TableCell>*/}
                            {/*        <TableCell>{row.adresa}</TableCell>*/}
                            {/*        <TableCell>{row.grad}</TableCell>*/}
                            {/*        <TableCell>{row.drzava}</TableCell>*/}
                            {/*        <TableCell>{row.telefon}</TableCell>*/}
                            {/*        <TableCell>{row.email}</TableCell>*/}
                            {/*    </TableRow>*/}
                            {/*))}*/}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Grid>
        </Grid>
    )
}

export default Partneri
