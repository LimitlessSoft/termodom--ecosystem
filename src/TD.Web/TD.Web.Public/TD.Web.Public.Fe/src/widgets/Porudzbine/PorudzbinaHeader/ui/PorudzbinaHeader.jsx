import { Grid, LinearProgress, Paper, Typography } from '@mui/material'
import { mainTheme } from '@/app/theme'
import moment from 'moment'
import { useEffect, useState } from 'react'
import dateHelpers from '@/app/helpers/dateHelpers'
import { handleApiError, webApi } from '@/api/webApi'

export const PorudzbinaHeader = (props) => {
    const [stores, setStores] = useState(undefined)
    const [paymentTypes, setPaymentTypes] = useState(undefined)

    const [mestoPreuzimanja, setMestoPreuzimanja] = useState(undefined)
    const [paymentType, setPaymentType] = useState(undefined)

    const [mestoPreuzimanjaUpdating, setMestoPreuzimanjaUpdating] =
        useState(false)
    const [paymentTypeUpdating, setPaymentTypeUpdating] = useState(false)

    useEffect(() => {
        Promise.all([
            webApi.get('/stores?sortColumn=Name'),
            webApi.get('/payment-types'),
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
                backgroundColor: mainTheme.palette.secondary.light,
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
                            {props.porudzbina.komercijalnoBrDok ??
                                `Nije povezan`}
                        </Typography>
                    )}
                    <Typography>
                        Datum:{' '}
                        {moment(
                            dateHelpers.asUtcString(
                                props.porudzbina.checkedOutAt
                            )
                        ).format(`DD.MM.YYYY. HH:mm`)}
                    </Typography>
                    <Typography>
                        Korisnik: {props.porudzbina.userInformation.name}
                    </Typography>
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
                        <Grid item sm={12}>
                            {stores == undefined ||
                            mestoPreuzimanja === undefined ? (
                                <LinearProgress />
                            ) : (
                                <Typography>
                                    Mesto preuzimanja:{' '}
                                    {stores.find(
                                        (s) => s.id === mestoPreuzimanja
                                    )?.name ?? `Nije izabrano`}
                                </Typography>
                            )}
                            {paymentTypes == undefined ||
                            paymentType === undefined ? (
                                <LinearProgress />
                            ) : (
                                <Typography>
                                    Način plaćanja:{' '}
                                    {paymentTypes.find(
                                        (s) => s.id === paymentType
                                    )?.name ?? `Nije izabrano`}
                                </Typography>
                            )}
                        </Grid>
                    </Grid>
                </Grid>
            </Grid>
        </Paper>
    )
}
