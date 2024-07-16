import { CircularProgress, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material"
import { KorisniciListRow } from "./KorisniciListRow"
import { KorisniciNovi } from "./KorisniciNovi"
import { useEffect, useState } from "react"
import {officeApi} from "@/apis/officeApi";

export const KorisniciList = (): JSX.Element => {
    
    const [data, setData] = useState<any[] | undefined>(undefined)

    useEffect(() => {
        officeApi.get(`/users`)
            .then((response: any) => {
                setData(response.data.payload)
            })
    }, [])

    return (
        <Grid p={2} container>
            <Grid item sm={12}>
                <KorisniciNovi />
            </Grid>
            { data === undefined && <CircularProgress /> }
            { data !== undefined && data.length === 0 && <Grid>Nema podataka</Grid> }
            { data !== undefined && data.length > 0 &&
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Id</TableCell>
                                <TableCell>Nadimak</TableCell>
                                <TableCell>Korisničko ime</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {
                                data?.map((korisnik, index) => (
                                    <KorisniciListRow key={index} korisnik={korisnik} />
                                ))
                            }
                        </TableBody>
                    </Table>
                </TableContainer>
            }
        </Grid>
    )
}