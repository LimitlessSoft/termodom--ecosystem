import {
    Box,
    Collapse,
    IconButton,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
} from '@mui/material'
import React, { useState } from 'react'
import { formatNumber } from '../../../../helpers/numberHelpers'
import { ArrowDropDownIcon } from '@mui/x-date-pickers'
import { IzvestajIzlazaRobePoGodinamaMagacinRow } from './IzvestajIzlazaRobePoGodinamaMagacinRow'

export const IzvestajIzlazaRobePoGodinamaRow = ({ data, centar, years }) => {
    const [open, setOpen] = useState(false)
    const magacini = Object.keys(data).filter((key) =>
        key.startsWith('magacin')
    )

    return (
        <>
            <TableRow>
                <TableCell>
                    <IconButton
                        size={`small`}
                        onClick={() => {
                            setOpen(!open)
                        }}
                    >
                        <ArrowDropDownIcon />
                    </IconButton>
                </TableCell>
                <TableCell>{centar}</TableCell>
                {years.map((year) => (
                    <TableCell key={year}>
                        {formatNumber(data[`godina${year}`])}
                    </TableCell>
                ))}
            </TableRow>
            <TableRow
                sx={{
                    backgroundColor: `#efb7ff`,
                }}
            >
                <TableCell
                    style={{ paddingBottom: 0, paddingTop: 0 }}
                    unmountOnExit
                    colSpan={12}
                >
                    <Collapse in={open} timeout={`auto`}>
                        <Box>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell></TableCell>
                                        <TableCell>Magacin</TableCell>
                                        {years.map((year) => (
                                            <TableCell key={year}>
                                                {year}
                                            </TableCell>
                                        ))}
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {magacini.map((magacin) => {
                                        const magacinData = data[magacin]
                                        return (
                                            <IzvestajIzlazaRobePoGodinamaMagacinRow
                                                key={magacin}
                                                data={magacinData}
                                                years={years}
                                            />
                                        )
                                    })}
                                </TableBody>
                            </Table>
                        </Box>
                    </Collapse>
                </TableCell>
            </TableRow>
        </>
    )
}
