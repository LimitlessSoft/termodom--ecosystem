import {
    Box,
    Button,
    Checkbox,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    IconButton,
    TableCell,
    TableRow,
    TextField,
} from '@mui/material'
import { adminApi, handleApiError } from '@/apis/adminApi'
import { ArrowDownward, ArrowUpward, Delete, Edit } from '@mui/icons-material'
import { toast } from 'react-toastify'
import { useState } from 'react'

export const PodesavanjaKalkulatorRow = (props: any) => {
    const [updating, setUpdating] = useState<boolean>(false)

    const [quantityEditingDialogOpened, setQuantityEditingDialogOpened] =
        useState<boolean>(false)

    const [newQuantity, setNewQuantity] = useState<any>(undefined)

    return (
        <TableRow>
            <TableCell>{props.item.productName}</TableCell>
            <TableCell>
                <Dialog
                    open={quantityEditingDialogOpened}
                    onClose={() => {
                        setQuantityEditingDialogOpened(false)
                    }}
                >
                    <DialogTitle>Izmena kolicine</DialogTitle>
                    <DialogContent>
                        <Box p={2}>
                            <TextField
                                value={newQuantity}
                                onChange={(e) => {
                                    setNewQuantity(e.target.value)
                                }}
                                label={`Kolicina`}
                            />
                        </Box>
                    </DialogContent>
                    <DialogActions>
                        <Button
                            disabled={updating}
                            variant={`contained`}
                            onClick={() => {
                                setUpdating(true)
                                adminApi
                                    .put(
                                        `/calculator-items/${props.item.id}/quantity`,
                                        {
                                            quantity: newQuantity,
                                        }
                                    )
                                    .then(() => {
                                        props.onRowUpdated()
                                        setQuantityEditingDialogOpened(false)
                                    })
                                    .catch(handleApiError)
                                    .finally(() => {
                                        setUpdating(false)
                                    })
                            }}
                        >
                            Sacuvaj
                        </Button>
                        <Button
                            onClick={() => {
                                setQuantityEditingDialogOpened(false)
                            }}
                        >
                            Odustani
                        </Button>
                    </DialogActions>
                </Dialog>
                <Button
                    variant={`contained`}
                    endIcon={<Edit />}
                    onClick={() => {
                        setNewQuantity(props.item.quantity)
                        setQuantityEditingDialogOpened(true)
                    }}
                >
                    {props.item.quantity}
                </Button>
            </TableCell>
            <TableCell>{props.item.unit}</TableCell>
            <TableCell>
                <Checkbox
                    checked={props.item.isPrimary}
                    disabled={props.item.isPrimary || updating}
                    onChange={() => {
                        setUpdating(true)
                        adminApi
                            .post(
                                `/calculator-items/${props.item.id}/mark-primary`
                            )
                            .then(() => props.onRowUpdated())
                            .catch(handleApiError)
                            .finally(() => {
                                setUpdating(false)
                            })
                    }}
                />
            </TableCell>
            <TableCell>
                <IconButton
                    disabled={updating}
                    onClick={() => {
                        if (props.item.isPrimary) {
                            toast.error('Ne mozes obrisati glavni proizvod!')
                            return
                        }
                        setUpdating(true)
                        adminApi
                            .delete(`/calculator-items/${props.item.id}`)
                            .then(() => props.onRowUpdated())
                            .catch(handleApiError)
                            .finally(() => {
                                setUpdating(false)
                            })
                    }}
                >
                    <Delete />
                </IconButton>
                {!props.isFirst && (
                    <IconButton
                        disabled={updating}
                        onClick={() => {
                            setUpdating(true)
                            adminApi
                                .put(`/calculator-items-move`, {
                                    id: props.item.id,
                                    direction: `up`,
                                })
                                .then(() => props.onRowUpdated())
                                .catch(handleApiError)
                                .finally(() => {
                                    setUpdating(false)
                                })
                        }}
                    >
                        <ArrowUpward />
                    </IconButton>
                )}
                {!props.isLast && (
                    <IconButton
                        disabled={updating}
                        onClick={() => {
                            setUpdating(true)
                            adminApi
                                .put(`/calculator-items-move`, {
                                    id: props.item.id,
                                    direction: `down`,
                                })
                                .then(() => props.onRowUpdated())
                                .catch(handleApiError)
                                .finally(() => {
                                    setUpdating(false)
                                })
                        }}
                    >
                        <ArrowDownward />
                    </IconButton>
                )}
            </TableCell>
        </TableRow>
    )
}
