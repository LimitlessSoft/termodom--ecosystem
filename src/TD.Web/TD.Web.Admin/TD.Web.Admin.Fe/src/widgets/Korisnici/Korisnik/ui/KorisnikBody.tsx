import {
    Button,
    CircularProgress,
    Grid,
    MenuItem,
    Stack,
    TextField,
    Typography,
} from '@mui/material'
import moment from 'moment'
import { KorisnikBodyInfoDataWrapperStyled } from './KorisnikBodyInfoDataWrapperStyled'
import { DatePicker } from '@mui/x-date-pickers'
import { mainTheme } from '@/theme'
import { useEffect, useRef, useState } from 'react'
import dayjs from 'dayjs'
import { toast } from 'react-toastify'
import { PostaviNovuLozinku } from './PostavniNovuLozinku'
import { asUtcString } from '@/helpers/dateHelpers'
import { PrikaziPorudzbineKorisnika } from './PrikaziPorudzbineKorisnika'
import { PrikaziAnalizuKorisnika } from './PrikaziAnalizuKorisnika'
import { adminApi, handleApiError } from '@/apis/adminApi'

export const KorisnikBody = (props: any) => {
    const putUserRequest = useRef<any>({})

    const [professions, setProfessions] = useState<any | undefined>(undefined)
    const [stores, setStores] = useState<any | undefined>(undefined)
    const [cities, setCities] = useState<any | undefined>(undefined)
    const [paymentTypes, setPaymentTypes] = useState<any | undefined>(undefined)

    useEffect(() => {
        Promise.all([
            adminApi.get(`/professions?sortColumn=Name`),
            adminApi.get(`/stores?sortColumn=Name`),
            adminApi.get(`/cities?sortColumn=Name`),
            adminApi.get(`/payment-types?sortColumn=Name`),
        ])
            .then(([professions, stores, cities, paymentTypes]) => {
                setProfessions(professions.data)
                setStores(stores.data)
                setCities(cities.data)
                setPaymentTypes(paymentTypes.data)
            })
            .catch((err) => handleApiError(err))
    }, [])

    useEffect(() => {
        !props.user
            ? (putUserRequest.current = undefined)
            : (putUserRequest.current = {
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
                  referentId: props.user.referentId,
              })
    }, [props.user])

    return !props.user ? (
        <CircularProgress />
    ) : (
        <Grid container>
            <Grid
                item
                sm={4}
                sx={{
                    padding: 2,
                }}
            >
                <Grid
                    container
                    sx={{
                        padding: 2,
                        borderRadius: 2,
                        backgroundColor: mainTheme.palette.secondary.main,
                        color: mainTheme.palette.secondary.contrastText,
                    }}
                    direction={`column`}
                >
                    <Typography>
                        Datum kreiranja naloga:{' '}
                        {moment(asUtcString(props.user.createdAt)).format(
                            'DD.MM.yyyy (HH:mm)'
                        )}
                    </Typography>
                    <Typography
                        fontWeight={`bold`}
                        color={
                            props.user.processingDate == null
                                ? mainTheme.palette.info.main
                                : mainTheme.palette.primary.contrastText
                        }
                    >
                        Datum odobrenja:{' '}
                        {props.user.processingDate
                            ? moment(
                                  asUtcString(props.user.processingDate)
                              ).format('DD.MM.yyyy (HH:mm)')
                            : 'Još uvek nije odobren'}
                    </Typography>
                    {props.user.amIOwner == true &&
                        !props.user.processingDate &&
                        props.user.isActive && (
                            <Button
                                variant={`contained`}
                                sx={{
                                    my: 2,
                                }}
                                onClick={() => {
                                    adminApi
                                        .put(
                                            `/users/${props.user.username}/approve`
                                        )
                                        .then(() => {
                                            props.onRealoadRequest()
                                            toast.success(
                                                `Uspešno odobren korisnik!`
                                            )
                                        })
                                        .catch((err) => handleApiError(err))
                                }}
                            >
                                Odobri korisnika
                            </Button>
                        )}
                    {props.user.referent === 'bez referenta' &&
                        props.user.isActive && (
                            <Button
                                variant={`contained`}
                                sx={{
                                    my: 2,
                                }}
                                onClick={() => {
                                    adminApi
                                        .put(
                                            `/users/${props.user.username}/get-ownership`
                                        )
                                        .then(() => {
                                            props.onRealoadRequest()
                                            toast.success(
                                                `Uspešno postavljen referent!`
                                            )
                                        })
                                        .catch((err) => handleApiError(err))
                                }}
                            >
                                Postani referent korisniku
                            </Button>
                        )}
                    <Typography>
                        Poslednji put viđen:{' '}
                        {props.user.lastTimeSeen !== null
                            ? moment(
                                  asUtcString(props.user.lastTimeSeen)
                              ).format('DD.MM.yyyy (HH:mm)')
                            : 'Nikada'}
                    </Typography>
                    {props.user.isActive && (
                        <Stack spacing={2} my={2}>
                            <PostaviNovuLozinku
                                username={props.user.username}
                            />
                            <PrikaziPorudzbineKorisnika
                                userId={props.user.id}
                                username={props.user.username}
                            />
                            <PrikaziAnalizuKorisnika
                                username={props.user.username}
                            />
                        </Stack>
                    )}
                </Grid>
            </Grid>
            <Grid
                item
                sm={8}
                sx={{
                    padding: 2,
                }}
            >
                <KorisnikBodyInfoDataWrapperStyled>
                    <TextField
                        disabled={props.disabled}
                        variant={`filled`}
                        defaultValue={props.user.nickname}
                        label={`Nadimak`}
                        onChange={(e) => {
                            putUserRequest.current.nickname = e.target.value
                        }}
                    />
                </KorisnikBodyInfoDataWrapperStyled>
                <KorisnikBodyInfoDataWrapperStyled>
                    {professions === undefined ? (
                        <CircularProgress />
                    ) : (
                        <TextField
                            disabled={props.disabled}
                            select
                            variant={`filled`}
                            defaultValue={props.user.profession.id}
                            label={`Zanimanje`}
                            onChange={(e) => {
                                putUserRequest.current.professionId =
                                    e.target.value
                            }}
                        >
                            {professions.map((p: any, index: number) => (
                                <MenuItem key={index} value={p.id}>
                                    {p.name}
                                </MenuItem>
                            ))}
                        </TextField>
                    )}
                </KorisnikBodyInfoDataWrapperStyled>
                <KorisnikBodyInfoDataWrapperStyled>
                    <TextField
                        disabled={props.disabled}
                        variant={`filled`}
                        defaultValue={props.user.ppid}
                        label={`PPID`}
                        onChange={(e) => {
                            putUserRequest.current.ppid = e.target.value
                        }}
                    />
                </KorisnikBodyInfoDataWrapperStyled>
                <Stack p={`5px`}>
                    <Typography>Datum rođenja</Typography>
                    <DatePicker
                        disabled={props.disabled}
                        sx={{
                            maxWidth: 200,
                            width: `100%`,
                        }}
                        defaultValue={dayjs(props.user.dateOfBirth)}
                        onChange={(e: any) => {
                            putUserRequest.current.dateOfBirth = e
                        }}
                    />
                </Stack>
                <KorisnikBodyInfoDataWrapperStyled>
                    {cities === undefined ? (
                        <CircularProgress />
                    ) : (
                        <TextField
                            disabled={props.disabled}
                            select
                            variant={`filled`}
                            defaultValue={props.user.city.id}
                            label={`Mesto`}
                            onChange={(e) => {
                                putUserRequest.current.cityId = e.target.value
                            }}
                        >
                            {cities.map((c: any, index: number) => (
                                <MenuItem key={index} value={c.id}>
                                    {c.name}
                                </MenuItem>
                            ))}
                        </TextField>
                    )}
                </KorisnikBodyInfoDataWrapperStyled>
                <KorisnikBodyInfoDataWrapperStyled>
                    <TextField
                        disabled={props.disabled}
                        variant={`filled`}
                        defaultValue={props.user.address}
                        label={`Adresa`}
                        onChange={(e) => {
                            putUserRequest.current.address = e.target.value
                        }}
                    />
                </KorisnikBodyInfoDataWrapperStyled>
                <KorisnikBodyInfoDataWrapperStyled>
                    <TextField
                        disabled={props.disabled}
                        variant={`filled`}
                        defaultValue={props.user.mobile}
                        label={`Mobilni`}
                        onChange={(e) => {
                            putUserRequest.current.mobile = e.target.value
                        }}
                    />
                </KorisnikBodyInfoDataWrapperStyled>
                <KorisnikBodyInfoDataWrapperStyled>
                    <TextField
                        disabled={props.disabled}
                        variant={`filled`}
                        defaultValue={props.user.mail}
                        label={`Mail`}
                        onChange={(e) => {
                            putUserRequest.current.mail = e.target.value
                        }}
                    />
                </KorisnikBodyInfoDataWrapperStyled>
                <KorisnikBodyInfoDataWrapperStyled>
                    {stores === undefined ? (
                        <CircularProgress />
                    ) : (
                        <TextField
                            sx={{
                                minWidth: `200px`,
                            }}
                            select
                            disabled={props.disabled}
                            variant={`filled`}
                            defaultValue={props.user.favoriteStore.id}
                            label={`Omiljena radnja`}
                            onChange={(e) => {
                                putUserRequest.current.favoriteStoreId =
                                    e.target.value
                            }}
                        >
                            {stores.map((s: any, index: number) => (
                                <MenuItem key={index} value={s.id}>
                                    {s.name}
                                </MenuItem>
                            ))}
                        </TextField>
                    )}
                </KorisnikBodyInfoDataWrapperStyled>
                <KorisnikBodyInfoDataWrapperStyled>
                    {paymentTypes === undefined ? (
                        <CircularProgress />
                    ) : (
                        <TextField
                            sx={{
                                minWidth: `200px`,
                            }}
                            disabled={props.disabled}
                            select
                            variant={`filled`}
                            defaultValue={props.user.defaultPaymentTypeId}
                            label={`Podrazumevani način plaćanja`}
                            onChange={(e) => {
                                putUserRequest.current.defaultPaymentTypeId =
                                    e.target.value
                            }}
                        >
                            {paymentTypes.map((p: any, index: number) => (
                                <MenuItem key={index} value={p.id}>
                                    {p.name}
                                </MenuItem>
                            ))}
                        </TextField>
                    )}
                </KorisnikBodyInfoDataWrapperStyled>
                <KorisnikBodyInfoDataWrapperStyled>
                    <TextField
                        disabled={props.disabled}
                        multiline={true}
                        fullWidth={true}
                        minRows={4}
                        defaultValue={props.user.comment}
                        label={`Komentar`}
                        onChange={(e) => {
                            putUserRequest.current.comment = e.target.value
                        }}
                    />
                </KorisnikBodyInfoDataWrapperStyled>
                <Button
                    disabled={props.disabled}
                    variant={`contained`}
                    onClick={() => {
                        adminApi
                            .put(`/users`, putUserRequest.current)
                            .then(() => {
                                toast.success(`Uspešno ažuriran korisnik!`)
                            })
                            .catch((err) => handleApiError(err))
                    }}
                >
                    Sačuvaj
                </Button>
            </Grid>
        </Grid>
    )
}
