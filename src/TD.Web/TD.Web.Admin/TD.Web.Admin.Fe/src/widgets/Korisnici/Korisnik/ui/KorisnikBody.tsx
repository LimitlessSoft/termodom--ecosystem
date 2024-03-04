import { Button, CircularProgress, Grid, MenuItem, Stack, TextField, Typography, radioClasses } from "@mui/material"
import moment from "moment"
import { KorisnikBodyInfoDataWrapperStyled } from "./KorisnikBodyInfoDataWrapperStyled"
import { DatePicker } from "@mui/x-date-pickers"
import { mainTheme } from "@/app/theme"
import { useEffect, useRef, useState } from "react"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import dayjs from "dayjs"
import { toast } from "react-toastify"

export const KorisnikBody = (props: any): JSX.Element => {

    const putUserRequest = useRef<any>({})

    const [professions, setProfessions] = useState<any | undefined>(undefined)
    const [stores, setStores] = useState<any | undefined>(undefined)
    const [cities, setCities] = useState<any | undefined>(undefined)

    useEffect(() => {
        fetchApi(ApiBase.Main, `/professions`)
        .then((r) => {
            setProfessions(r)
        })

        fetchApi(ApiBase.Main, `/stores`)
        .then((r) => {
            setStores(r)
        })

        fetchApi(ApiBase.Main, `/cities`)
        .then((r) => {
            setCities(r)
        })
    }, [])

    useEffect(() => {
        props.user === undefined ? 
            putUserRequest.current = undefined :
            putUserRequest.current = {
                id: props.user.id,
                username: props.user.username,
                nickname: props.user.nickname,
                professionId: props.user.profession.id,
                ppid: props.user.ppid,
                dateOfBirth: props.user.dateOfBirth,
                cityId: props.user.city.id,
                address: props.user.address,
                mobile: props.user.mobile,
                mail: props.user.mail,
                favoriteStoreId: props.user.favoriteStore.id,
                comment: props.user.comment,
                type: props.user.type,
                isActive: props.user.isActive,
                referentId: props.user.referentId,
            }
    }, [props.user])

    return (
        props.user === undefined ?
            <CircularProgress /> :
            <Grid container>
                <Grid item sm={4}
                    sx={{
                        padding: 2,
                    }}>
                    <Grid container
                        sx={{
                            padding: 2,
                            borderRadius: 2,
                            backgroundColor: mainTheme.palette.secondary.main,
                            color: mainTheme.palette.secondary.contrastText
                        }}
                        direction={`column`}>
                        <Typography>
                            Datum kreiranja naloga: {moment(props.user.createdAt).format("DD.MM.yyyy (HH:mm)")}
                        </Typography>
                        <Typography>
                            Datum obrade: { props.user.processingDate !== null ? moment(props.user.processingDate).format("DD.MM.yyyy (HH:mm)") : "Još uvek nije obrađen"}
                        </Typography>
                        <Typography>
                            Poslednji put viđen: { props.user.lastTimeSeen !== null ? moment(props.user.lastTimeSeen).format("DD.MM.yyyy (HH:mm)") : "Nikada"}
                        </Typography>
                    </Grid>
                </Grid>
                <Grid item sm={8}
                    sx={{
                        padding: 2
                    }}>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.nickname}
                                label={`Nadimak`}
                                onChange={(e) => {
                                    putUserRequest.current.nickname = e.target.value
                                }} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            {
                                professions === undefined ?
                                    <CircularProgress /> :
                                    <TextField
                                        select
                                        variant={`filled`}
                                        defaultValue={props.user.profession.id}
                                        label={`Zanimanje`}
                                        onChange={(e) => {
                                            putUserRequest.current.professionId = e.target.value
                                        }}>
                                        {
                                            professions.map((p: any, index: number) => (
                                                <MenuItem key={index} value={p.id}>{p.name}</MenuItem>
                                            ))
                                        }
                                    </TextField>
                            }
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.ppid}
                                label={`PPID`}
                                onChange={(e) => {
                                    putUserRequest.current.ppid = e.target.value
                                }} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <Stack p={`5px`}>
                            <Typography>
                                Datum rođenja
                            </Typography>
                            <DatePicker
                                sx={{
                                    maxWidth: 200,
                                    width: `100%`
                                }}
                                defaultValue={dayjs(props.user.dateOfBirth)}
                                onChange={(e: any) => {
                                    putUserRequest.current.dateOfBirth = e
                                }}/>
                        </Stack>
                        <KorisnikBodyInfoDataWrapperStyled>
                            {
                                cities === undefined ?
                                    <CircularProgress /> :
                                    <TextField
                                        select
                                        variant={`filled`}
                                        defaultValue={props.user.city.id}
                                        label={`Mesto`}
                                        onChange={(e) => {
                                            putUserRequest.current.cityId = e.target.value
                                        }}>
                                        {
                                            cities.map((c: any, index: number) => (
                                                <MenuItem key={index} value={c.id}>{c.name}</MenuItem>
                                            ))
                                        }
                                    </TextField>
                            }
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.address}
                                label={`Adresa`}
                                onChange={(e) => {
                                    putUserRequest.current.address = e.target.value
                                }}/>
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.mobile}
                                label={`Mobilni`}
                                onChange={(e) => {
                                    putUserRequest.current.mobile = e.target.value
                                }} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                variant={`filled`}
                                defaultValue={props.user.mail}
                                label={`Mail`}
                                onChange={(e) => {
                                    putUserRequest.current.mail = e.target.value
                                }} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            {
                                stores === undefined ?
                                    <CircularProgress /> :
                                    <TextField
                                        select
                                        variant={`filled`}
                                        defaultValue={props.user.favoriteStore.id}
                                        label={`Omiljena radnja`}
                                        onChange={(e) => {
                                            putUserRequest.current.favoriteStoreId = e.target.value
                                        }}>
                                        {
                                            stores.map((s: any, index: number) => (
                                                <MenuItem key={index} value={s.id}>{s.name}</MenuItem>
                                            ))
                                        }
                                    </TextField>
                            }
                        </KorisnikBodyInfoDataWrapperStyled>
                        <KorisnikBodyInfoDataWrapperStyled>
                            <TextField
                                multiline={true}
                                fullWidth={true}
                                minRows={4}
                                defaultValue={props.user.comment}
                                label={`Komentar`}
                                onChange={(e) => {
                                    putUserRequest.current.comment = e.target.value
                                }} />
                        </KorisnikBodyInfoDataWrapperStyled>
                        <Button
                            variant={`contained`}
                            onClick={() => {
                                fetchApi(ApiBase.Main, `/users`, {
                                    method: `PUT`,
                                    body: putUserRequest.current,
                                    contentType: ContentType.ApplicationJson
                                })
                                .then((r) => {
                                    toast.success(`Uspešno ažuriran korisnik!`)
                                })
                            }}>
                            Sačuvaj
                        </Button>
                </Grid>
            </Grid>
    )
}