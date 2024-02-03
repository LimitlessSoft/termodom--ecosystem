import { Button, CircularProgress, TableCell, TableRow, Typography } from "@mui/material"
import { IKorpaRowProps } from "../interfaces/IKorpaRowProps"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { toast } from "react-toastify"
import { CookieNames } from "@/app/constants"
import useCookie from 'react-use-cookie'
import { useEffect, useState } from "react"
import { formatNumber } from "@/app/helpers/numberHelpers"
import { KorpaIzmenaKolicineDialog } from "./KorpaIzmenaKolicineDialog"

export const KorpaRow = (props: IKorpaRowProps): JSX.Element => {

    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [isRemoving, setIsRemoving] = useState<boolean>(false)
    const [isIzmenaKolicine, setIsIzmenaKolicine] = useState<boolean>(false)
    const [isIzmenaKolicineDialogOpen, setIsIzmenaKolicineDialogOpen] = useState<boolean>(false)

    useEffect(() => {
        setIsIzmenaKolicine(false)
        setIsRemoving(false)
    }, [props.item])
    
    return (
        <TableRow>
            <TableCell>{props.item.name}</TableCell>
            <TableCell>
                <KorpaIzmenaKolicineDialog
                    isOpen={isIzmenaKolicineDialogOpen}
                    handleClose={(value?: number) => {
                        setIsIzmenaKolicineDialogOpen(false)
                        if(value == null)
                            return

                        setIsIzmenaKolicine(true)
                        fetchApi(ApiBase.Main, `/products/${props.item.id}/set-cart-quantity`, {
                            method: `PUT`,
                            body: {
                                id: props.item.productId,
                                quantity: value,
                                oneTimeHash: cartId
                            },
                            contentType: ContentType.ApplicationJson
                        })
                        .then(() => {
                            props.reloadKorpa()
                            toast.success(`Količina je izmenjena na ${value}`)
                        })
                        .finally(() => {
                        })
                    }}
                    currentKolicina={props.item.quantity} />
                {formatNumber(props.item.quantity)}
                <Typography component={`span`} mx={1}>
                    {props.item.unit}
                </Typography>
                <Button
                    disabled={isRemoving || isIzmenaKolicine}
                    startIcon={isIzmenaKolicine ? <CircularProgress size={`1rem`} /> : null}
                    color={`secondary`} onClick={() => {
                    setIsIzmenaKolicineDialogOpen(true)
                }}>izmeni</Button>
            </TableCell>
            <TableCell>{formatNumber(props.item.price)} RSD</TableCell>
            <TableCell>{formatNumber(props.item.price + (props.item.price * props.item.vat / 100))} RSD</TableCell>
            <TableCell>
                <Button
                    disabled={isRemoving || isIzmenaKolicine}
                    startIcon={isRemoving ? <CircularProgress size={`1rem`} /> : null}
                    variant={`text`}
                    onClick={() => {
                        setIsRemoving(true)
                        
                        fetchApi(ApiBase.Main, `/products/${props.item.id}/remove-from-cart`, {
                            method: `DELETE`,
                            body: {
                                id: props.item.productId,
                                oneTimeHash: cartId
                            },
                            contentType: ContentType.ApplicationJson
                        })
                        .then(() => {
                            props.reloadKorpa()
                            toast.success(`Proizvod je uklonjen iz korpe`)
                        })
                        .finally(() => {
                        })
                    }}>
                    Ukloni
                </Button>
            </TableCell>
        </TableRow>
    )
}