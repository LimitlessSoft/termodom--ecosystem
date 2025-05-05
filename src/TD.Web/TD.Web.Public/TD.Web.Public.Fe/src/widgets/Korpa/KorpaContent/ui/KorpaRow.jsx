import {
    CircularProgress,
    IconButton,
    TableCell,
    TableRow,
    Typography,
} from '@mui/material'
import { toast } from 'react-toastify'
import { CookieNames } from '@/app/constants'
import useCookie from 'react-use-cookie'
import { useEffect, useState } from 'react'
import { formatNumber } from '@/app/helpers/numberHelpers'
import { KorpaIzmenaKolicineDialog } from './KorpaIzmenaKolicineDialog'
import { handleApiError, webApi } from '@/api/webApi'
import { Delete, Edit } from '@mui/icons-material'

export const KorpaRow = (props) => {
    const [cartId] = useCookie(CookieNames.cartId)
    const [isRemoving, setIsRemoving] = useState(false)
    const [isIzmenaKolicine, setIsIzmenaKolicine] = useState(false)
    const [isIzmenaKolicineDialogOpen, setIsIzmenaKolicineDialogOpen] =
        useState(false)

    useEffect(() => {
        setIsIzmenaKolicine(false)
        setIsRemoving(false)
    }, [props.item])

    const handleCloseDialog = () => setIsIzmenaKolicineDialogOpen(false)

    return (
        <TableRow>
            <TableCell>{props.item.name}</TableCell>
            <TableCell sx={{ whiteSpace: 'nowrap', textAlign: `right` }}>
                <KorpaIzmenaKolicineDialog
                    isOpen={isIzmenaKolicineDialogOpen}
                    onConfirm={(value) => {
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
                                handleCloseDialog()
                            })
                            .catch(handleApiError)
                            .finally(() => setIsIzmenaKolicine(false))
                    }}
                    onClose={handleCloseDialog}
                    quantity={props.item.quantity}
                    baseUnit={props.item.unit}
                    alternateUnit={props.item.alternateUnit}
                    oneAlternatePackageEquals={
                        props.item.oneAlternatePackageEquals
                    }
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
