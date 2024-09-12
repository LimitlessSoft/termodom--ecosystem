import {
    Autocomplete,
    CircularProgress,
    Dialog,
    Grid,
    TextField,
    Typography,
} from '@mui/material'
import { useEffect, useState } from 'react'
import { IPartnerCreateRequest } from '@/widgets/Partneri/PartneriList/interfaces/IPartnerCreateRequest'
import { PartnerNewDialogTextFieldStyled } from '@/widgets/Partneri/PartneriList/styled/PartnerNewDialogTextFieldStyled'
import { PARTNERI_NEW_MESTA_PAYLOAD_DEFAULT_VALUE } from '@/widgets/Partneri/PartneriList/constants'
import { handleApiError, officeApi } from '@/apis/officeApi'
import { ENDPOINTS } from '@/constants'

export const PartnerNewDialog = () => {
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

    const [mestaPayload, setMestaPayload] = useState<any | undefined>(
        PARTNERI_NEW_MESTA_PAYLOAD_DEFAULT_VALUE
    )

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

    return (
        <Dialog open={true}>
            <Grid container gap={2} p={2} direction={`column`}>
                <Grid item>
                    <Typography>Kreiraj novog partnera</Typography>
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Naziv'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Naziv: '' }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Adresa'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Adresa: '' }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Posta'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Posta: '' }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Mesto'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Mesto: '' }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Email'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Email: '' }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Kontakt'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Kontakt: '' }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Kategorija'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Kategorija: 0 }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Mbroj'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Mbroj: '' }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    {mestaPayload === undefined && <CircularProgress />}
                    {mestaPayload !== undefined && (
                        <Autocomplete
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
                                    label={`MestoId`}
                                />
                            )}
                        />
                    )}
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Pib'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Pib: '' }
                            })
                        }}
                    />
                </Grid>
                <Grid item>
                    <PartnerNewDialogTextFieldStyled
                        placeholder={'Mobilni'}
                        onChange={() => {
                            setRBody((prev) => {
                                return { ...prev, Mobilni: '' }
                            })
                        }}
                    />
                </Grid>
            </Grid>
        </Dialog>
    )
}
