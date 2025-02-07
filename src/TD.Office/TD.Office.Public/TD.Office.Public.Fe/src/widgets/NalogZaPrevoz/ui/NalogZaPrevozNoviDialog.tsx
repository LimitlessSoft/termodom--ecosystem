import {
    Accordion,
    AccordionDetails,
    AccordionSummary,
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Grid,
    LinearProgress,
    MenuItem,
    Paper,
    TextField,
    Typography,
} from '@mui/material'
import { formatNumber } from '@/helpers/numberHelpers'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import { mainTheme } from '@/themes'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { NALOG_ZA_PREVOZ } from '@/constants'
import { Preview } from '@mui/icons-material'

export const NalogZaPrevozNoviDialog = (props: any): JSX.Element => {
    const vrDoks = [
        { id: 13, name: 'Faktura' },
        { id: 15, name: 'MP Racun' },
        { id: 19, name: 'Int. MP. Otp' },
        { id: 33, name: 'Narudzbenica' },
        { id: 34, name: 'Ponuda' },
        { id: -1, name: 'Ostalo' },
    ]
    const defaultSaveRequest = {
        id: null,
        mobilni: null,
        address: null,
        cenaPrevozaBezPdv: 0,
        miNaplatiliKupcuBezPdv: 0,
        note: null,
        vrDok: null,
        brDok: null,
        storeId: props.store?.id,
        placenVirmanom: false,
    }
    const defaultReferentniRequest = {
        vrDok: 15,
        brDok: 0,
    }
    const defaultKupacPlacaTip = 'gotovinom'

    const [loadingReferentniDokument, setLoadingReferentniDokument] =
        useState<boolean>(false)

    const [kupacPlacaTip, setKupacPlacaTip] =
        useState<string>(defaultKupacPlacaTip)
    const [referentniDokument, setReferentniDokument] = useState<
        any | undefined
    >(undefined)

    const [saveRequest, setSaveRequest] = useState<any>(defaultSaveRequest)

    const [referentniRequest, setReferentniRequest] = useState<any>(
        defaultReferentniRequest
    )

    const [osnov, setOsnov] = useState<any>(NALOG_ZA_PREVOZ.DOKUMENT)

    useEffect(() => {
        setReferentniRequest(defaultReferentniRequest)
        setSaveRequest(defaultSaveRequest)
        setKupacPlacaTip(defaultKupacPlacaTip)
        setReferentniDokument(undefined)
    }, [props.open])

    useEffect(() => {
        if (referentniDokument === undefined) return

        setKupacPlacaTip(
            referentniDokument.vrednostStavkePrevozaBezPdv === null
                ? 'gotovinom'
                : 'dokumentom'
        )
        setSaveRequest((prev: any) => {
            return {
                ...prev,
                vrDok: referentniRequest.vrDok,
                brDok: referentniRequest.brDok,
            }
        })

        if (referentniDokument.vrednostStavkePrevozaBezPdv === null) return

        setSaveRequest((prev: any) => {
            return {
                ...prev,
                miNaplatiliKupcuBezPdv:
                    referentniDokument.vrednostStavkePrevozaBezPdv,
            }
        })
    }, [referentniDokument])

    useEffect(() => {
        setSaveRequest((prev: any) => {
            return {
                ...prev,
                storeId: props.store?.id,
            }
        })
    }, [props.store])

    return (
        <Dialog
            open={props.open}
            onClose={() => {
                props.onClose()
            }}
        >
            <DialogTitle>
                Novi nalog za prevoz ({props.store?.name})
            </DialogTitle>
            <DialogContent>
                <Grid container spacing={2} p={2}>
                    <Grid item xs={12}>
                        <Typography>Nalog se vezuje za dokument</Typography>
                    </Grid>
                    <Grid item>
                        <TextField
                            select
                            defaultValue={15}
                            onChange={(e) => {
                                if(parseInt(e.target.value) <= 0) {
                                    setOsnov(NALOG_ZA_PREVOZ.OSTALO)
                                    setSaveRequest((prev: any) => {
                                        return {
                                            ...prev,
                                            vrDok: parseInt(e.target.value),
                                            brDok: null
                                        }
                                    })
                                    return;
                                }

                                setOsnov(NALOG_ZA_PREVOZ.DOKUMENT)
                                setReferentniRequest((prev: any) => {
                                    return {
                                        ...prev,
                                        vrDok: e.target.value,
                                    }
                                })

                                setSaveRequest((prev: any) => {
                                    return {
                                        ...prev,
                                        vrDok: e.target.value,
                                    }
                                })
                            }}
                            label={`Osnov`}
                        >
                            {vrDoks.map((vrDok) => {
                                return (
                                    <MenuItem value={vrDok.id} key={vrDok.id}>
                                        {vrDok.name}
                                    </MenuItem>
                                )
                            })}
                        </TextField>
                    </Grid>
                    {osnov === NALOG_ZA_PREVOZ.DOKUMENT && 
                        <>
                            <Grid item>
                                <TextField
                                    label={`Broj dokumenta`}
                                    onChange={(e) => {
                                        setReferentniRequest((prev: any) => {
                                            return {
                                                ...prev,
                                                brDok: e.target.value,
                                            }
                                        })

                                        setSaveRequest((prev: any) => {
                                            return {
                                                ...prev,
                                                brDok: e.target.value,
                                            }
                                        })
                                    }}
                                />
                            </Grid>
                        
                            <Grid item xs={12}>
                                <Button
                                    color={`secondary`}
                                    variant={`contained`}
                                    onClick={() => {
                                        setReferentniDokument(undefined)
                                        setLoadingReferentniDokument(true)

                                        officeApi
                                            .get(
                                                `/nalog-za-prevoz-referentni-dokument?vrDok=${referentniRequest.vrDok}&brDok=${referentniRequest.brDok}`
                                            )
                                            .then((response: any) => {
                                                setReferentniDokument(response.data)

                                                setSaveRequest((prev: any) => {
                                                    return {
                                                        ...prev,
                                                        placenVirmanom:
                                                            response.data
                                                                .placenVirmanom,
                                                    }
                                                })
                                            })
                                            .catch((err) => handleApiError(err))
                                            .finally(() => {
                                                setLoadingReferentniDokument(false)
                                            })
                                    }}
                                >
                                    Ucitaj referentni dokument
                                </Button>
                            </Grid>
                        </>
                    }
                    <Grid item xs={12}>
                        {loadingReferentniDokument && (
                            <LinearProgress sx={{ my: 2 }} />
                        )}
                        <Accordion expanded={(referentniDokument !== undefined && osnov === NALOG_ZA_PREVOZ.DOKUMENT) || osnov === NALOG_ZA_PREVOZ.OSTALO}>
                            {referentniDokument === undefined && (
                                <AccordionSummary>
                                    Ucitaj referentni dokument za dalje korake
                                </AccordionSummary>
                            )}
                            <AccordionDetails>
                                <Grid container spacing={1}>
                                    <Grid item xs={12}>
                                        <TextField
                                            disabled={
                                                osnov !== NALOG_ZA_PREVOZ.OSTALO &&
                                                referentniDokument === undefined
                                            }
                                            fullWidth
                                            label={`Adresa`}
                                            onChange={(e) => {
                                                setSaveRequest((prev: any) => {
                                                    return {
                                                        ...prev,
                                                        address: e.target.value,
                                                    }
                                                })
                                            }}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <TextField
                                            disabled={
                                                osnov !== NALOG_ZA_PREVOZ.OSTALO &&
                                                referentniDokument === undefined
                                            }
                                            fullWidth
                                            label={`Kontakt kupca`}
                                            onChange={(e) => {
                                                setSaveRequest((prev: any) => {
                                                    return {
                                                        ...prev,
                                                        mobilni: e.target.value,
                                                    }
                                                })
                                            }}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <TextField
                                            disabled={
                                                osnov !== NALOG_ZA_PREVOZ.OSTALO &&
                                                referentniDokument === undefined
                                            }
                                            fullWidth
                                            label={`Napomena`}
                                            onChange={(e) => {
                                                setSaveRequest((prev: any) => {
                                                    return {
                                                        ...prev,
                                                        note: e.target.value,
                                                    }
                                                })
                                            }}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <TextField
                                            disabled={
                                                osnov !== NALOG_ZA_PREVOZ.OSTALO &&
                                                referentniDokument === undefined
                                            }
                                            fullWidth
                                            label={`Prevoznik`}
                                            onChange={(e) => {
                                                setSaveRequest((prev: any) => {
                                                    return {
                                                        ...prev,
                                                        prevoznik:
                                                            e.target.value,
                                                    }
                                                })
                                            }}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <TextField
                                            disabled={
                                                osnov !== NALOG_ZA_PREVOZ.OSTALO &&
                                                referentniDokument === undefined
                                            }
                                            fullWidth
                                            label={`Puna cena prevoza (bez PDV)`}
                                            onChange={(e) => {
                                                setSaveRequest((prev: any) => {
                                                    return {
                                                        ...prev,
                                                        cenaPrevozaBezPdv:
                                                            e.target.value,
                                                    }
                                                })
                                            }}
                                        />
                                    </Grid>
                                    <Grid item xs={12}>
                                        <Paper sx={{ p: 1 }}>
                                            <Typography m={1}>
                                                Od toga kupac placa
                                            </Typography>
                                            <Accordion
                                                expanded={
                                                    (kupacPlacaTip ===
                                                        'gotovinom' &&
                                                    referentniDokument !==
                                                        undefined) || 
                                                    osnov === NALOG_ZA_PREVOZ.OSTALO
                                                }
                                                disabled={
                                                    (kupacPlacaTip ===
                                                        'dokumentom' ||
                                                    referentniDokument ===
                                                        undefined) &&
                                                    osnov !== NALOG_ZA_PREVOZ.OSTALO
                                                }
                                            >
                                                <AccordionSummary>
                                                    <Typography>
                                                        Gotovinom
                                                    </Typography>
                                                </AccordionSummary>
                                                <AccordionDetails>
                                                    <TextField
                                                        label={`Iznos`}
                                                        onChange={(e) => {
                                                            setSaveRequest(
                                                                (prev: any) => {
                                                                    return {
                                                                        ...prev,
                                                                        miNaplatiliKupcuBezPdv:
                                                                            e
                                                                                .target
                                                                                .value,
                                                                    }
                                                                }
                                                            )
                                                        }}
                                                    />
                                                </AccordionDetails>
                                            </Accordion>
                                            <Accordion
                                                expanded={
                                                    kupacPlacaTip ===
                                                        'dokumentom' &&
                                                    referentniDokument !==
                                                        undefined
                                                }
                                                disabled={
                                                    kupacPlacaTip ===
                                                        'gotovinom' ||
                                                    referentniDokument ===
                                                        undefined
                                                }
                                            >
                                                <AccordionSummary>
                                                    <Typography>
                                                        Kroz dokument
                                                    </Typography>
                                                </AccordionSummary>
                                                <AccordionDetails>
                                                    <Typography
                                                        color={
                                                            mainTheme.palette
                                                                .success.main
                                                        }
                                                    >
                                                        {formatNumber(
                                                            referentniDokument?.vrednostStavkePrevozaBezPdv
                                                        )}{' '}
                                                        rsd + PDV
                                                    </Typography>
                                                </AccordionDetails>
                                            </Accordion>
                                        </Paper>
                                    </Grid>
                                </Grid>
                            </AccordionDetails>
                        </Accordion>
                    </Grid>
                </Grid>
            </DialogContent>
            <DialogActions>
                <Button
                    onClick={() => {
                        props.onClose()
                    }}
                >
                    Odustani
                </Button>
                <Button
                    variant={`contained`}
                    onClick={() => {
                        officeApi
                            .put(`/nalog-za-prevoz`, saveRequest)
                            .then(() => {
                                toast.success('Nalog uspesno kreiran')
                                props.onClose()
                            })
                            .catch((err) => handleApiError(err))
                    }}
                >
                    Kreiraj
                </Button>
            </DialogActions>
        </Dialog>
    )
}
