import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { useUser } from "@/app/hooks"
import { Box, Button, CircularProgress, Grid, MenuItem, Stack, TextField, Typography } from "@mui/material"
import { useEffect, useState } from "react"
import { IZakljuciPorudzbinuRequest } from "../interfaces/IZakljuciPorudzbinuRequest"
import { IKorpaZakljucivanjeProps } from "../interfaces/IKorpaZakljucivanjeProps"
import { toast } from 'react-toastify'

const textFieldVariant = 'filled'

export const KorpaZakljucivanje = (props: IKorpaZakljucivanjeProps): JSX.Element => {

    const user = useUser()
    const [stores, setStores] = useState<any | undefined>(null)
    const [paymentTypes, setPaymentTypes] = useState<any | undefined>(undefined)
    const [request, setRequest] = useState<IZakljuciPorudzbinuRequest>({
        storeId: undefined,
        name: undefined,
        mobile: undefined,
        note: undefined,
        paymentTypeId: undefined,
        oneTimeHash: props.oneTimeHash
    })

    const [isInProgress, setIsInProgress] = useState<boolean>(false)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/stores?sortColumn=name`)
        .then((res) => {
            setStores(res)
        })

        fetchApi(ApiBase.Main, `/payment-types`)
        .then((res) => {
            setPaymentTypes(res)
        })
    }, [])

    return (
        user == null || user.isLoading ?
        <CircularProgress /> :
        <Grid my={5}>
            <Stack
                alignItems={`center`}
                direction={`column`}
                spacing={2}>
                {
                    stores == null ?
                        <CircularProgress /> :
                        <TextField
                            disabled={isInProgress}
                            id='mesto-preuzimanja'
                            select
                            required
                            defaultValue={props.favoriteStoreId}
                            label='Mesto preuzimanja'
                            sx={{ minWidth: 350 }}
                            onChange={(e) => {
                                setRequest((prev) => {
                                    return { ...prev, storeId: Number.parseInt(e.target.value)}
                                })
                            }}
                            helperText='Izaberite mesto preuzimanja'>
                                {
                                    stores.map((store: any) => {
                                        return (
                                            <MenuItem key={store.id} value={store.id}>
                                                {store.name}
                                            </MenuItem>
                                        )
                                    })
                                }
                        </TextField>
                }

                {
                    user.isLogged ? null :
                        <TextField
                            required
                            disabled={isInProgress}
                            sx={{ m: 1, minWidth: 350 }}
                            id='ime-i-prezime'
                            label='Ime i prezime'
                            onChange={(e) => {
                                setRequest((prev) => { return { ...prev, name: e.target.value }})
                            }}
                            variant={textFieldVariant} />
                }

                {
                    user.isLogged ? null :
                        <TextField
                            required
                            disabled={isInProgress}
                            sx={{ m: 1, minWidth: 350 }}
                            id='mobilni'
                            label='Mobilni telefon'
                            onChange={(e) => {
                                setRequest((prev) => { return { ...prev, mobile: e.target.value }})
                            }}
                            variant={textFieldVariant} />
                }

                <TextField
                    disabled={isInProgress}
                    sx={{ m: 1, minWidth: 350 }}
                    id='napomena'
                    label='Napomena'
                    onChange={(e) => {
                        setRequest((prev) => { return { ...prev, note: e.target.value }})
                    }}
                    variant={textFieldVariant} />

                {
                    paymentTypes == undefined || paymentTypes == null ?
                        <CircularProgress /> : 
                        <TextField
                            disabled={isInProgress}
                            id='nacini-placanja'
                            select
                            required
                            label='Način plaćanja'
                            sx={{ minWidth: 350 }}
                            onChange={(e) => {
                                setRequest((prev) => { return { ...prev, paymentTypeId: Number.parseInt(e.target.value) }})
                            }}
                            helperText='Izaberite način plaćanja'>
                                {
                                    paymentTypes.map((pt: any) => {
                                        return (
                                            <MenuItem key={pt.id} value={pt.id}>
                                                {pt.name}
                                            </MenuItem>
                                        )
                                    })
                                }
                        </TextField>
                }

                <Typography>
                    Cene iz ove porudžbine važe 1 dan/a od dana zaključivanja!
                </Typography>

                <Button
                    disabled={isInProgress}
                    startIcon={isInProgress ? <CircularProgress size={`1em`} /> : null}
                    variant={`contained`}
                    onClick={() => {
                        props.onProcessStart()
                        setIsInProgress(true)
                        fetchApi(ApiBase.Main, `/checkout`, {
                            method: `POST`,
                            body: request,
                            contentType: ContentType.ApplicationJson
                        })
                        .then((res) => {
                            props.onSuccess()
                            toast.success(`Uspešno ste zaključili porudžbinu!`)
                        })
                        .catch(() => {
                            setIsInProgress(false)
                            props.onFail()
                        })
                        .finally(() => {
                            props.onProcessEnd()
                        })
                    }}>
                    Zaključi porudžbinu
                </Button>
            </Stack>
        </Grid>
    )
}