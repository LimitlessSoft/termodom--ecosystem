import { Grid, LinearProgress, MenuItem, Paper, Typography } from "@mui/material"
import { IPorudzbinaHeaderProps } from "../models/IPorudzbinaHeaderProps"
import { mainTheme } from "@/app/theme"
import moment from 'moment'
import { useEffect, useRef, useState } from "react"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { asUtcString } from "@/app/helpers/dateHelpers";

export const PorudzbinaHeader = (props: IPorudzbinaHeaderProps): JSX.Element => {

    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const [paymentTypes, setPaymentTypes] = useState<any[] | undefined>(undefined)

    const [mestoPreuzimanja, setMestoPreuzimanja] = useState<number | undefined>(undefined)
    const [paymentType, setPaymentType] = useState<number | undefined>(undefined)

    const [mestoPreuzimanjaUpdating, setMestoPreuzimanjaUpdating] = useState<boolean>(false)
    const [paymentTypeUpdating, setPaymentTypeUpdating] = useState<boolean>(false)

    useEffect(() => {

        fetchApi(ApiBase.Main, `/stores?sortColumn=Name`)
        .then((r) => {
            r.json().then((r: any) => {
                setStores(r)
            })
        })

        fetchApi(ApiBase.Main, `/payment-types`)
        .then((r) => {
            r.json().then((r: any) => {
                setPaymentTypes(r)
            })
        })

    }, [props.porudzbina])

    useEffect(() => {
        if(stores === undefined || props.porudzbina == null)
            return
        
        setMestoPreuzimanja(props.porudzbina.storeId)

    }, [stores])

    useEffect(() => {
        if(paymentTypes === undefined || props.porudzbina == null)
            return
        
        setPaymentType(props.porudzbina.paymentTypeId)
    }, [paymentTypes])

    return (
        <Paper
            sx={{
                m: 2,
                p: 2,
                backgroundColor: mainTheme.palette.secondary.light,
                color: mainTheme.palette.secondary.contrastText
            }}>
                <Grid
                    container
                    width={`100%`}
                    spacing={1}>
                    <Grid
                        item
                        sm={3}>
                        <Typography>
                            WEB: {props.porudzbina.oneTimeHash.substring(0, 8)}
                        </Typography>
                        {
                            props.isTDNumberUpdating ?
                            <LinearProgress /> :
                            <Typography>
                                TD: {props.porudzbina.komercijalnoBrDok ?? `Nije povezan`}
                            </Typography>
                        }
                        <Typography>
                            Datum: {moment(asUtcString(props.porudzbina.checkedOutAt)).format(`DD.MM.YYYY. HH:mm`)}
                        </Typography>
                        <Typography>
                            Korisnik: {props.porudzbina.userInformation.name}
                        </Typography>
                        <Typography sx={{
                            fontWeight: `bold`,
                            my: 2,
                            color: mainTheme.palette.info.main
                        }}>
                            Status: {props.porudzbina.status}
                        </Typography>
                    </Grid>
                    <Grid
                        item
                        sm={9}>
                        <Grid
                            container
                            spacing={1}
                            width={`100%`}>
                            <Grid
                                item
                                sm={12}>
                                    {
                                        stores == undefined || mestoPreuzimanja === undefined ?
                                        <LinearProgress /> :
                                        <Typography>
                                            Mesto preuzimanja: {stores.find((s) => s.id === mestoPreuzimanja)?.name ?? `Nije izabrano`}
                                        </Typography>
                                    }
                                    {
                                        paymentTypes == undefined || paymentType === undefined ?
                                        <LinearProgress /> :
                                        <Typography>
                                            Način plaćanja: {paymentTypes.find((s) => s.id === paymentType)?.name ?? `Nije izabrano`}
                                        </Typography>
                                    }
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
        </Paper>
    )
}