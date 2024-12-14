import {
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@mui/material'
import { IzvestajIzlazaRobePoGodinamaRow } from './IzvestajIzlazaRobePoGodinamaRow'
import React from 'react'

export const IzvestajIzlazRobePoGodinamaTable = ({ data }) => {
    const centri = Object.keys(data)
    const godine = Object.keys(data[centri[0]])
        .filter((key) => key.startsWith('godina'))
        .map((key) => key.replace('godina', ''))

    return (
        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell width={50}></TableCell>
                        <TableCell>Centar</TableCell>
                        {godine.map((godina) => (
                            <TableCell key={godina}>{godina}</TableCell>
                        ))}
                    </TableRow>
                </TableHead>
                <TableBody>
                    {centri.map((centar) => (
                        <IzvestajIzlazaRobePoGodinamaRow
                            key={centar}
                            centar={centar}
                            years={godine}
                            data={data[centar]}
                        />
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}
