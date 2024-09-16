import {
    CircularProgress,
    Grid,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material'
import { usePermissions } from '@/hooks/usePermissionsHook'
import { ENDPOINTS, PERMISSIONS_GROUPS, USER_PERMISSIONS } from '@/constants'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { useEffect, useState } from 'react'
import { handleApiError, officeApi } from '@/apis/officeApi'

export const PartneriSkoroKreirani = () => {
    const permissions = usePermissions(PERMISSIONS_GROUPS.PARTNERI)

    const [data, setData] = useState<any | undefined>(undefined)

    useEffect(() => {
        officeApi
            .get(ENDPOINTS.PARTNERS.RECENTLY_CREATED)
            .then((r: any) => {
                setData(r.data)
            })
            .catch(handleApiError)
    }, [])

    if (hasPermission(permissions, USER_PERMISSIONS.PARTNERI.SKORO_KREIRANI))
        return (
            <Paper
                sx={{
                    p: 2,
                }}
            >
                <Stack>
                    <Typography variant={`h6`}>
                        Skoro kreirani partneri
                    </Typography>
                    {data === undefined && <CircularProgress />}
                    {data !== undefined && (
                        <TableContainer component={Paper}>
                            <Table>
                                <TableHead>
                                    <TableRow>
                                        <TableCell>PPID</TableCell>
                                        <TableCell>Naziv</TableCell>
                                        <TableCell>Adresa</TableCell>
                                        <TableCell>Pib</TableCell>
                                        <TableCell>Mobilni</TableCell>
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {data.map((row: any) => (
                                        <TableRow key={row.ppid}>
                                            <TableCell>{row.ppid}</TableCell>
                                            <TableCell>{row.naziv}</TableCell>
                                            <TableCell>{row.adresa}</TableCell>
                                            <TableCell>{row.pib}</TableCell>
                                            <TableCell>{row.mobilni}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    )}
                </Stack>
            </Paper>
        )

    return <></>
}
