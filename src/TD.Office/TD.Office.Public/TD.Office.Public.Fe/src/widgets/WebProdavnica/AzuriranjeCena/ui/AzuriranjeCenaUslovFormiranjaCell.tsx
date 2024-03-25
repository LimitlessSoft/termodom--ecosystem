import { Button, Chip, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle, Grid, MenuItem, TextField, Typography } from "@mui/material"
import { useState } from "react"
import { toast } from "react-toastify"
import { ApiBase, ContentType, fetchApi } from "@/app/api"
import { IAzuriranjeCenaUslovFormiranjaCellProps } from "../models/IAzuriranjeCenaUslovFormiranjaCellProps"
import { IAzuriranjeCenaUslovFormiranjaCellRequest } from "../models/IAzuriranjeCenaUslovFormiranjaCellRequest"

export const AzuriranjeCenaUslovFormiranjaCell = (props: IAzuriranjeCenaUslovFormiranjaCellProps): JSX.Element => {

    const [isUpdating, setIsUpdating] = useState(false)
    const [isDialogOpen, setIsDialogOpen] = useState(false)
    const [data, setData] = useState(props.data)
    const [request, setRequest] = useState<IAzuriranjeCenaUslovFormiranjaCellRequest>({
        id: props.data.uslovFormiranjaWebCeneId,
        webProductId: props.data.id,
        type: props.data.uslovFormiranjaWebCeneType,
        modifikator: props.data.uslovFormiranjaWebCeneModifikator
    })

    const [isLoadingSuggestions, setIsLoadingSuggestions] = useState(false)
    const [suggestions, setSuggestions] = useState<any[]>([])

    return (
        <Grid>
            <Dialog
                onClose={() => {
                    setIsDialogOpen(false)
                }}
                open={isDialogOpen}>
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
                                    type: Number(e.target.value)
                                })
                            }}
                            helperText={`Izaberite uslov formiranja cene`}>
                                <MenuItem value={0}>Nabavna cena +%</MenuItem>
                                <MenuItem value={1}>Prodajna cena -%</MenuItem>
                                <MenuItem value={2}>Referentni Web Proizvod</MenuItem>
                        </TextField>
                        {
                            request.type == 2  ?
                                <Grid container>
                                    <Grid item sm={12}>
                                        <TextField
                                            type={`text`}
                                            defaultValue={props.data.uslovFormiranjaWebCeneModifikator}
                                            label={`Referentni proizvod`}
                                            disabled={isLoadingSuggestions}
                                            onChange={(e) => {
                                                if(e.target.value != null && e.target.value.length >= 4) {
                                                    setIsLoadingSuggestions(true)
                                                    fetchApi(ApiBase.Main, `/web-azuriraj-cene-uslov-formiranja-min-web-osnova-product-suggestion?SearchText=${e.target.value}`)
                                                    .then((response) => {
                                                        setSuggestions(response)
                                                        setIsLoadingSuggestions(false)
                                                    })
                                                }
                                                setRequest({
                                                    ...request,
                                                    modifikator: Number(e.target.value)
                                                })
                                            }}
                                            placeholder={`Započnite kucanje naziva proizvoda...`}>
                                        </TextField>
                                    </Grid>
                                    <Grid container spacing={1} sx={{
                                        my: 2
                                    }}>
                                        {
                                            suggestions.map((suggestion) => {
                                                return (
                                                    <Grid item key={suggestion.key}>
                                                        <Chip label={suggestion.value} variant="outlined" onClick={() => {
                                                            setRequest({
                                                                ...request,
                                                                modifikator: suggestion.key
                                                            })
                                                        }} />
                                                    </Grid>
                                                )
                                            })
                                        }
                                    </Grid>
                                </Grid>
                                :
                                <TextField
                                    type={`text`}
                                    defaultValue={props.data.uslovFormiranjaWebCeneModifikator}
                                    label={`Modifikator`}
                                    onChange={(e) => {
                                        setRequest({
                                            ...request,
                                            modifikator: Number(e.target.value)
                                        })
                                    }}
                                    helperText={`Modifikator (možete staviti vrednost u minusu)`}>
                                </TextField>
                        }
                        <Grid container direction={`column`}>
                            <Typography>Buduca platinum cena: 250rsd</Typography>
                            <Typography>Buduca gold cena: 250rsd</Typography>
                            <Typography>Buduca silver cena: 250rsd</Typography>
                            <Typography>Buduca iron cena: 250rsd</Typography>
                        </Grid>
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => {
                            setIsUpdating(true)
                            fetchApi(ApiBase.Main, `/web-azuriraj-cene-uslovi-formiranja-min-web-osnova`, {
                                method: 'PUT',
                                body: request,
                                contentType: ContentType.ApplicationJson
                            }).then(() => {
                                toast.success(`Uspešno ažuriran uslov formiranja cene!`)
                                setData({
                                    ...data,
                                    uslovFormiranjaWebCeneModifikator: request.modifikator,
                                    uslovFormiranjaWebCeneType: request.type
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
                        }}>Potvrdi</Button>
                        <Button onClick={() => {
                            setIsDialogOpen(false)
                        }}>Odustani</Button>
                    </DialogActions>
            </Dialog>
            <Button
                disabled={isUpdating || props.disabled}
                startIcon={isUpdating ? <CircularProgress size={`1em`} /> : null}
                color={`info`} variant={`contained`} onClick={() => {
                setIsDialogOpen(true)
            }}>
            {
                data.uslovFormiranjaWebCeneType == 0 ? `Nabavna cena ` : `Prodajna cena `
            }
            {
                data.uslovFormiranjaWebCeneType == 0 ? `+ ` : `- `
            }
            {
                data.uslovFormiranjaWebCeneModifikator
            }%</Button>
        </Grid>
    )
}