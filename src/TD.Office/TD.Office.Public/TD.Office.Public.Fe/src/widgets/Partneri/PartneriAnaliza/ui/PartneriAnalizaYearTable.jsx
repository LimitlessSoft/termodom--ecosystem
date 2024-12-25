import {
    Alert,
    Box,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '../../../../apis/officeApi'
import { ENDPOINTS_CONSTANTS } from '../../../../constants'
import { InteractiveLoader } from '../../../InteractiveLoader/ui/InteractiveLoader'
import { formatNumber } from '../../../../helpers/numberHelpers'
import { number } from 'yup'

export const PartneriAnalizaYearTable = ({ ppid }) => {
    const [fetching, setFetching] = useState(false)
    const [data, setData] = useState(null)

    useEffect(() => {
        if (!ppid) return

        setData(null)
        setFetching(true)
        officeApi
            .get(ENDPOINTS_CONSTANTS.PARTNERS.GET_ANALIZA(ppid))
            .then((response) => {
                setData(response.data)
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    }, [ppid])

    if (!ppid)
        return (
            <Box>
                <Paper>
                    <Typography p={2}>Odaberite partnera</Typography>
                </Paper>
            </Box>
        )

    if (fetching)
        return <InteractiveLoader messages={['Učitavanje podataka']} />

    if (!data) return null

    const keys = Object.keys(data)
    const years = Object.keys(data[keys[0]]).toSorted((a, b) => {
        return b - a
    })

    return (
        <Box>
            <Alert
                sx={{
                    my: 2,
                }}
                severity="warning"
                variant={'filled'}
            >
                <Typography>
                    Vrednost rabata se uzima samo iz maloprodajnih računa!
                </Typography>
            </Alert>
            <Alert
                sx={{
                    my: 2,
                }}
                severity="info"
                variant={'filled'}
            >
                <Typography>
                    U vrednost prometa se računaju maloprodajni računi i
                    fakture.
                </Typography>
            </Alert>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Podatak</TableCell>
                            {years.map((year) => (
                                <TableCell key={year}>{year}</TableCell>
                            ))}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {keys.map((key) => (
                            <TableRow key={key}>
                                <TableCell>{key}</TableCell>
                                {years.map((year) => (
                                    <TableCell key={year}>
                                        {formatNumber(data[key][year])}
                                    </TableCell>
                                ))}
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    )
}
