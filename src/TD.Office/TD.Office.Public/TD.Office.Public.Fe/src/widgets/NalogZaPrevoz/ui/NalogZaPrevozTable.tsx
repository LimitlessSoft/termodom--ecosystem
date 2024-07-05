import { formatNumber } from "@/app/Helpers/numberHelpers"
import { Print } from "@mui/icons-material"
import { Grid, Link, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material"
import moment from "moment"

export const NalogZaPrevozTable = (props: any): JSX.Element => {
    return (
        <Grid item sm={12}>
            {
                (props.data === undefined || props.data !== undefined && props.data.length === 0)
                &&
                <Typography>
                    Nema podataka za prikazati
                </Typography>
            }

            {
                
                props.data !== undefined && props.data.length > 0
                && 
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Datum</TableCell>
                            <TableCell>Adresa</TableCell>
                            <TableCell>Mobilni</TableCell>
                            <TableCell>VrDok</TableCell>
                            <TableCell>BrDok</TableCell>
                            <TableCell>Napomena</TableCell>
                            <TableCell>Prevoznik</TableCell>
                            <TableCell>Cena prevoza bez PDV</TableCell>
                            <TableCell>Od toga mi kupcu naplatili</TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {
                            props.data.map((row: any, index: number) => (
                                <TableRow key={index}>
                                    <TableCell>{moment(row.createdAt).format("DD.MM.yyyy (HH:mm)")}</TableCell>
                                    <TableCell>{row.address}</TableCell>
                                    <TableCell>{row.mobilni}</TableCell>
                                    <TableCell>{row.vrDok}</TableCell>
                                    <TableCell>{row.brDok}</TableCell>
                                    <TableCell>{row.note}</TableCell>
                                    <TableCell>{row.prevoznik}</TableCell>
                                    <TableCell>{formatNumber(row.cenaPrevozaBezPdv)} {row.placenVirmanom && `(V)`}</TableCell>
                                    <TableCell>{formatNumber(row.miNaplatiliKupcuBezPdv)}</TableCell>
                                    <TableCell>
                                        <Link color={`secondary`} href={`/nalog-za-prevoz/${row.id}?noLayout=true`}
                                            target={`_blank`}>
                                            <Print />
                                        </Link>
                                    </TableCell>
                                </TableRow>
                            ))
                        }
                    </TableBody>
                </Table>
            }
        </Grid>
    )
}