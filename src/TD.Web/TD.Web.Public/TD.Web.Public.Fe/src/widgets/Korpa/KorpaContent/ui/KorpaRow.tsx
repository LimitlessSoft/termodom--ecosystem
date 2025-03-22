import {
    Button,
    CircularProgress,
    IconButton,
    TableCell,
    TableRow,
    Typography,
} from '@mui/material'
import { IKorpaRowProps } from '../interfaces/IKorpaRowProps'
import { toast } from 'react-toastify'
import { CookieNames } from '@/app/constants'
import useCookie from 'react-use-cookie'
import { useEffect, useState } from 'react'
import { formatNumber } from '@/app/helpers/numberHelpers'
import { KorpaIzmenaKolicineDialog } from './KorpaIzmenaKolicineDialog'
import { handleApiError, webApi } from '@/api/webApi'
import { Delete, Edit } from '@mui/icons-material'

export const KorpaRow = (props: IKorpaRowProps): JSX.Element => {
    const [cartId, setCartId] = useCookie(CookieNames.cartId)
    const [isRemoving, setIsRemoving] = useState<boolean>(false)
    const [isIzmenaKolicine, setIsIzmenaKolicine] = useState<boolean>(false)
    const [isIzmenaKolicineDialogOpen, setIsIzmenaKolicineDialogOpen] =
        useState<boolean>(false)

    useEffect(() => {
        setIsIzmenaKolicine(false)
        setIsRemoving(false)
    }, [props.item])

    return (
        <TableRow>
            <TableCell>{props.item.name}</TableCell>
            <TableCell sx={{ whiteSpace: 'nowrap', textAlign: `right` }}>
                <KorpaIzmenaKolicineDialog
                    isOpen={isIzmenaKolicineDialogOpen}
                    handleClose={(value?: number) => {
                        setIsIzmenaKolicineDialogOpen(false)
                        if (value == null) return

                        setIsIzmenaKolicine(true)
                        webApi
                            .put(
                                `/products/${props.item.productId}/set-cart-quantity`,
                                {
                                    id: props.item.productId,
                                    quantity: value,
                                    oneTimeHash: cartId,
                                }
                            )
                            .then(() => {
                                props.reloadKorpa()
                                toast.success(
                                    `Količina je izmenjena na ${value}`
                                )
                            })
                            .catch((err) => {
                                setIsIzmenaKolicine(false)
                                handleApiError(err)
                            })
                            .finally(() => {})
                    }}
                    currentKolicina={props.item.quantity}
                />
                {formatNumber(props.item.quantity)}
                <Typography component={`span`} mx={1}>
                    {props.item.unit}
                </Typography>
                <IconButton
                    sx={{ p: 0 }}
                    disabled={isRemoving || isIzmenaKolicine || props.disabled}
                    onClick={() => {
                        setIsIzmenaKolicineDialogOpen(true)
                    }}
                >
                    {isIzmenaKolicine ? (
                        <CircularProgress size={`1rem`} />
                    ) : (
                        <Edit />
                    )}
                </IconButton>
            </TableCell>
            <TableCell sx={{ whiteSpace: 'nowrap', textAlign: `right` }}>
                {formatNumber(props.item.priceWithVAT)} RSD
            </TableCell>
            <TableCell sx={{ whiteSpace: 'nowrap', textAlign: `right` }}>
                {formatNumber(props.item.valueWithVAT)} RSD
            </TableCell>
            <TableCell>
                <IconButton
                    sx={{ p: 0 }}
                    disabled={isRemoving || isIzmenaKolicine || props.disabled}
                    onClick={() => {
                        setIsRemoving(true)

                        webApi
                            .delete(
                                `/products/${props.item.productId}/remove-from-cart`,
                                {
                                    data: {
                                        id: props.item.productId,
                                        oneTimeHash: cartId,
                                    },
                                }
                            )
                            .then(() => {
                                props.reloadKorpa()
                                toast.success(`Proizvod je uklonjen iz korpe`)
                            })
                            .catch((err) => handleApiError(err))
                            .finally(() => {})
                    }}
                >
                    {isRemoving ? (
                        <CircularProgress size={`1rem`} />
                    ) : (
                        <Delete />
                    )}
                </IconButton>
            </TableCell>
        </TableRow>
    )
}
