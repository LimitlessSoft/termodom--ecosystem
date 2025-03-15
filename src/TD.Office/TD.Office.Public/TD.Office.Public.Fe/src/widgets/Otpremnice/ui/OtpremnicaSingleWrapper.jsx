import { Box, Button, CircularProgress, IconButton, Stack, TextField, Tooltip, Typography } from '@mui/material'
import Grid2 from '@mui/material/Unstable_Grid2'
import moment from 'moment/moment'
import { hasPermission } from '../../../helpers/permissionsHelpers'
import { ENDPOINTS_CONSTANTS, PERMISSIONS_CONSTANTS } from '../../../constants'
import { toast } from 'react-toastify'
import { AddCircle, Delete, KeyboardDoubleArrowRightRounded, Lock, LockOpen } from '@mui/icons-material'
import { useCallback, useEffect, useRef, useState } from 'react'
import { handleApiError, officeApi } from '../../../apis/officeApi'
import { useZMagacini } from '../../../zStore'
import { DataGrid } from '@mui/x-data-grid'
import { formatNumber } from '../../../helpers/numberHelpers'
import { IzmenaKolicineCell } from '../../IzmenaKolicineCell/ui/IzmenaKolicineCell'
import { usePermissions } from '../../../hooks/usePermissionsHook'

export const OtpremnicaSingleWrapper = ({ id }) => {
    const zMagacini = useZMagacini()

    const permissions = usePermissions(
        PERMISSIONS_CONSTANTS.PERMISSIONS_GROUPS.OTPREMNICE
    )
    
    const [currentDocument, setCurrentDocument] = useState(undefined)
    const [fetching, setFetching] = useState(false)

    const loadDocumentAsync = useCallback(() => {
        setFetching(true)
        officeApi
            .get(ENDPOINTS_CONSTANTS.OTPREMNICE.GET(id))
            .then((response) => {
                setCurrentDocument(response.data)
            })
            .catch(handleApiError)
            .finally(() => {
                setFetching(false)
            })
    })
    
    const addWindow = useRef(null)
    
    useEffect(() => {
        loadDocumentAsync()
    }, [])
    
    if (!currentDocument || !zMagacini) return <CircularProgress />
    
    return (
        <Box>
            <Typography>
                
            </Typography>
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
                                zMagacini.find(
                                    (x) => x.id === currentDocument.magacinId
                                )?.name
                            }
                            sx={{
                                width: 400,
                            }}
                            label="Iz Magacina"
                        />

                        <TextField
                            value={
                                zMagacini.find(
                                    (x) => x.id === currentDocument.destinacioniMagacinId
                                )?.name
                            }
                            sx={{
                                width: 400,
                            }}
                            label="U Magacin"
                        />

                        <TextField
                            value={moment(
                                currentDocument?.createdAt
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
                                        currentDocument.komercijalnoDokument ||
                                        (currentDocument.state === 0 &&
                                            !hasPermission(
                                                permissions,
                                                PERMISSIONS_CONSTANTS
                                                    .USER_PERMISSIONS.OTPREMNICE
                                                    .LOCK
                                            )) ||
                                        (currentDocument.state === 1 &&
                                            !hasPermission(
                                                permissions,
                                                PERMISSIONS_CONSTANTS
                                                    .USER_PERMISSIONS.OTPREMNICE
                                                    .UNLOCK
                                            ))
                                    }
                                    onClick={() => {
                                        setFetching(true)
                                        officeApi
                                            .post(
                                                ENDPOINTS_CONSTANTS.OTPREMNICE.STATE(
                                                    currentDocument.id,
                                                    currentDocument.state ===
                                                    0
                                                        ? 1
                                                        : 0,
                                                ))
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
                                !currentDocument?.komercijalnoDokument
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
                                        currentDocument.komercijalnoDokument
                                    }
                                    onClick={() => {
                                        setFetching(true)
                                        officeApi
                                            .post(
                                                ENDPOINTS_CONSTANTS.OTPREMNICE.FORWARD_TO_KOMERCIJALNO(
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
                                            `host-otpremnica-new-item` +
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
                                                        .put(
                                                            ENDPOINTS_CONSTANTS.OTPREMNICE.SAVE_ITEM(
                                                                currentDocument.id
                                                            ),
                                                            {
                                                                InternaOtpremnicaId: currentDocument.id,
                                                                robaId,
                                                                kolicina
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
                        field: 'proizvod',
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
                                <IzmenaKolicineCell
                                    disabled={
                                        fetching || currentDocument.state !== 0
                                    }
                                    kolicina={params.row.kolicina}
                                    onChange={(kolicina, resolve) => {
                                        officeApi
                                            .put(
                                                ENDPOINTS_CONSTANTS.OTPREMNICE.SAVE_ITEM(
                                                    currentDocument.id
                                                ),
                                                {
                                                    id: params.row.id,
                                                    InternaOtpremnicaId: currentDocument.id,
                                                    robaId: params.row.robaId,
                                                    kolicina
                                                }
                                            )
                                            .then((response) => {
                                                resolve()
                                                setCurrentDocument((prev) => {
                                                    return {
                                                        ...prev,
                                                        items: prev.items.map(
                                                            (x) => {
                                                                if (x.id === response.data.id) {
                                                                    return { ...x, kolicina: response.data.kolicina }
                                                                }
                                                                return x
                                                            }
                                                        ),
                                                    }
                                                })
                                            })
                                            .finally(() => {
                                                setFetching(false)
                                            })
                                            .catch(handleApiError)
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
                                                            ENDPOINTS_CONSTANTS.OTPREMNICE.DELETE_ITEM(
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