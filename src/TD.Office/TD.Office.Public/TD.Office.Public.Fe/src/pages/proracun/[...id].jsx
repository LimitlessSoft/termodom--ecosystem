import { useRouter } from 'next/router'
import {
    Autocomplete,
    Box,
    Button,
    CircularProgress,
    FormControl,
    IconButton,
    MenuItem,
    Stack,
    TextField,
    Tooltip,
    Typography,
} from '@mui/material'
import { toast } from 'react-toastify'
import { useEffect, useState } from 'react'
import {
    Add,
    AddCircle,
    KeyboardDoubleArrowRightRounded,
    Label,
    Lock,
    LockOpen,
} from '@mui/icons-material'
import { DataGrid } from '@mui/x-data-grid'
import { useZMagacini } from '@/zStore'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { HorizontalActionBar, HorizontalActionBarButton } from '../../widgets'
import { ENDPOINTS } from '../../constants'
import { Unlock } from 'next/dist/compiled/@next/font/dist/google'
import moment from 'moment'

const ProracunPage = () => {
    const router = useRouter()

    const magacini = useZMagacini()

    const [partneri, setPartneri] = useState([])
    const [partneriSearch, setPartneriSearch] = useState('')
    const [partneriLoading, setPartneriLoading] = useState(false)

    const [currentDocument, setCurrentDocument] = useState(undefined)

    const [fetching, setFetching] = useState(false)

    useEffect(() => {
        if (router === undefined) return
        if (router.query === undefined) return
        if (router.query.id === undefined) return

        officeApi
            .get(ENDPOINTS.PRORACUNI.GET(router.query.id))
            .then((response) => {
                setCurrentDocument(response.data)
            })
            .catch(handleApiError)
    }, [router])

    if (!currentDocument) return <CircularProgress />

    return (
        <Box p={1}>
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text="Nazad"
                    onClick={() => router.push(`/prracun`)}
                />
            </HorizontalActionBar>
            <Stack direction={`row`} gap={1} my={2}>
                <TextField
                    value={currentDocument?.id}
                    sx={{
                        maxWidth: 100,
                        '& .MuiInputBase-input': {
                            textAlign: 'right',
                        },
                    }}
                    label="Broj"
                />

                <TextField
                    value={
                        magacini.find((x) => x.id === currentDocument.magacinId)
                            ?.name
                    }
                    sx={{
                        width: 300,
                    }}
                    label="Magacin"
                />

                <TextField
                    value={moment(currentDocument?.createdAt + `Z`).format(
                        'DD.MM.YYYY'
                    )}
                    sx={{
                        maxWidth: 200,
                        '& .MuiInputBase-input': {
                            textAlign: 'center',
                        },
                    }}
                    label="Datum zaključavanja"
                />

                <Tooltip
                    title={
                        currentDocument.state === 0 ? `Zaključaj` : `Otkljucaj`
                    }
                    arrow
                >
                    <Button
                        color={
                            currentDocument.state === 0 ? 'success' : 'error'
                        }
                        variant={`contained`}
                        disabled={fetching}
                        onClick={() => {
                            setFetching(true)
                            officeApi
                                .put(
                                    ENDPOINTS.PRORACUNI.STATE(
                                        currentDocument.id
                                    ),
                                    {
                                        state:
                                            currentDocument.state === 0 ? 1 : 0,
                                    }
                                )
                                .then(() => {
                                    toast.success('Dokument zaključan')
                                })
                                .catch(handleApiError)
                                .finally(() => {
                                    setFetching(false)
                                })
                        }}
                    >
                        {currentDocument.state === 0 ? <LockOpen /> : <Lock />}
                    </Button>
                </Tooltip>

                <TextField
                    sx={{
                        maxWidth: 150,
                        '& .MuiInputBase-input': {
                            textAlign: 'center',
                        },
                    }}
                    value={
                        currentDocument?.komercijalnoDokument.length === 0
                            ? `nije poslat`
                            : currentDocument?.komercijalnoDokument
                    }
                    label={`Komercijalno`}
                />
                <Tooltip title={`Pošalji u komercijalno`} arrow>
                    <Button variant={`contained`} disabled={fetching}>
                        <KeyboardDoubleArrowRightRounded />
                    </Button>
                </Tooltip>
            </Stack>
            <Stack direction={`row`} gap={1}>
                {partneri === undefined ? (
                    <CircularProgress />
                ) : (
                    <Autocomplete
                        sx={{
                            minWidth: 300,
                            maxWidth: 500,
                        }}
                        options={partneri}
                        noOptionsText={`Unesi pretragu i lupi enter...`}
                        onInputChange={(event, value) => {
                            setPartneriSearch(value ?? '')
                        }}
                        onKeyDown={(event) => {
                            if (event.key === 'Enter') {
                                if (partneriSearch.length < 5) {
                                    toast.error(
                                        'Pretraga mora imati bar 5 karaktera'
                                    )
                                    return
                                }

                                setPartneriLoading(true)
                                officeApi
                                    .get(
                                        `/partners?searchKeyword=${partneriSearch}`
                                    )
                                    .then((response) => {
                                        setPartneri(response.data.payload)
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setPartneriLoading(false)
                                    })
                            }
                        }}
                        loading={partneriLoading}
                        loadingText={`Pretraga partnera...`}
                        onChange={(event, value) => {
                            toast.error('Partner promenjen')
                        }}
                        getOptionLabel={(option) => {
                            return `${option.naziv}`
                        }}
                        renderInput={(params) => (
                            <TextField
                                {...params}
                                label={currentDocument.ppid`Partner`}
                            />
                        )}
                    />
                )}
                <TextField
                    select
                    label="Način plaćanja"
                    sx={{
                        width: 200,
                    }}
                >
                    <MenuItem>
                        <Typography>Gotovina</Typography>
                    </MenuItem>
                    <MenuItem>
                        <Typography>Kartica</Typography>
                    </MenuItem>
                    <MenuItem>
                        <Typography>Virman</Typography>
                    </MenuItem>
                    <MenuItem>
                        <Typography>Ček</Typography>
                    </MenuItem>
                </TextField>
            </Stack>
            <Stack direction={`row`} paddingTop={2}>
                <Tooltip title={`Dodaj novu stavku`} arrow>
                    <IconButton color={`primary`}>
                        <AddCircle />
                    </IconButton>
                </Tooltip>
            </Stack>
            <DataGrid
                sx={{
                    my: 2,
                }}
                columns={[
                    {
                        field: 'id',
                        headerName: 'ID',
                        width: 100,
                    },
                    {
                        field: 'proizvodIme',
                        headerName: 'Proizvod',
                        width: 200,
                    },
                    {
                        field: 'kolicina',
                        headerName: 'Količina',
                        width: 150,
                    },
                    {
                        field: 'jedinicaMere',
                        headerName: 'JM',
                        width: 150,
                    },
                    {
                        field: 'rabat',
                        headerName: 'Rabat',
                        width: 150,
                    },
                    {
                        field: 'cena',
                        headerName: 'Cena',
                        width: 150,
                    },
                    {
                        field: 'vrednostBezPdv',
                        headerName: 'Vrednost bez PDV',
                        width: 150,
                    },
                    {
                        field: 'vrednostSaPdv',
                        headerName: 'Vrednost sa PDV',
                        width: 150,
                    },
                ]}
                rows={[
                    {
                        id: 1,
                        proizvodIme: 'Proizvod 1',
                        kolicina: 1,
                        jedinicaMere: 'kom',
                        rabat: 0,
                        cena: 1000,
                        vrednostBezPdv: 1000,
                        vrednostSaPdv: 1200,
                    },
                    {
                        id: 2,
                        proizvodIme: 'Proizvod 2',
                        kolicina: 2,
                        jedinicaMere: 'kom',
                        rabat: 0,
                        cena: 2000,
                        vrednostBezPdv: 4000,
                        vrednostSaPdv: 4800,
                    },
                ]}
            />
        </Box>
    )
}

export default ProracunPage
