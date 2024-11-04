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
    Typography,
} from '@mui/material'
import { toast } from 'react-toastify'
import { useEffect, useRef, useState } from 'react'
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
import {
    ENDPOINTS,
    PERMISSIONS_GROUPS,
    USER_PERMISSIONS,
} from '../../constants'
import moment from 'moment'
import { formatNumber } from '../../helpers/numberHelpers'
import { ProracunKolicinaCell } from '../../widgets/Proracun/ProracunTable/ui/ProracunKolicinaCell'
import Grid2 from '@mui/material/Unstable_Grid2'
import { ProracunRabatCell } from '../../widgets/Proracun/ProracunTable/ui/ProracunRabatCell'
import { hasPermission } from '../../helpers/permissionsHelpers'
import { usePermissions } from '../../hooks/usePermissionsHook'

const ProracunPage = () => {
    const router = useRouter()

    const permissions = usePermissions(PERMISSIONS_GROUPS.PRORACUNI)

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

    const addWindow = useRef(null)

    const loadDocumentAsync = () => {
        setFetching(true)
        officeApi
            .get(ENDPOINTS.PRORACUNI.GET(router.query.id))
            .then((response) => {
                setCurrentDocument(response.data)
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    }

    useEffect(() => {
        if (router === undefined) return
        if (router.query === undefined) return
        if (router.query.id === undefined) return

        setCurrentDocument(undefined)
        loadDocumentAsync()
    }, [router])

    useEffect(() => {
        if (
            currentDocument === undefined ||
            currentDocument.ppid === undefined ||
            currentDocument.ppid === '' ||
            currentDocument.ppid === null
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
            <Grid2 container>
                <Grid2 sm={8}>
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
                                magacini.find(
                                    (x) => x.id === currentDocument.magacinId
                                )?.name
                            }
                            sx={{
                                width: 300,
                            }}
                            label="Magacin"
                        />

                        <TextField
                            value={moment(
                                currentDocument?.createdAt + `Z`
                            ).format('DD.MM.YYYY')}
                            sx={{
                                maxWidth: 200,
                                '& .MuiInputBase-input': {
                                    textAlign: 'center',
                                },
                            }}
                            label="Datum"
                        />

                        <Tooltip
                            title={
                                currentDocument.state === 0
                                    ? `Zaključaj`
                                    : `Otkljucaj`
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
                                    disabled={
                                        fetching ||
                                        (currentDocument.state === 0 &&
                                            !hasPermission(
                                                permissions,
                                                USER_PERMISSIONS.PRORACUNI.LOCK
                                            )) ||
                                        (currentDocument.state === 1 &&
                                            !hasPermission(
                                                permissions,
                                                USER_PERMISSIONS.PRORACUNI
                                                    .UNLOCK
                                            ))
                                    }
                                    onClick={() => {
                                        setFetching(true)
                                        officeApi
                                            .put(
                                                ENDPOINTS.PRORACUNI.STATE(
                                                    currentDocument.id
                                                ),
                                                {
                                                    state:
                                                        currentDocument.state ===
                                                        0
                                                            ? 1
                                                            : 0,
                                                }
                                            )
                                            .then(() => {
                                                toast.success(
                                                    'Dokument zaključan'
                                                )
                                                setCurrentDocument((prev) => {
                                                    return {
                                                        ...prev,
                                                        state:
                                                            prev.state === 0
                                                                ? 1
                                                                : 0,
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
                                currentDocument?.komercijalnoDokument.length ===
                                0
                                    ? `nije poslat`
                                    : currentDocument?.komercijalnoDokument
                            }
                            label={`Komercijalno`}
                        />
                        <Tooltip title={`Pošalji u komercijalno`} arrow>
                            <>
                                <Button
                                    variant={`contained`}
                                    disabled={
                                        fetching ||
                                        currentDocument.state !== 1 ||
                                        currentDocument.komercijalnoDokument !==
                                            ''
                                    }
                                    onClick={() => {
                                        setFetching(true)
                                        officeApi
                                            .post(
                                                ENDPOINTS.PRORACUNI.FORWARD_TO_KOMERCIJALNO(
                                                    currentDocument.id
                                                )
                                            )
                                            .then(() => {
                                                toast.success(
                                                    `Dokument poslat u komercijalno`
                                                )
                                                loadDocumentAsync()
                                            })
                                            .catch(handleApiError)
                                            .finally(() => {
                                                setFetching(false)
                                            })
                                    }}
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
                                disabled={
                                    fetching || currentDocument.state !== 0
                                }
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
                                                setPartneri(
                                                    response.data.payload
                                                )
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
                            disabled={
                                fetching ||
                                currentDocument.state !== 0 ||
                                currentDocument.type != 'MP'
                            }
                            value={currentDocument.nuid}
                            onChange={(event) => {
                                setFetching(true)
                                officeApi
                                    .put(
                                        ENDPOINTS.PRORACUNI.NUID(
                                            currentDocument.id
                                        ),
                                        {
                                            nuid: event.target.value,
                                        }
                                    )
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
                            <>
                                <IconButton
                                    color={`primary`}
                                    disabled={
                                        fetching || currentDocument.state !== 0
                                    }
                                    onClick={() => {
                                        const channelName =
                                            `host-proracun-new-item` +
                                            Date.now()
                                        addWindow.current = window.open(
                                            `/izbor-robe?channel=${channelName}&noLayout=true`,
                                            `newWindow`,
                                            `popup,width=800,height=600`
                                        )

                                        if (!addWindow.current) {
                                            toast.error(
                                                `Nije moguće otvoriti novi prozor`
                                            )
                                        }

                                        const channel = new BroadcastChannel(
                                            channelName
                                        )
                                        channel.onmessage = (event) => {
                                            switch (event.data.type) {
                                                case 'select-roba':
                                                    setFetching(true)
                                                    const robaId =
                                                        event.data.payload
                                                            .robaId
                                                    const kolicina =
                                                        event.data.payload
                                                            .kolicina
                                                    officeApi
                                                        .post(
                                                            ENDPOINTS.PRORACUNI.POST_ITEM(
                                                                currentDocument.id
                                                            ),
                                                            {
                                                                robaId,
                                                                kolicina,
                                                            }
                                                        )
                                                        .then((response) => {
                                                            setCurrentDocument(
                                                                (prev) => {
                                                                    return {
                                                                        ...prev,
                                                                        items: [
                                                                            ...prev.items,
                                                                            response.data,
                                                                        ],
                                                                    }
                                                                }
                                                            )
                                                            loadDocumentAsync()
                                                        })
                                                        .finally(() => {
                                                            setFetching(false)
                                                        })
                                                        .catch(handleApiError)

                                                    break
                                                default:
                                                    toast.error(
                                                        `Nepoznata akcija`
                                                    )
                                                    break
                                            }
                                        }
                                    }}
                                >
                                    <AddCircle />
                                </IconButton>
                            </>
                        </Tooltip>
                    </Stack>
                </Grid2>
                <Grid2 sm={4}>
                    <Stack gap={2} alignItems={`end`}>
                        {fetching ? (
                            <CircularProgress />
                        ) : (
                            <TextField
                                label={`Ukupno bez PDV`}
                                sx={{
                                    maxWidth: 250,
                                    '& .MuiInputBase-input': {
                                        textAlign: 'right',
                                    },
                                }}
                                value={formatNumber(
                                    currentDocument.ukupnoBezPdv
                                )}
                            />
                        )}
                        {fetching ? (
                            <CircularProgress />
                        ) : (
                            <TextField
                                label={`Ukupno sa PDV`}
                                sx={{
                                    maxWidth: 250,
                                    '& .MuiInputBase-input': {
                                        textAlign: 'right',
                                    },
                                }}
                                value={formatNumber(
                                    currentDocument.ukupnoBezPdv +
                                        currentDocument.ukupnoPdv
                                )}
                            />
                        )}
                    </Stack>
                </Grid2>
            </Grid2>
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
                        renderCell: (params) => {
                            return (
                                <ProracunKolicinaCell
                                    disabled={
                                        fetching || currentDocument.state !== 0
                                    }
                                    kolicina={params.row.kolicina}
                                    proracunId={currentDocument.id}
                                    stavkaId={params.row.id}
                                    onKolicinaSaved={(stavkaId, kolicina) => {
                                        setCurrentDocument((prev) => {
                                            return {
                                                ...prev,
                                                items: prev.items.map((x) => {
                                                    if (x.id === stavkaId) {
                                                        return {
                                                            ...x,
                                                            kolicina:
                                                                kolicina * 1,
                                                        }
                                                    }
                                                    return x
                                                }),
                                            }
                                        })
                                        loadDocumentAsync()
                                    }}
                                />
                            )
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
                        renderCell: (params) => {
                            return (
                                <ProracunRabatCell
                                    disabled={
                                        fetching || currentDocument.state !== 0
                                    }
                                    rabat={params.row.rabat}
                                    proracunId={currentDocument.id}
                                    stavkaId={params.row.id}
                                    onRabatSaved={(stavkaId, rabat) => {
                                        setCurrentDocument((prev) => {
                                            return {
                                                ...prev,
                                                items: prev.items.map((x) => {
                                                    if (x.id === stavkaId) {
                                                        return {
                                                            ...x,
                                                            rabat: rabat * 1,
                                                        }
                                                    }
                                                    return x
                                                }),
                                            }
                                        })
                                        loadDocumentAsync()
                                    }}
                                />
                            )
                        },
                    },
                    {
                        field: 'cenaBezPdv',
                        headerName: 'Cena bez PDV',
                        width: 150,
                        align: 'right',
                        headerAlign: 'right',
                        renderCell: (params) => {
                            if (params.row.rabat === 0) {
                                return (
                                    <Typography>
                                        {formatNumber(params.row.cenaBezPdv)}
                                    </Typography>
                                )
                            } else {
                                return (
                                    <Typography>
                                        <span
                                            style={{
                                                color: `gray`,
                                                margin: `0 5px`,
                                            }}
                                        >
                                            (
                                            {formatNumber(
                                                params.row.cenaBezPdv
                                            )}
                                            )
                                        </span>
                                        {formatNumber(
                                            (params.row.cenaBezPdv *
                                                (100 - params.row.rabat)) /
                                                100
                                        )}
                                    </Typography>
                                )
                            }
                        },
                    },
                    {
                        field: 'cenaSaPdv',
                        headerName: 'Cena sa PDV',
                        width: 150,
                        align: 'right',
                        headerAlign: 'right',
                        renderCell: (params) => {
                            if (params.row.rabat === 0) {
                                return (
                                    <Typography>
                                        {formatNumber(params.row.cenaSaPdv)}
                                    </Typography>
                                )
                            } else {
                                return (
                                    <Typography>
                                        <span
                                            style={{
                                                color: `gray`,
                                                margin: `0 5px`,
                                            }}
                                        >
                                            (
                                            {formatNumber(params.row.cenaSaPdv)}
                                            )
                                        </span>
                                        {formatNumber(
                                            (params.row.cenaSaPdv *
                                                (100 - params.row.rabat)) /
                                                100
                                        )}
                                    </Typography>
                                )
                            }
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
                                        <>
                                            <IconButton
                                                disabled={
                                                    fetching ||
                                                    currentDocument.state !== 0
                                                }
                                                color={`error`}
                                                onClick={() => {
                                                    setFetching(true)
                                                    officeApi
                                                        .delete(
                                                            ENDPOINTS.PRORACUNI.DELETE_ITEM(
                                                                currentDocument.id,
                                                                params.row.id
                                                            )
                                                        )
                                                        .then(() => {
                                                            setCurrentDocument(
                                                                (prev) => {
                                                                    return {
                                                                        ...prev,
                                                                        items: prev.items.filter(
                                                                            (
                                                                                x
                                                                            ) =>
                                                                                x.id !==
                                                                                params
                                                                                    .row
                                                                                    .id
                                                                        ),
                                                                    }
                                                                }
                                                            )
                                                            loadDocumentAsync()
                                                        })
                                                        .catch(handleApiError)
                                                        .finally(() => {
                                                            setFetching(false)
                                                        })
                                                }}
                                            >
                                                <Delete />
                                            </IconButton>
                                        </>
                                    </Tooltip>
                                </Stack>
                            )
                        },
                    },
                ]}
                rows={currentDocument.items}
            />
        </Box>
    )
}

export default ProracunPage
