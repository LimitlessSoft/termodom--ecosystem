import {
    Box,
    Collapse,
    IconButton,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
} from '@mui/material'
import { formatNumber } from '../../../../helpers/numberHelpers'
import React, { useState } from 'react'
import { ArrowDropDownIcon } from '@mui/x-date-pickers'

export const IzvestajIzlazaRobePoGodinamaMagacinRow = ({ data, years }) => {
    const dokumentiKeys = Object.keys(data[`godina${years[0]}`].dokumenti)
    const [open, setOpen] = useState(false)
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
                <TableCell>{data.naziv}</TableCell>
                {years.map((year) => (
                    <TableCell key={year}>
                        {formatNumber(data[`godina${year}`].vrednost)}
                    </TableCell>
                ))}
            </TableRow>
            <TableRow
                sx={{
                    backgroundColor: `#b7ceff`,
                }}
            >
                <TableCell
                    style={{ paddingBottom: 0, paddingTop: 0 }}
                    colSpan={12}
                >
                    <Collapse in={open} timeout={`auto`} unmountOnExit>
                        <Box>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>Dokument</TableCell>
                                        {years.map((year) => (
                                            <TableCell key={year}>
                                                {year}
                                            </TableCell>
                                        ))}
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {dokumentiKeys.map((dokumentKey) => (
                                        <TableRow key={dokumentKey}>
                                            <TableCell>
                                                {
                                                    data[`godina${years[0]}`]
                                                        .dokumenti[dokumentKey]
                                                        .naziv
                                                }
                                            </TableCell>
                                            {years.map((year) => (
                                                <TableCell key={year}>
                                                    {formatNumber(
                                                        data[`godina${year}`]
                                                            .dokumenti[
                                                            dokumentKey
                                                        ]?.vrednost || 0
                                                    )}
                                                </TableCell>
                                            ))}
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </Box>
                    </Collapse>
                </TableCell>
            </TableRow>
        </>
    )
}
