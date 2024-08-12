import {
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    Grid,
    MenuItem,
    TextField,
    Typography,
} from '@mui/material'
import { AzuriranjeCenaUslovFormiranjaReferentniProizvod } from './AzuriranjeCenaUslovFormiranjaReferentniProizvod'
import { IAzuriranjeCenaUslovFormiranjaCellRequest } from '../models/IAzuriranjeCenaUslovFormiranjaCellRequest'
import { IAzuriranjeCenaUslovFormiranjaCellProps } from '../models/IAzuriranjeCenaUslovFormiranjaCellProps'
import { toast } from 'react-toastify'
import { useState } from 'react'
import { officeApi } from '@/apis/officeApi'

export const AzuriranjeCenaUslovFormiranjaCell = (
    props: IAzuriranjeCenaUslovFormiranjaCellProps
) => {
    const isInitialReferentnaCena = props.data.uslovFormiranjaWebCeneType == 2
    const [isUpdating, setIsUpdating] = useState(false)
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [data, setData] = useState(props.data)
    const [request, setRequest] =
        useState<IAzuriranjeCenaUslovFormiranjaCellRequest>({
            id: props.data.uslovFormiranjaWebCeneId,
            webProductId: props.data.id,
            type: props.data.uslovFormiranjaWebCeneType,
            modifikator: props.data.uslovFormiranjaWebCeneModifikator,
        })

    const uslovLabel = (modifikator: number) => {
        switch (data.uslovFormiranjaWebCeneType) {
            case 0:
                return `Nabavna cena + ${modifikator}%`
            case 1:
                return `Prodajna cena - ${modifikator}%`
            case 2:
                return `Cena na upit`
        }
    }

    return (
        <Grid>
            <Dialog
                onClose={() => {
                    setIsDialogOpen(false)
                }}
                open={isDialogOpen}
            >
                <DialogTitle>
                    Uslov formiranja Min Web Osnove - {props.data.naziv}
                </DialogTitle>
                <DialogContent>
                    <TextField
                        id={`uslov-formiranja`}
                        select
                        required
                        defaultValue={props.data.uslovFormiranjaWebCeneType}
                        label={`Uslov formiranja cene`}
                        sx={{ minWidth: 350, my: 1 }}
                        onChange={(e) => {
                            setRequest({
                                ...request,
                                type: Number(e.target.value),
                            })
                        }}
                        helperText={`Izaberite uslov formiranja cene`}
                    >
                        <MenuItem value={0}>Nabavna cena +%</MenuItem>
                        <MenuItem value={1}>Prodajna cena -%</MenuItem>
                        <MenuItem value={2}>Cena na upit</MenuItem>
                    </TextField>
                    {request.type != 2 && (
                        <TextField
                            type={`text`}
                            defaultValue={
                                props.data.uslovFormiranjaWebCeneModifikator
                            }
                            label={`Modifikator`}
                            onChange={(e) => {
                                setRequest({
                                    ...request,
                                    modifikator: Number(e.target.value),
                                })
                            }}
                            helperText={`Modifikator (možete staviti vrednost u minusu)`}
                        ></TextField>
                    )}
                    <Grid container direction={`column`}>
                        <Typography>
                            Buduca platinum cena: to be implemented
                        </Typography>
                        <Typography>
                            Buduca gold cena: to be implemented
                        </Typography>
                        <Typography>
                            Buduca silver cena: to be implemented
                        </Typography>
                        <Typography>
                            Buduca iron cena: to be implemented
                        </Typography>
                    </Grid>
                </DialogContent>
                <DialogActions>
                    <Button
                        variant={`contained`}
                        onClick={() => {
                            setIsUpdating(true)

                            officeApi
                                .put(
                                    `/web-azuriraj-cene-uslovi-formiranja-min-web-osnova`,
                                    request
                                )
                                .then(() => {
                                    toast.success(
                                        `Uspešno ažuriran uslov formiranja cene!`
                                    )
                                    setData({
                                        ...data,
                                        uslovFormiranjaWebCeneModifikator:
                                            request.modifikator,
                                        uslovFormiranjaWebCeneType:
                                            request.type,
                                    })
                                    props.onSuccessUpdate()
                                    setIsDialogOpen(false)
                                })
                                .catch(() => {
                                    props.onErrorUpdate()
                                })
                                .finally(() => {
                                    setIsUpdating(false)
                                })
                        }}
                    >
                        Potvrdi
                    </Button>
                    <Button
                        onClick={() => {
                            setIsDialogOpen(false)
                        }}
                    >
                        Odustani
                    </Button>
                </DialogActions>
            </Dialog>
            <Button
                disabled={isUpdating || props.disabled}
                startIcon={
                    isUpdating ? <CircularProgress size={`1em`} /> : null
                }
                color={`info`}
                variant={`contained`}
                onClick={() => {
                    setIsDialogOpen(true)
                }}
            >
                {uslovLabel(data.uslovFormiranjaWebCeneModifikator)}
            </Button>
        </Grid>
    )
}
