import { useRouter } from 'next/router'
import {
    Autocomplete,
    Box,
    Button,
    CircularProgress,
    IconButton,
    MenuItem,
    Stack,
    TextField,
    Tooltip,
} from '@mui/material'
import { toast } from 'react-toastify'
import { useEffect, useState } from 'react'
import {
    AddCircle,
    Delete,
    KeyboardDoubleArrowRightRounded,
    Lock,
    LockOpen,
} from '@mui/icons-material'
import { DataGrid } from '@mui/x-data-grid'
import { useZMagacini } from '@/zStore'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { HorizontalActionBar, HorizontalActionBarButton } from '../../widgets'
import { ENDPOINTS } from '../../constants'
import moment from 'moment'
import { formatNumber } from '../../helpers/numberHelpers'

const ProracunPage = () => {
    const router = useRouter()

    const magacini = useZMagacini()

    const [partneri, setPartneri] = useState([])
    const [partneriSearch, setPartneriSearch] = useState('')
    const [partneriLoading, setPartneriLoading] = useState(false)

    const [selectedPartner, setSelectedPartner] = useState(undefined)

    const [currentDocument, setCurrentDocument] = useState(undefined)

    const [fetching, setFetching] = useState(false)

    const [naciniPlacanja, setNaciniPlacanja] = useState([
        {
            nuid: 5,
            naziv: 'Gotovina',
        },
        {
            nuid: 1,
            naziv: 'Virmanom',
        },
        {
            nuid: 11,
            naziv: 'Karticom',
        },
    ])

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

    useEffect(() => {
        if (
            currentDocument === undefined ||
            currentDocument.ppid === undefined
        ) {
            setSelectedPartner(undefined)
            return
        }

        setFetching(true)
        officeApi
            .get(ENDPOINTS.PARTNERS.GET(currentDocument.ppid))
            .then((response) => {
                setSelectedPartner(response.data)
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    }, [currentDocument])

    if (!currentDocument) return <CircularProgress />

    return (
        <Box p={1}>
            <HorizontalActionBar>
                <HorizontalActionBarButton
                    text="Nazad"
                    onClick={() => router.push(`/proracun`)}
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
                    <>
                        <Button
                            color={
                                currentDocument.state === 0
                                    ? 'success'
                                    : 'error'
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
                                                currentDocument.state === 0
                                                    ? 1
                                                    : 0,
                                        }
                                    )
                                    .then(() => {
                                        toast.success('Dokument zaključan')
                                        setCurrentDocument((prev) => {
                                            return {
                                                ...prev,
                                                state: prev.state === 0 ? 1 : 0,
                                            }
                                        })
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setFetching(false)
                                    })
                            }}
                        >
                            {currentDocument.state === 0 ? (
                                <LockOpen />
                            ) : (
                                <Lock />
                            )}
                        </Button>
                    </>
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
                    <>
                        <Button
                            variant={`contained`}
                            disabled={fetching || currentDocument.state !== 1}
                        >
                            <KeyboardDoubleArrowRightRounded />
                        </Button>
                    </>
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
                        disabled={fetching}
                        options={partneri}
                        noOptionsText={`Unesi pretragu i lupi enter...`}
                        onInputChange={(event, value) => {
                            setPartneriSearch(value ?? '')
                        }}
                        value={selectedPartner || null}
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
                            setFetching(true)
                            officeApi
                                .put(
                                    ENDPOINTS.PRORACUNI.PPID(
                                        currentDocument.id
                                    ),
                                    {
                                        ppid: value.ppid,
                                    }
                                )
                                .then(() => {
                                    setCurrentDocument({
                                        ...currentDocument,
                                        ppid: value.ppid,
                                    })
                                })
                                .catch(handleApiError)
                                .finally(() => {
                                    setFetching(false)
                                })
                        }}
                        getOptionLabel={(option) => {
                            return `${option.naziv}`
                        }}
                        renderInput={(params) => (
                            <TextField
                                {...params}
                                label={
                                    currentDocument.ppid
                                        ? `Partner`
                                        : `Nema partnera`
                                }
                            />
                        )}
                    />
                )}
                <TextField
                    select
                    label="Način plaćanja"
                    disabled={fetching}
                    value={currentDocument.nuid}
                    onChange={(event) => {
                        setFetching(true)
                        officeApi
                            .put(ENDPOINTS.PRORACUNI.NUID(currentDocument.id), {
                                nuid: event.target.value,
                            })
                            .then(() => {
                                setCurrentDocument((prev) => {
                                    return {
                                        ...prev,
                                        nuid: event.target.value,
                                    }
                                })
                            })
                            .catch(handleApiError)
                            .finally(() => {
                                setFetching(false)
                            })
                    }}
                    sx={{
                        width: 200,
                    }}
                >
                    {naciniPlacanja.map((nacin) => (
                        <MenuItem key={nacin.nuid} value={nacin.nuid}>
                            {nacin.naziv}
                        </MenuItem>
                    ))}
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
                        field: 'naziv',
                        headerName: 'Proizvod',
                        minWidth: 300,
                        flex: 1,
                    },
                    {
                        field: 'kolicina',
                        headerName: 'Količina',
                        width: 150,
                        align: 'right',
                        headerAlign: 'right',
                        valueGetter: (params) => {
                            return `${formatNumber(params.row.kolicina, {
                                decimalCount: 0,
                            })}`
                        },
                    },
                    {
                        field: 'jm',
                        headerName: 'JM',
                        width: 80,
                        align: 'center',
                        headerAlign: 'center',
                    },
                    {
                        field: 'rabat',
                        headerName: 'Rabat',
                        width: 80,
                        align: 'center',
                        headerAlign: 'center',
                        valueGetter: (params) => {
                            return `${params.row.rabat}%`
                        },
                    },
                    {
                        field: 'cenaBezPdv',
                        headerName: 'Cena bez PDV',
                        width: 150,
                        align: 'right',
                        headerAlign: 'right',
                        valueGetter: (params) => {
                            return `${formatNumber(params.row.cenaBezPdv)}`
                        },
                    },
                    {
                        field: 'cenaSaPdv',
                        headerName: 'Cena sa PDV',
                        width: 150,
                        align: 'right',
                        headerAlign: 'right',
                        valueGetter: (params) => {
                            return `${formatNumber(params.row.cenaSaPdv)}`
                        },
                    },
                    {
                        field: 'vrednostBezPdv',
                        headerName: 'Vrednost bez PDV',
                        width: 150,
                        align: 'right',
                        headerAlign: 'right',
                        valueGetter: (params) => {
                            return `${formatNumber(params.row.vrednostBezPdv)}`
                        },
                    },
                    {
                        field: 'vrednostSaPdv',
                        headerName: 'Vrednost sa PDV',
                        width: 150,
                        align: 'right',
                        headerAlign: 'right',
                        valueGetter: (params) => {
                            return `${formatNumber(params.row.vrednostSaPdv)}`
                        },
                    },
                    {
                        field: 'actions',
                        headerName: 'Akcije',
                        width: 150,
                        align: 'center',
                        headerAlign: 'center',
                        renderCell: (params) => {
                            return (
                                <Stack direction={`row`} gap={1}>
                                    <Tooltip title={`Obriši stavku`} arrow>
                                        <IconButton color={`error`}>
                                            <Delete />
                                        </IconButton>
                                    </Tooltip>
                                </Stack>
                            )
                        },
                    },
                ]}
                rows={
                    currentDocument.items
                    // [
                    //     {
                    //         id: 1,
                    //         proizvodIme: 'Proizvod 1',
                    //         kolicina: 1,
                    //         jedinicaMere: 'kom',
                    //         rabat: 0,
                    //         cena: 1000,
                    //         vrednostBezPdv: 1000,
                    //         vrednostSaPdv: 1200,
                    //     },
                    //     {
                    //         id: 2,
                    //         proizvodIme: 'Proizvod 2',
                    //         kolicina: 2,
                    //         jedinicaMere: 'kom',
                    //         rabat: 0,
                    //         cena: 2000,
                    //         vrednostBezPdv: 4000,
                    //         vrednostSaPdv: 4800,
                    //     },
                    // ]
                }
            />
        </Box>
    )
}

export default ProracunPage
