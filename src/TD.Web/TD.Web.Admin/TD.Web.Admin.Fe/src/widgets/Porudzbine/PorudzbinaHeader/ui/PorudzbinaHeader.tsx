import {
    Button,
    Grid,
    LinearProgress,
    MenuItem,
    Paper,
    Typography,
} from '@mui/material'
import { IPorudzbinaHeaderProps } from '../models/IPorudzbinaHeaderProps'
import { mainTheme } from '@/theme'
import moment from 'moment'
import { PorudzbinaHeaderDropdownStyled } from './PorudzbinaHeaderDropdownStyled'
import { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import NextLink from 'next/link'
import { asUtcString } from '@/helpers/dateHelpers'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { getUserTrackPriceLevelColor } from '@/helpers/userHelpers'

export const PorudzbinaHeader = (
    props: IPorudzbinaHeaderProps
): JSX.Element => {
    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const [paymentTypes, setPaymentTypes] = useState<any[] | undefined>(
        undefined
    )

    const [mestoPreuzimanja, setMestoPreuzimanja] = useState<
        number | undefined
    >(undefined)
    const [paymentType, setPaymentType] = useState<number | undefined>(
        undefined
    )

    const [mestoPreuzimanjaUpdating, setMestoPreuzimanjaUpdating] =
        useState<boolean>(false)
    const [paymentTypeUpdating, setPaymentTypeUpdating] =
        useState<boolean>(false)

    useEffect(() => {
        Promise.all([
            adminApi.get(`/stores?sortColumn=Name`),
            adminApi.get(`/payment-types`),
        ])
            .then(([stores, paymentTypes]) => {
                setStores(stores.data)
                setPaymentTypes(paymentTypes.data)
            })
            .catch((err) => handleApiError(err))
    }, [props.porudzbina])

    useEffect(() => {
        if (!stores || !props.porudzbina) return

        setMestoPreuzimanja(props.porudzbina.storeId)
    }, [stores])

    useEffect(() => {
        if (!paymentTypes || !props.porudzbina) return

        setPaymentType(props.porudzbina.paymentTypeId)
    }, [paymentTypes])

    return (
        <Paper
            sx={{
                m: 2,
                p: 2,
                backgroundColor:
                    props.porudzbina.statusId == 5
                        ? `gray`
                        : mainTheme.palette.secondary.light,
                color: mainTheme.palette.secondary.contrastText,
            }}
        >
            <Grid container width={`100%`} spacing={1}>
                <Grid item sm={3}>
                    <Typography>
                        WEB: {props.porudzbina.oneTimeHash.substring(0, 8)}
                    </Typography>
                    {props.isTDNumberUpdating ? (
                        <LinearProgress />
                    ) : (
                        <Typography>
                            TD:{' '}
                            {`${props.porudzbina.komercijalnoVrDok} - ${props.porudzbina.komercijalnoBrDok}` ??
                                `Nije povezan`}
                        </Typography>
                    )}
                    <Typography>
                        Datum:{' '}
                        {moment(
                            asUtcString(props.porudzbina.checkedOutAt)
                        ).format(`DD.MM.YYYY. HH:mm`)}
                    </Typography>
                    {!props.porudzbina.userInformation.id ? (
                        <Typography>
                            {' '}
                            Jednokratni: {props.porudzbina.userInformation.name}
                        </Typography>
                    ) : (
                        <Button
                            href={`/korisnici/${props.porudzbina.username}`}
                            target={`_blank`}
                            component={NextLink}
                            variant={`text`}
                            color={`info`}
                            sx={{
                                color: getUserTrackPriceLevelColor(
                                    props.porudzbina.trackPriceLevel
                                ),
                                p: 0,
                                textDecoration: `underline`,
                                fontWeight: `bolder`,
                            }}
                        >
                            Korisnik: {props.porudzbina.userInformation.name}
                        </Button>
                    )}

                    <Typography
                        sx={{
                            fontWeight: `bold`,
                            my: 2,
                            color: mainTheme.palette.info.main,
                        }}
                    >
                        Status: {props.porudzbina.status}
                    </Typography>
                </Grid>
                <Grid item sm={9}>
                    <Grid container spacing={1} width={`100%`}>
                        <Grid item sm={9}>
                            {!stores || !mestoPreuzimanja ? (
                                <LinearProgress />
                            ) : (
                                <PorudzbinaHeaderDropdownStyled
                                    disabled={
                                        mestoPreuzimanjaUpdating ||
                                        props.isDisabled ||
                                        props.porudzbina.komercijalnoBrDok !=
                                            null ||
                                        props.porudzbina.statusId == 5
                                    }
                                    id="store"
                                    select
                                    value={mestoPreuzimanja}
                                    onChange={(e) => {
                                        var val = parseInt(e.target.value)

                                        setMestoPreuzimanjaUpdating(true)

                                        adminApi
                                            .put(
                                                `/orders/${props.porudzbina.oneTimeHash}/storeId/${val}`
                                            )
                                            .then(() => {
                                                setMestoPreuzimanja(val)
                                                props.onMestoPreuzimanjaChange(
                                                    val
                                                )
                                                toast.success(
                                                    `Mesto preuzimanja uspešno ažurirano!`
                                                )
                                            })
                                            .catch((err) => handleApiError(err))
                                            .finally(() => {
                                                setMestoPreuzimanjaUpdating(
                                                    false
                                                )
                                            })
                                    }}
                                    label="Mesto preuzimanja"
                                    helperText="Izaberite mesto preuzimanja"
                                >
                                    {stores.map((store: any) => (
                                        <MenuItem
                                            key={store.id}
                                            value={store.id}
                                        >
                                            {store.name}
                                        </MenuItem>
                                    ))}
                                </PorudzbinaHeaderDropdownStyled>
                            )}
                        </Grid>
                        <Grid item sm={3}>
                            {paymentTypes == undefined ||
                            paymentType === undefined ? (
                                <LinearProgress />
                            ) : (
                                <PorudzbinaHeaderDropdownStyled
                                    id="nacin-placanja"
                                    disabled={
                                        paymentTypeUpdating ||
                                        props.isDisabled ||
                                        props.porudzbina.komercijalnoBrDok !=
                                            null ||
                                        props.porudzbina.statusId == 5
                                    }
                                    select
                                    onChange={(e) => {
                                        var val = parseInt(e.target.value)

                                        setPaymentTypeUpdating(true)

                                        adminApi
                                            .put(
                                                `/orders/${props.porudzbina.oneTimeHash}/paymentTypeId/${val}`
                                            )
                                            .then(() => {
                                                setPaymentType(val)
                                                toast.success(
                                                    `Način plaćanja uspešno ažuriran!`
                                                )
                                            })
                                            .catch((err) => handleApiError(err))
                                            .finally(() => {
                                                setPaymentTypeUpdating(false)
                                            })
                                    }}
                                    value={paymentType}
                                    color={`secondary`}
                                    label="Način plaćanja"
                                    helperText="Izaberite način plaćanja"
                                >
                                    {paymentTypes.map((paymentType: any) => (
                                        <MenuItem
                                            key={paymentType.id}
                                            value={paymentType.id}
                                        >
                                            {paymentType.name}
                                        </MenuItem>
                                    ))}
                                </PorudzbinaHeaderDropdownStyled>
                            )}
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Paper>
    )
}
