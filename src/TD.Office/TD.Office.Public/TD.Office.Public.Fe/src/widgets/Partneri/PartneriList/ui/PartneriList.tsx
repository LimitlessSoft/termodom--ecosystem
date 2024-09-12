import {
    CircularProgress,
    Grid,
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
import { PARTNERI_DATA_DEFAULT_VALUE } from '@/widgets/Partneri/PartneriList/constants'
import { IPartnerDto } from '@/dtos/partneri/IPartnerDto'
import { ENDPOINTS_KOMERCIJALNO } from '@/constants'

export const PartneriList = () => {
    const [partneriData, setPartneriData] = useState<IPartnerDto[] | undefined>(
        PARTNERI_DATA_DEFAULT_VALUE
    )

    useEffect(() => {
        setPartneriData(PARTNERI_DATA_DEFAULT_VALUE)

        const fetchPartneriData = async () => {
            return await komercijalnoApi
                .TCMDZ(new Date().getFullYear())
                .get(ENDPOINTS_KOMERCIJALNO.PARTNERI.GET_MULTIPLE)
                .catch(handleApiError)
        }

        fetchPartneriData().then((response: any) => {
            setPartneriData(response.data)
        })
    }, [])

    return (
        <Grid>
            {partneriData === undefined && <CircularProgress />}
            {partneriData !== undefined && partneriData.length === 0 && (
                <Typography> Nema podataka </Typography>
            )}
            {partneriData !== undefined && partneriData.length > 0 && (
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>PPID</TableCell>
                                <TableCell>Naziv</TableCell>
                                <TableCell>PIB</TableCell>
                                <TableCell>Adresa</TableCell>
                                <TableCell>Mobilni</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {partneriData.map((partner: IPartnerDto) => (
                                <TableRow key={partner.ppid}>
                                    <TableCell>{partner.ppid}</TableCell>
                                    <TableCell>{partner.naziv}</TableCell>
                                    <TableCell>{partner.pib}</TableCell>
                                    <TableCell>{partner.adresa}</TableCell>
                                    <TableCell>{partner.mobilni}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            )}
        </Grid>
    )
}
