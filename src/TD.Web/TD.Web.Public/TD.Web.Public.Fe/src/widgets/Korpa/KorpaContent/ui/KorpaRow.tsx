import { Button, CircularProgress, TableCell, TableRow, Typography } from "@mui/material"
import { IKorpaRowProps } from "../interfaces/IKorpaRowProps"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { toast } from "react-toastify"
import { CookieNames } from "@/app/constants"
import useCookie from 'react-use-cookie'
import { useState } from "react"
import { formatNumber } from "@/app/helpers/numberHelpers"

export const KorpaRow = (props: IKorpaRowProps): JSX.Element => {

    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [isRemoving, setIsRemoving] = useState<boolean>(false)

    const item = props.item
    
    return (
        <TableRow>
            <TableCell>{item.name}</TableCell>
            <TableCell>
                {formatNumber(item.quantity)}
                <Typography component={`span`} mx={1}>
                    {item.unit}
                </Typography>
            </TableCell>
            <TableCell>{formatNumber(item.price)} RSD</TableCell>
            <TableCell>{formatNumber(item.price + (item.price * item.vat / 100))} RSD</TableCell>
            <TableCell>
                <Button
                    disabled={isRemoving}
                    startIcon={isRemoving ? <CircularProgress size={`1rem`} /> : null}
                    variant={`text`}
                    onClick={() => {
                        setIsRemoving(true)
                        
                        fetchApi(ApiBase.Main, `/products/${item.id}/remove-from-cart`, {
                            method: `DELETE`,
                            body: {
                                id: item.productId,
                                oneTimeHash: cartId
                            },
                            contentType: ContentType.ApplicationJson
                        })
                        .then(() => {
                            props.onItemRemove(item)
                            toast.success(`Proizvod je uklonjen iz korpe`)
                        })
                        .finally(() => {
                            setIsRemoving(false)
                        })
                    }}>
                    Ukloni
                </Button>
            </TableCell>
        </TableRow>
    )
}