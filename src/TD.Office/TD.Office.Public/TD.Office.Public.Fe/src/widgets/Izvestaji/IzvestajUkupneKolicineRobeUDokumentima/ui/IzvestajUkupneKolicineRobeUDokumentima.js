import {
    Autocomplete,
    Box,
    Button,
    CircularProgress,
    Paper,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TextField,
    Typography,
} from '@mui/material'
import { useState } from 'react'
import { useVrDoks, useZNaciniPlacanja } from '@/zStore'
import { toast } from 'react-toastify'
import Grid2 from '@mui/material/Unstable_Grid2'
import { mainTheme } from '@/themes'

export const IzvestajUkupneKolicineRobeUDokumentima = () => {
    const vrDoks = useVrDoks()
    const naciniUplate = useZNaciniPlacanja()

    const [data, setData] = useState([
        {
            robaID: 1,
            naziv: 'Roba 1',
            kolicina: 10,
        },
        {
            robaID: 2,
            naziv: 'Roba 2',
            kolicina: 20,
        },
    ])

    return (
        <Box my={2} px={1}>
            <Stack gap={2}>
                <Typography variant={`h5`}>
                    Izveštaj ukupne količine po robi u filtriranim dokumentima
                </Typography>

                <Grid2 container gap={2} alignItems={`center`}>
                    <Grid2>
                        {(!vrDoks || vrDoks.length === 0) && (
                            <CircularProgress />
                        )}
                        {vrDoks && vrDoks.length > 0 && (
                            <Autocomplete
                                sx={{
                                    width: 400,
                                }}
                                getOptionLabel={(option) =>
                                    `[${option.vrDok}] ${option.nazivDok}`
                                }
                                renderInput={(params) => {
                                    return <TextField {...params} />
                                }}
                                options={vrDoks}
                                defaultValue={vrDoks[0]}
                            />
                        )}
                    </Grid2>

                    <Grid2>
                        {(!naciniUplate || naciniUplate.length === 0) && (
                            <CircularProgress />
                        )}
                        {naciniUplate && naciniUplate.length > 0 && (
                            <Autocomplete
                                sx={{
                                    width: 300,
                                }}
                                getOptionLabel={(option) => option.value}
                                renderInput={(params) => {
                                    return <TextField {...params} />
                                }}
                                options={naciniUplate}
                                defaultValue={naciniUplate[0]}
                            />
                        )}
                    </Grid2>

                    <Grid2>
                        <Button
                            onClick={() => {
                                toast.success('Učitavanje izveštaja...')
                            }}
                            color={`primary`}
                            variant={`contained`}
                        >
                            Učitaj izveštaj
                        </Button>
                    </Grid2>
                </Grid2>
                {data && data.length > 0 && (
                    <Stack
                        direction={`row`}
                        gap={2}
                        component={Paper}
                        p={2}
                        sx={{
                            backgroundColor: mainTheme.palette.info.light,
                        }}
                    >
                        <Button variant={`contained`}>
                            Izvezi količine u dokument
                        </Button>
                    </Stack>
                )}
                <TableContainer component={Paper}>
                    <Table sx={{ minWidth: 650 }}>
                        <TableHead>
                            <TableRow>
                                <TableCell>RobaID</TableCell>
                                <TableCell>Naziv</TableCell>
                                <TableCell>Količina</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {data.map((row) => (
                                <TableRow key={row.robaID}>
                                    <TableCell component="th" scope="row">
                                        {row.robaID}
                                    </TableCell>
                                    <TableCell>{row.naziv}</TableCell>
                                    <TableCell>{row.kolicina}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Stack>
        </Box>
    )
}
