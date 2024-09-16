import {
    Autocomplete,
    Button,
    CircularProgress,
    Dialog,
    Grid,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { IPartnerCreateRequest } from '@/widgets/Partneri/PartneriList/interfaces/IPartnerCreateRequest'
import { PartnerNewDialogTextFieldStyled } from '@/widgets/Partneri/PartneriList/styled/PartnerNewDialogTextFieldStyled'
import {
    PARTNERI_NEW_KATEGORIJE_PAYLOAD_DEFAULT_VALUE,
    PARTNERI_NEW_MESTA_PAYLOAD_DEFAULT_VALUE,
    PARTNERI_NEW_MIN_GROUPS_CHECKED,
} from '@/widgets/Partneri/PartneriList/constants'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS } from '@/constants'
import { PartneriPickGroups } from '@/widgets/Partneri/PartneriList/ui/PartneriPickGroups'
import { toast } from 'react-toastify'
import { IPartneriNewDialogProps } from '@/widgets/Partneri/PartneriList/interfaces/IPartneriNewDialogProps'

export const PartneriNewDialog = (props: IPartneriNewDialogProps) => {
    const [rBody, setRBody] = useState<IPartnerCreateRequest>({
        Naziv: '',
        Adresa: '',
        Posta: '',
        Mesto: '',
        Email: '',
        Kontakt: '',
        Kategorija: 0,
        Mbroj: '',
        MestoId: '',
        ZapId: 0,
        RefId: 0,
        Pib: '',
        Mobilni: '',
    })

    const [isPosting, setIsPosting] = useState<boolean>(false)

    const [mestaPayload, setMestaPayload] = useState<any | undefined>(
        PARTNERI_NEW_MESTA_PAYLOAD_DEFAULT_VALUE
    )
    const [kategorijePayload, setKategorijePayload] = useState<any | undefined>(
        PARTNERI_NEW_KATEGORIJE_PAYLOAD_DEFAULT_VALUE
    )

    const [isPickGroupsOpen, setIsPickGroupsOpen] = useState<boolean>(false)
    const [groupsChecked, setGroupsChecked] = useState<number>(0)

    useEffect(() => {
        const fetchMesta = async () => {
            return await officeApi
                .get(ENDPOINTS.PARTNERS.GET_MESTA)
                .catch(handleApiError)
        }

        fetchMesta().then((response: any) => {
            setMestaPayload(response.data)
        })
    }, [])

    useEffect(() => {
        const fetchKategorije = async () => {
            return await officeApi
                .get(ENDPOINTS.PARTNERS.GET_KATEGORIJE)
                .catch(handleApiError)
        }

        fetchKategorije().then((response: any) => {
            setKategorijePayload(response.data)
        })
    }, [])

    const [isNazivValid, setIsNazivValid] = useState<boolean>(false)
    const [isAdresaValid, setIsAdresaValid] = useState<boolean>(false)
    const [isPostaValid, setIsPostaValid] = useState<boolean>(false)
    const [isGradValid, setIsGradValid] = useState<boolean>(false)
    const [isEmailValid, setIsEmailValid] = useState<boolean>(false)
    const [isKontaktValid, setIsKontaktValid] = useState<boolean>(false)
    const [isKategorijaValid, setIsKategorijaValid] = useState<boolean>(false)
    const [isMbrojValid, setIsMbrojValid] = useState<boolean>(false)
    const [isMestoValid, setIsMestoValid] = useState<boolean>(false)
    const [isPibValid, setIsPibValid] = useState<boolean>(false)
    const [isMobilniValid, setIsMobilniValid] = useState<boolean>(false)

    useEffect(() => {
        setIsNazivValid(rBody.Naziv.length > 5)
    }, [rBody.Naziv])

    useEffect(() => {
        setIsMestoValid(rBody.Mesto.length > 5)
    }, [rBody.Mesto])

    useEffect(() => {
        setIsAdresaValid(rBody.Adresa.length > 5)
    }, [rBody.Adresa])

    useEffect(() => {
        setIsPostaValid(rBody.Posta.length > 4)
    }, [rBody.Posta])

    useEffect(() => {
        setIsGradValid(rBody.MestoId.length > 0)
    }, [rBody.MestoId])

    useEffect(() => {
        setIsEmailValid(rBody.Email.length > 5)
    }, [rBody.Email])

    useEffect(() => {
        setIsKontaktValid(rBody.Kontakt.length > 5)
    }, [rBody.Kontakt])

    useEffect(() => {
        setIsKategorijaValid(
            rBody.Kategorija > 0 &&
                groupsChecked >= PARTNERI_NEW_MIN_GROUPS_CHECKED
        )
    }, [rBody.Kategorija, groupsChecked])

    useEffect(() => {
        setIsMbrojValid(rBody.Mbroj.length > 5)
    }, [rBody.Mbroj])

    useEffect(() => {
        setIsPibValid(rBody.Pib.length > 5)
    }, [rBody.Pib])

    useEffect(() => {
        setIsMobilniValid(rBody.Mobilni.length > 5)
    }, [rBody.Mobilni])

    return (
        <Dialog open={props.isOpen} onClose={props.onClose}>
            <Grid container gap={2} p={2} direction={`column`}>
                <Grid item>
                    <Typography>Kreiraj novog partnera</Typography>
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Naziv'}
                        error={!isNazivValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Naziv: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Adresa'}
                        error={!isAdresaValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Adresa: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Postanski Broj'}
                        error={!isPostaValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Posta: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    {mestaPayload === undefined && <CircularProgress />}
                    {mestaPayload !== undefined && (
                        <Autocomplete
                            disabled={isPosting}
                            defaultValue={mestaPayload[0]}
                            options={mestaPayload}
                            onChange={(event, value) => {
                                setRBody((prev) => {
                                    return {
                                        ...prev,
                                        MestoId: value?.mestoId ?? '',
                                    }
                                })
                            }}
                            getOptionLabel={(option) => {
                                return `${option.naziv}`
                            }}
                            renderInput={(params) => (
                                <PartnerNewDialogTextFieldStyled
                                    {...params}
                                    label={`Grad`}
                                />
                            )}
                        />
                    )}
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Mesto'}
                        error={!isMestoValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Mesto: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Email'}
                        error={!isEmailValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Email: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Kontakt'}
                        error={!isKontaktValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Kontakt: e.target.value }
                            })
                        }}
                    />
                </Grid>
                {kategorijePayload === undefined && <CircularProgress />}
                {kategorijePayload !== undefined && (
                    <Grid item textAlign={`center`}>
                        <PartneriPickGroups
                            onClose={() => {
                                setIsPickGroupsOpen(false)
                            }}
                            kategorije={kategorijePayload}
                            open={isPickGroupsOpen}
                            onChange={(val, groupsChecked) => {
                                setGroupsChecked(groupsChecked)
                                setRBody((prev) => {
                                    return { ...prev, Kategorija: val }
                                })
                            }}
                        />
                        <Button
                            disabled={isPosting}
                            variant={`contained`}
                            onClick={() => {
                                setIsPickGroupsOpen(true)
                            }}
                        >
                            Izaberi grupe
                        </Button>
                        <Typography
                            sx={{
                                color: isKategorijaValid ? 'green' : 'red',
                            }}
                        >
                            Izabrano grupa: {groupsChecked} /{' '}
                            {PARTNERI_NEW_MIN_GROUPS_CHECKED}
                        </Typography>
                    </Grid>
                )}
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Maticni Broj'}
                        error={!isMbrojValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Mbroj: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Pib'}
                        error={!isPibValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Pib: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        disabled={isPosting}
                        label={'Mobilni'}
                        error={!isMobilniValid}
                        onChange={(e) => {
                            setRBody((prev) => {
                                return { ...prev, Mobilni: e.target.value }
                            })
                        }}
                    />
                </Grid>
                <Grid item textAlign={`center`}>
                    <Button
                        variant={`contained`}
                        disabled={
                            isPosting ||
                            !isNazivValid ||
                            !isAdresaValid ||
                            !isPostaValid ||
                            !isGradValid ||
                            !isEmailValid ||
                            !isKontaktValid ||
                            !isKategorijaValid ||
                            !isMbrojValid ||
                            !isPibValid ||
                            !isMobilniValid
                        }
                        onClick={() => {
                            setIsPosting(true)
                            officeApi
                                .post(ENDPOINTS.PARTNERS.POST, rBody)
                                .then((e) => {
                                    toast.success('Partner uspesno kreiran')
                                })
                                .catch(handleApiError)
                                .finally(() => {
                                    setIsPosting(false)
                                })
                        }}
                    >
                        Kreiraj partnera{' '}
                        {isPosting && (
                            <CircularProgress
                                size={`2em`}
                                sx={{
                                    px: 2,
                                }}
                            />
                        )}
                    </Button>
                </Grid>
            </Grid>
        </Dialog>
    )
}
