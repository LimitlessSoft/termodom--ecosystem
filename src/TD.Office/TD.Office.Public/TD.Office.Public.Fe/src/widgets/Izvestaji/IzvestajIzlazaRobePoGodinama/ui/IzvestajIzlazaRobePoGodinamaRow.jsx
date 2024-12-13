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

export const IzvestajIzlazaRobePoGodinamaRow = () => {
    return (
        <>
            <TableRow>
                <TableCell>
                    {formatNumber(1000, {
                        thousandSeparator: false,
                        decimalCount: 0,
                    })}
                </TableCell>
                <TableCell>Neki naziv</TableCell>
            </TableRow>
            <TableRow>
                <TableCell
                    style={{ paddingBottom: 0, paddingTop: 0 }}
                    colSpan={12}
                >
                    <Collapse in={true} timeout={`auto`} unmountOnExit>
                        <Box>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>More head</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    <TableRow>
                                        <TableCell>Some more data</TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </Box>
                    </Collapse>
                </TableCell>
            </TableRow>
        </>
    )
}
