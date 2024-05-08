import { Button, Grid, LinearProgress, MenuItem, Paper, Typography } from "@mui/material"
import { IPorudzbinaHeaderProps } from "../models/IPorudzbinaHeaderProps"
import { mainTheme } from "@/app/theme"
import moment from 'moment'
import { PorudzbinaHeaderDropdownStyled } from "./PorudzbinaHeaderDropdownStyled"
import { useEffect, useRef, useState } from "react"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { toast } from "react-toastify"
import NextLink from 'next/link'

export const PorudzbinaHeader = (props: IPorudzbinaHeaderProps): JSX.Element => {

    const [stores, setStores] = useState<any[] | undefined>(undefined)
    const [paymentTypes, setPaymentTypes] = useState<any[] | undefined>(undefined)

    const [mestoPreuzimanja, setMestoPreuzimanja] = useState<number | undefined>(undefined)
    const [paymentType, setPaymentType] = useState<number | undefined>(undefined)

    const [mestoPreuzimanjaUpdating, setMestoPreuzimanjaUpdating] = useState<boolean>(false)
    const [paymentTypeUpdating, setPaymentTypeUpdating] = useState<boolean>(false)

    useEffect(() => {

        fetchApi(ApiBase.Main, `/stores`)
        .then((r) => {
            setStores(r)
        })

        fetchApi(ApiBase.Main, `/payment-types`)
        .then((r) => {
            setPaymentTypes(r)
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
                            Datum: {moment(props.porudzbina.checkedOutAt).format(`DD.MM.YYYY. HH:mm`)}
                        </Typography>
                            {
                                props.porudzbina.userInformation.username === undefined || props.porudzbina.userInformation.username.length == 0
                                    ? <Typography> Korisnik: {props.porudzbina.userInformation.name}</Typography>
                                    : <Button href={`/korisnici/${props.porudzbina.userInformation.username}`} target={`_blank`} component={NextLink} variant={`text`} color={`info`} sx={{ p: 0, textDecoration: `underline`, fontWeight: `bolder` }}>Korisnik: {props.porudzbina.userInformation.username}</Button>
                            }
                        
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
                                sm={9}>
                                    {
                                        stores == undefined || mestoPreuzimanja === undefined ?
                                        <LinearProgress /> :
                                        <PorudzbinaHeaderDropdownStyled
                                            disabled={mestoPreuzimanjaUpdating || props.isDisabled || props.porudzbina.komercijalnoBrDok != null}
                                            id='store'
                                            select
                                            value={mestoPreuzimanja}
                                            onChange={(e) => {
                                                var val = parseInt(e.target.value)

                                                setMestoPreuzimanjaUpdating(true)

                                                fetchApi(ApiBase.Main, `/orders/${props.porudzbina.oneTimeHash}/storeId/${val}`, {
                                                    method: `PUT`,
                                                    contentType: ContentType.TextPlain,
                                                    body: null
                                                }).then((r) => {
                                                    setMestoPreuzimanja(val)
                                                    props.onMestoPreuzimanjaChange(val)
                                                    toast.success(`Mesto preuzimanja uspešno ažurirano!`)
                                                }).finally(() => {
                                                    setMestoPreuzimanjaUpdating(false)
                                                })
                                            }}
                                            label='Mesto preuzimanja'
                                            helperText='Izaberite mesto preuzimanja'>
                                                {
                                                    stores.map((store: any) => (
                                                        <MenuItem key={store.id} value={store.id}>
                                                            {store.name}
                                                        </MenuItem>
                                                    ))
                                                }
                                        </PorudzbinaHeaderDropdownStyled>
                                    }
                            </Grid>
                            <Grid
                                item
                                sm={3}>
                                    {
                                        paymentTypes == undefined || paymentType === undefined ?
                                        <LinearProgress /> :
                                        <PorudzbinaHeaderDropdownStyled
                                            id='nacin-placanja'
                                            disabled={paymentTypeUpdating || props.isDisabled || props.porudzbina.komercijalnoBrDok != null}
                                            select
                                            onChange={(e) => {
                                                var val = parseInt(e.target.value)
        
                                                setPaymentTypeUpdating(true)
        
                                                fetchApi(ApiBase.Main, `/orders/${props.porudzbina.oneTimeHash}/paymentTypeId/${val}`, {
                                                    method: `PUT`,
                                                    contentType: ContentType.TextPlain,
                                                    body: null
                                                }).then((r) => {
                                                    setPaymentType(val)
                                                    toast.success(`Način plaćanja uspešno ažuriran!`)
                                                }).finally(() => {
                                                    setPaymentTypeUpdating(false)
                                                })
                                            
                                            }}
                                            value={paymentType}
                                            color={`secondary`}
                                            label='Način plaćanja'
                                            helperText='Izaberite način plaćanja'>
                                                {
                                                    paymentTypes.map((paymentType: any) => (
                                                        <MenuItem key={paymentType.id} value={paymentType.id}>
                                                            {paymentType.name}
                                                        </MenuItem>
                                                    ))
                                                }
                                        </PorudzbinaHeaderDropdownStyled>
                                    }
                            </Grid>
                        </Grid>
                    </Grid>
                </Grid>
        </Paper>
    )
}