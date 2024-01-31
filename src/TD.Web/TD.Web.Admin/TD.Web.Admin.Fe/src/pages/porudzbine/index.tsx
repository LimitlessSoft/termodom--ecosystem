import { ApiBase, fetchApi } from "@/app/api"
import { PorudzbinaRow } from "@/widgets/Porudzbine/PorudzbinaRow"
import { IPorudzbina } from "@/widgets/Porudzbine/models/IPorudzbina"
import { CircularProgress, Grid, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material"
import { useEffect, useState } from "react"

const Porudzbine = (): JSX.Element => {

    const [porudzbine, setPorudzbine] = useState<IPorudzbina[] | null>(null)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/orders`)
            .then(res => {
                setPorudzbine(res)
            })
    }, [])

    return (
        porudzbine == null ?
        <CircularProgress /> :
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
                        {
                            porudzbine.map((porudzbina: any) => (
                                <PorudzbinaRow key={porudzbina.id} porudzbina={porudzbina} />
                            ))
                        }
                    </TableBody>
                </Table>
            </TableContainer>
        </Grid>
    )
}

export default Porudzbine