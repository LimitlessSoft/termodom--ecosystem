import { Button, Grid, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material"
import { hasPermission } from "@/helpers/permissionsHelpers"
import {formatNumber} from "@/helpers/numberHelpers"
import { USER_PERMISSIONS } from "@/constants"
import { Print } from "@mui/icons-material"
import NextLink from 'next/link'
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
                                        <Button 
                                            LinkComponent={NextLink}
                                            color={`secondary`}
                                            href={`/nalog-za-prevoz/${row.id}?noLayout=true`}
                                            target={`_blank`}
                                            disabled={!hasPermission(props.permissions, USER_PERMISSIONS.NALOG_ZA_PREVOZ.INDIVIDUAL_ORDER_PRINT)}>
                                            <Print />
                                        </Button>
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