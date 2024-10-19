import {
    Autocomplete,
    Box,
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogTitle,
    LinearProgress,
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
import { useEffect, useState } from 'react'
import { useVrDoks, useZNaciniPlacanja } from '@/zStore'
import { toast } from 'react-toastify'
import Grid2 from '@mui/material/Unstable_Grid2'
import { mainTheme } from '@/themes'
import { DatePicker } from '@mui/x-date-pickers'
import { hasPermission } from '@/helpers/permissionsHelpers'
import { USER_PERMISSIONS } from '@/constants'
import dayjs from 'dayjs'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { DialogBody } from 'next/dist/client/components/react-dev-overlay/internal/components/Dialog'

export const IzvestajUkupneKolicineRobeUDokumentima = () => {
    const vrDoks = useVrDoks()
    const naciniUplate = useZNaciniPlacanja()

    const [data, setData] = useState(undefined)
    const [isDataLoading, setIsDataLoading] = useState(false)

    const [request, setRequest] = useState({
        datumOd: new Date(),
        datumDo: new Date(),
    })

    const [izveziRequest, setIzveziRequest] = useState({})
    const [isIzveziDialogOpen, setIsIzveziDialogOpen] = useState(false)
    const [izveziInProgres, setIzveziInProgres] = useState(false)

    useEffect(() => {
        if (!vrDoks) return

        setRequest((prev) => ({
            ...prev,
            vrDok: vrDoks[0].vrDok,
        }))

        setIzveziRequest((prev) => ({
            ...prev,
            destinationVrDok: vrDoks[0].vrDok,
        }))
    }, [vrDoks])

    useEffect(() => {
        if (!naciniUplate) return

        setRequest((prev) => ({
            ...prev,
            nuid: naciniUplate[0].nuid,
        }))
    }, [naciniUplate])

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
                                    return (
                                        <TextField
                                            {...params}
                                            label={'Vrsta dokumenta'}
                                        />
                                    )
                                }}
                                disabled={isDataLoading}
                                options={vrDoks}
                                defaultValue={vrDoks[0]}
                                onChange={(e, value) => {
                                    setRequest({
                                        ...request,
                                        vrDok: value.vrDok,
                                    })
                                }}
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
                                getOptionLabel={(option) => option.naziv}
                                renderInput={(params) => {
                                    return (
                                        <TextField
                                            {...params}
                                            label={'Nacin uplate'}
                                        />
                                    )
                                }}
                                disabled={isDataLoading}
                                options={naciniUplate}
                                defaultValue={naciniUplate[0]}
                                onChange={(e, value) => {
                                    setRequest({
                                        ...request,
                                        nuid: value.nuid,
                                    })
                                }}
                            />
                        )}
                    </Grid2>

                    <Grid2>
                        <DatePicker
                            disabled={!vrDoks || !naciniUplate || isDataLoading}
                            label="Od datuma"
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e) => {
                                setRequest({
                                    ...request,
                                    datumOd: e,
                                })
                            }}
                        />
                    </Grid2>

                    <Grid2>
                        <DatePicker
                            disabled={!vrDoks || !naciniUplate || isDataLoading}
                            label="Do datuma"
                            format="DD.MM.YYYY"
                            defaultValue={dayjs(new Date())}
                            onChange={(e) => {
                                setRequest({
                                    ...request,
                                    datumDo: e,
                                })
                            }}
                        />
                    </Grid2>

                    <Grid2>
                        <Button
                            disabled={!vrDoks || !naciniUplate || isDataLoading}
                            onClick={() => {
                                setIsDataLoading(true)
                                setData(undefined)
                                officeApi
                                    .get(
                                        `izvestaji-ukupnih-kolicina-po-robi-u-filtriranim-dokumentima`,
                                        {
                                            params: request,
                                        }
                                    )
                                    .then((res) => {
                                        setData(res.data)
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setIsDataLoading(false)
                                    })
                            }}
                            color={`primary`}
                            variant={`contained`}
                        >
                            Učitaj izveštaj
                        </Button>
                    </Grid2>
                </Grid2>
                {data && data.items.length > 0 && (
                    <Stack
                        direction={`row`}
                        gap={2}
                        component={Paper}
                        p={2}
                        my={2}
                    >
                        <Dialog
                            open={isIzveziDialogOpen}
                            onClose={() => {
                                if (izveziInProgres) return
                                setIsIzveziDialogOpen(false)
                            }}
                        >
                            <DialogTitle>
                                Izvezi količine u dokument
                            </DialogTitle>
                            <DialogBody>
                                <Stack p={2} gap={2}>
                                    <Autocomplete
                                        disabled={izveziInProgres}
                                        sx={{
                                            width: 400,
                                        }}
                                        getOptionLabel={(option) =>
                                            `[${option.vrDok}] ${option.nazivDok}`
                                        }
                                        renderInput={(params) => {
                                            return (
                                                <TextField
                                                    {...params}
                                                    label={'Vrsta dokumenta'}
                                                />
                                            )
                                        }}
                                        options={vrDoks}
                                        defaultValue={vrDoks[0]}
                                        onChange={(e, value) => {
                                            setIzveziRequest((prev) => ({
                                                ...request,
                                                ...prev,
                                                destinationVrDok: value.vrDok,
                                            }))
                                        }}
                                    />
                                    <TextField
                                        disabled={izveziInProgres}
                                        label={`Broj dokumenta`}
                                        onChange={(e) => {
                                            setIzveziRequest((prev) => ({
                                                ...request,
                                                ...prev,
                                                destinationBrDok:
                                                    e.target.value,
                                            }))
                                        }}
                                    />
                                </Stack>
                            </DialogBody>
                            <DialogActions>
                                {!izveziInProgres && (
                                    <Button
                                        onClick={() => {
                                            setIzveziInProgres(true)
                                            officeApi
                                                .post(
                                                    `izvestaji-ukupnih-kolicina-po-robi-u-filtriranim-dokumentima-izvezi`,
                                                    {
                                                        ...izveziRequest,
                                                        ...request,
                                                    }
                                                )
                                                .then(() => {
                                                    toast.success(
                                                        'Uspešno izvezeno'
                                                    )
                                                })
                                                .catch(handleApiError)
                                                .finally(() => {
                                                    setIsIzveziDialogOpen(false)
                                                    setIzveziInProgres(false)
                                                })
                                        }}
                                        variant={`contained`}
                                    >
                                        Izvezi
                                    </Button>
                                )}
                                {!izveziInProgres && (
                                    <Button
                                        variant={`outlined`}
                                        onClick={() => {
                                            setIsIzveziDialogOpen(false)
                                        }}
                                    >
                                        Otkaži
                                    </Button>
                                )}
                                {izveziInProgres && (
                                    <Box sx={{ width: `100%` }}>
                                        <LinearProgress />
                                    </Box>
                                )}
                            </DialogActions>
                        </Dialog>
                        <Button
                            variant={`contained`}
                            color={'secondary'}
                            onClick={() => {
                                setIsIzveziDialogOpen(true)
                            }}
                        >
                            Izvezi količine u dokument
                        </Button>
                    </Stack>
                )}

                {data && data.items.length !== 0 && (
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
                                {data.items.map((row) => (
                                    <TableRow key={row.robaId}>
                                        <TableCell component="th" scope="row">
                                            {row.robaId}
                                        </TableCell>
                                        <TableCell>{row.naziv}</TableCell>
                                        <TableCell>{row.kolicina}</TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                )}

                {data && data.items.length === 0 && (
                    <Typography variant={`h6`}>
                        Nema podataka za izabrane parametre
                    </Typography>
                )}
            </Stack>
        </Box>
    )
}
