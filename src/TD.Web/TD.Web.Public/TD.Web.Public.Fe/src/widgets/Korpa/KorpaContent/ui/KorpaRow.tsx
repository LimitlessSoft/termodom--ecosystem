import { Button, CircularProgress, TableCell, TableRow } from "@mui/material"
import { IKorpaRowProps } from "../interfaces/IKorpaRowProps"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { toast } from "react-toastify"
import { CookieNames } from "@/app/constants"
import useCookie from 'react-use-cookie'
import { useState } from "react"

export const KorpaRow = (props: IKorpaRowProps): JSX.Element => {

    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [isRemoving, setIsRemoving] = useState<boolean>(false)

    const item = props.item
    
    return (
        <TableRow>
            <TableCell>{item.name}</TableCell>
            <TableCell>{item.quantity}</TableCell>
            <TableCell>{item.price}</TableCell>
            <TableCell>{item.price + (item.price * item.vat / 100)}</TableCell>
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