import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid, MenuItem, TextField, Typography } from "@mui/material"
import { useState } from "react"
import { toast } from "react-toastify"

interface IAzuriranjeCenaUslovFormiranjaCellProps {
    naziv: string
}

export const AzuriranjeCenaUslovFormiranjaCell = (props: IAzuriranjeCenaUslovFormiranjaCellProps): JSX.Element => {

    const [isDialogOpen, setIsDialogOpen] = useState(false)

    return (
        <Grid>
            <Dialog
                onClose={() => {
                    setIsDialogOpen(false)
                }}
                open={isDialogOpen}>
                    <DialogTitle>
                        Uslov formiranja Min Web Osnove - {props.naziv}
                    </DialogTitle>
                    <DialogContent>
                        <TextField
                            id={`uslov-formiranja`}
                            select
                            required
                            label={`Uslov formiranja cene`}
                            sx={{ minWidth: 350 }}
                            onChange={() => {
                                toast.warning(`Ova funkcionalnost još uvek nije implementirana.`)
                            }}
                            helperText={`Izaberite uslov formiranja cene`}>
                                <MenuItem value={`NabavnaCenaPlusProcenat`}>Nabavna cena +%</MenuItem>
                                <MenuItem value={`ProdajnaCenaPlusProcenat`}>Prodajna cena +%</MenuItem>
                        </TextField>
                        <TextField
                            type={`text`}
                            required
                            label={`Modifikator`}
                            onChange={() => {
                                toast.warning(`Ova funkcionalnost još uvek nije implementirana.`)
                            }}
                            helperText={`Modifikator (možete staviti vrednost u minusu)`}>
                        </TextField>
                        <Grid container direction={`column`}>
                            <Typography>Buduca platinum cena: 250rsd</Typography>
                            <Typography>Buduca gold cena: 250rsd</Typography>
                            <Typography>Buduca silver cena: 250rsd</Typography>
                            <Typography>Buduca iron cena: 250rsd</Typography>
                        </Grid>
                    </DialogContent>
                    <DialogActions>
                        <Button onClick={() => {
                            toast.warning(`Ova funkcionalnost još uvek nije implementirana.`)
                            setIsDialogOpen(false)
                        }}>Potvrdi</Button>
                        <Button onClick={() => {
                            setIsDialogOpen(false)
                        }}>Odustani</Button>
                    </DialogActions>
            </Dialog>
            <Button color={`info`} variant={`contained`} onClick={() => {
                setIsDialogOpen(true)
            }}>Nabavna cena * 1.2</Button>
        </Grid>
    )
}